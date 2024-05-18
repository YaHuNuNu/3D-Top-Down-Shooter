using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public struct ReplaceClip 
{
	public string clipName;
	public Playable playable;
}


namespace FSM
{
	public class CustomPlayableAnimator
	{
		private Animator m_animator;
		private AnimationClipData[] m_clipDatas;
		private ReplaceAnimationClipData[] m_replaceClipDatas;
		private PlayableGraph m_graph;
		private AnimationPlayableOutput m_output;
		private AnimationLayerMixerPlayable m_layerMixer;

		private Dictionary<string, Playable> m_dictionary = new Dictionary<string, Playable>();
		private Dictionary<string, List<ReplaceClip>> m_replaceDictionary = new Dictionary<string, List<ReplaceClip>>();

		private IMachineConstructor m_constructor;
		private IMachine[] rootMachines;

		public CustomPlayableAnimator(UnityEngine.Object entity, Animator animator, AnimationClipData[] clipDatas, ReplaceAnimationClipData[] replaceClipDatas, IMachineConstructor constructor)
		{
			m_animator = animator;
			m_clipDatas = clipDatas;
			m_replaceClipDatas = replaceClipDatas;
			m_constructor = constructor;
			Init(entity);
		}

		private void Init(UnityEngine.Object entity)
		{
			IMachine[] tempRootMachines = m_constructor.Create(entity);
			//调整Machine顺序，按layer从上到下（从小到大）
			rootMachines = new IMachine[tempRootMachines.Length];
			foreach (var machine in tempRootMachines)
			{
				rootMachines[machine.layer] = machine;
			}

			m_graph = PlayableGraph.Create("CustomAnimator");
			m_graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			m_output = AnimationPlayableOutput.Create(m_graph, "CustomAnimatorOutput", m_animator);
			m_layerMixer = AnimationLayerMixerPlayable.Create(m_graph);
			m_output.SetSourcePlayable(m_layerMixer);
			Load();
		}

		public void GraphPlay() => m_graph.Play();
		public void GraphStop() => m_graph.Stop();
		public void GraphDestroy() => m_graph.Destroy();

		private void Load()
		{
			LoadClip();
			LoadReplaceClip();
			int i = 0;
			foreach (var machine in rootMachines)
			{
				var temp = ScriptPlayable<PlayableTransition>.Create(m_graph, 1);
				temp.GetBehaviour().Init(m_dictionary[machine.originalType.ToString()]);
				machine.playableTransition = temp.GetBehaviour();
				temp.GetBehaviour().SetUpdateFunc(machine.UpdateMachine);
				temp.GetBehaviour().SetGetWeightFunc(machine.GetRootMachineWeight);
				int newPort = m_layerMixer.AddInput(temp, 0, 1);
				temp.GetBehaviour().upPort = newPort;
				m_layerMixer.SetLayerMaskFromAvatarMask((uint)newPort,
					m_clipDatas[i++].avatarMask);

			}

			foreach (var _playable in m_dictionary)
			{
				var state = m_constructor.states[_playable.Key];
				state.playable = _playable.Value;
				state.SetPlayable();
			}

			if(m_replaceClipDatas!= null && m_replaceClipDatas.Length > 0)
				ChangeOverrideAniamtion(m_replaceClipDatas[0].replaceName);
		}

		private void LoadClip()
		{
			foreach (var clipData in m_clipDatas)
			{
				foreach (var clipInfo in clipData.clipDatas)
				{
					Playable playable = AnimationClipPlayable.Create(m_graph, clipInfo.clip);
					playable.Pause();
					m_dictionary.Add(clipInfo.StateName, playable);
				}

				foreach (var clipInfo in clipData.blend1DClipDatas)
				{
					Playable[] playables = new Playable[clipInfo.clips.Length];
					int i = 0;
					foreach (var clip in clipInfo.clips)
						playables[i++] = AnimationClipPlayable.Create(m_graph, clip);

					var blendPlayable = ScriptPlayable<Playable1DBlend>.Create(m_graph, 1);
					blendPlayable.GetBehaviour().Init(playables, clipInfo.positions);
					blendPlayable.Pause();
					m_dictionary.Add(clipInfo.StateName, blendPlayable);
				}

				foreach (var clipInfo in clipData.blend2DClipDatas)
				{
					Playable[] playables = new Playable[clipInfo.clips.Length];
					int i = 0;
					foreach (var clip in clipInfo.clips)
						playables[i++] = AnimationClipPlayable.Create(m_graph, clip);

					var blendPlayable = ScriptPlayable<Playable2DBlend>.Create(m_graph, 1);
					blendPlayable.GetBehaviour().Init(playables, clipInfo.positions);
					blendPlayable.Pause();
					m_dictionary.Add(clipInfo.StateName, blendPlayable);
				}

				foreach (var ACInfo in clipData.ACs)
				{
					Playable AC = AnimatorControllerPlayable.Create(m_graph, ACInfo.AC);
					AC.Pause();
					m_dictionary.Add(ACInfo.StateName, AC);
				}
			}
		}

		private void LoadReplaceClip()
		{
			if (m_replaceClipDatas == null)
				return;
			foreach (var clipData in m_replaceClipDatas)
			{
				List<ReplaceClip> tempList = new List<ReplaceClip>();
				m_replaceDictionary.Add(clipData.replaceName, tempList);

				foreach (var clipInfo in clipData.clipDatas)
				{
					ReplaceClip replaceClip;
					replaceClip.clipName = clipInfo.StateName;
					replaceClip.playable = AnimationClipPlayable.Create(m_graph, clipInfo.clip);
					replaceClip.playable.Pause();
					tempList.Add(replaceClip);
				}

				foreach (var clipInfo in clipData.blend1DClipDatas)
				{
					Playable[] playables = new Playable[clipInfo.clips.Length];
					int i = 0;
					foreach (var clip in clipInfo.clips)
						playables[i++] = AnimationClipPlayable.Create(m_graph, clip);

					var blendPlayable = ScriptPlayable<Playable1DBlend>.Create(m_graph, 1);
					blendPlayable.GetBehaviour().Init(playables, clipInfo.positions);
					blendPlayable.Pause();

					ReplaceClip replaceClip;
					replaceClip.clipName = clipInfo.StateName;
					replaceClip.playable = blendPlayable;
					tempList.Add(replaceClip);
				}

				foreach (var clipInfo in clipData.blend2DClipDatas)
				{
					Playable[] playables = new Playable[clipInfo.clips.Length];
					int i = 0;
					foreach (var clip in clipInfo.clips)
						playables[i++] = AnimationClipPlayable.Create(m_graph, clip);

					var blendPlayable = ScriptPlayable<Playable2DBlend>.Create(m_graph, 1);
					blendPlayable.GetBehaviour().Init(playables, clipInfo.positions);
					blendPlayable.Pause();

					ReplaceClip replaceClip;
					replaceClip.clipName = clipInfo.StateName;
					replaceClip.playable = blendPlayable;
					tempList.Add(replaceClip);
				}

				foreach (var ACInfo in clipData.ACs)
				{
					Playable AC = AnimatorControllerPlayable.Create(m_graph, ACInfo.AC);
					AC.Pause();

					ReplaceClip replaceClip;
					replaceClip.clipName = ACInfo.StateName;
					replaceClip.playable = AC;
					tempList.Add(replaceClip);
				}
			}
		}

		public void ChangeOverrideAniamtion(string name)
		{
			List<ReplaceClip> clipsInfo = m_replaceDictionary[name];
			foreach(ReplaceClip clipInfo in clipsInfo)
			{
				IState state = m_constructor.states[clipInfo.clipName];
				state.playable = clipInfo.playable;
				state.SetPlayable();
			}
		}
	}
}
