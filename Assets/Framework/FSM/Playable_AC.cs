using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace FSM 
{
	public class PlayableAC : PlayableBehaviour
	{
		private PlayableGraph m_graph;
		private AnimatorControllerPlayable m_playableAC;
		
		public void init(RuntimeAnimatorController playableAC)
		{
			m_playableAC = AnimatorControllerPlayable.Create(m_graph, playableAC);
		}
public override void OnPlayableCreate(Playable playable)
		{
			base.OnPlayableCreate(playable);

		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
			base.PrepareFrame(playable, info);
		}
	}
}
