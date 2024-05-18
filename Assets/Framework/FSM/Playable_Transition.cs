using System;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace FSM
{
	public class TwoPlayableTransition : PlayableBehaviour
	{
		private int m_currentPort = 0;
		private PlayableGraph m_graph;
		private AnimationMixerPlayable m_mixer;
		public float duration = -1;
		private float m_fadeLength = -1;

		public void Init(Playable startState)
		{
			m_mixer.ConnectInput(0, startState, 0);
			startState.Play();
			m_mixer.SetInputWeight(0, 1);
			m_mixer.SetInputWeight(1, 0);
		}

		public void DisConnectPlayable(Playable playable)
		{
			for (int i = 0; i < 2; i++)
			{
				if (m_mixer.GetInput(i).Equals(playable))
					m_mixer.DisconnectInput(i);
			}
		}
		public override void OnPlayableCreate(Playable playable)
		{
			base.OnPlayableCreate(playable);
			m_graph = playable.GetGraph();
			m_mixer = AnimationMixerPlayable.Create(m_graph, 2);
			playable.ConnectInput(0, m_mixer, 0);
			playable.SetInputWeight(0, 1);
		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
			base.PrepareFrame(playable, info);
			if (duration >= 0)
			{
				duration -= info.deltaTime;
				__Crossfade();
			}
		}

		public void Crossfade(Playable target, float fadeLength)
		{
			m_currentPort = 1 - m_currentPort;
			m_mixer.DisconnectInput(m_currentPort);
			m_mixer.ConnectInput(m_currentPort, target, 0);
			target.Play();
			duration = fadeLength;
			m_fadeLength = fadeLength;
		}

		private void __Crossfade()
		{
			float weight = 0;
			if (duration <= 0)
			{
				weight = 1;
				var temp = m_mixer.GetInput(1 - m_currentPort);
				if (!temp.Equals(Playable.Null))
					temp.Pause();
				duration = m_fadeLength = -1;
			}
			else
				weight = 1 - duration / m_fadeLength;

			m_mixer.SetInputWeight(m_currentPort, weight);
			m_mixer.SetInputWeight(1 - m_currentPort, 1 - weight);
		}
	}

	//用于解决多动画过渡问题（未解决）
	public class PlayableTransition : PlayableBehaviour
	{
		private ScriptPlayable<TwoPlayableTransition> m_currentPlayable;
		private TwoPlayableTransition m_current;
		private ScriptPlayable<TwoPlayableTransition> m_backupPlayable;
		private TwoPlayableTransition m_backup;
		private Playable thisPlayable;

		private Action m_update;
		private Func<float> m_getWeight;
		public int upPort = 0;

		public void SetUpdateFunc(Action update)
		{
			m_update += update;
		}
		public void SetGetWeightFunc(Func<float> getWeight)
		{
			m_getWeight = getWeight;
		}

		public void Init(Playable startState)
		{
			m_current.Init(startState);
		}

		public override void OnPlayableCreate(Playable playable)
		{
			base.OnPlayableCreate(playable);
			thisPlayable = playable;

			m_currentPlayable = ScriptPlayable<TwoPlayableTransition>.Create(playable.GetGraph(), 1);
			m_current = m_currentPlayable.GetBehaviour();
			playable.ConnectInput(0, m_currentPlayable, 0);

			m_backupPlayable = ScriptPlayable<TwoPlayableTransition>.Create(playable.GetGraph(), 1);
			m_backup = m_backupPlayable.GetBehaviour();
			m_backupPlayable.Pause();
		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
			base.PrepareFrame(playable, info);
			float upWeight = m_getWeight.Invoke();
			if (upWeight <= 0)
			{
				playable.GetOutput(0).SetInputWeight(upPort, 0);
				return;
			}
			playable.GetOutput(0).SetInputWeight(upPort, upWeight);
			m_update.Invoke();
		}



		public void Crossfade(Playable target, float fadeLength)
		{
			if (m_current.duration >= 0)
			{
				var tempPlayable = m_currentPlayable;
				m_currentPlayable = m_backupPlayable;
				m_backupPlayable = tempPlayable;
				var temp = m_current;
				m_current = m_backup;
				m_backup = temp;
				m_currentPlayable.Play();

				thisPlayable.DisconnectInput(0);
				m_backup.DisConnectPlayable(m_currentPlayable);
				thisPlayable.ConnectInput(0, m_currentPlayable, 0);
				m_current.Crossfade(m_backupPlayable, 0);
			}
			m_current.DisConnectPlayable(target);
			m_backup.DisConnectPlayable(target);
			m_current.Crossfade(target, fadeLength);
		}
	}
}
