using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace FSM
{
	public class Playable2DBlend : PlayableBehaviour
	{
		private PlayableGraph m_graph;
		private AnimationMixerPlayable m_mixer;
		private Playable[] playables;
		private Vector2[] m_positions;
		private float[] weights;
		private float[] animationLengths;
		private float deltaTime = 0.015f;
		private Vector2 currentParameter = Vector2.zero;
		private const float DELTA = 0.000001f;

		private float Timer = 0;
		private Vector2 ParameterValue = Vector2.zero;

		public void Init(Playable[] _playables, Vector2[] positions)
		{
			this.playables = _playables;
			m_mixer.SetInputCount(_playables.Length);
			m_positions = positions;
			weights = new float[m_positions.Length];
			animationLengths = new float[m_positions.Length];
			for (int i = 0; i < _playables.Length; i++)
			{
				m_mixer.ConnectInput(i, _playables[i], 0);
				//temp
				animationLengths[i] = ((AnimationClipPlayable)_playables[i]).GetAnimationClip().length;
				_playables[i].Play();
			}
		}

		public override void OnPlayableCreate(Playable playable)
		{
			base.OnPlayableCreate(playable);
			m_graph = playable.GetGraph();
			m_mixer = AnimationMixerPlayable.Create(m_graph);
			playable.ConnectInput(0, m_mixer, 0);
		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
			base.PrepareFrame(playable, info);
			deltaTime = info.deltaTime;
			if (Timer > 0)
			{
				SetParameterGrad(ParameterValue, Timer);
				Timer -= info.deltaTime;
			}
		}
		public void SetParameter(float x, float y)
		{
			float totalWeight = 0;
			float mixLength = 0;
			for (int i = 0; i < m_positions.Length; i++)
			{
				weights[i] = 1.0f / (Mathf.Abs(x - m_positions[i].x) + Mathf.Abs(y - m_positions[i].y) + DELTA);
				totalWeight += weights[i];
				
			}

			for (int i = 0; i < m_positions.Length; i++)
			{
				float _weight = weights[i] / totalWeight;
				m_mixer.SetInputWeight(i, _weight);
				mixLength += _weight * animationLengths[i];
			}

			for (int i = 0; i < m_positions.Length; i++)
			{
				playables[i].SetSpeed(animationLengths[i] / mixLength);
			}
		}

		public void SetParameterGrad(float x, float y, float duration)
		{
			SetParameterGrad(new Vector2(x, y), duration);
		}

		public void SetParameterGrad(Vector2 parameter, float duration)
		{
			Vector2 grad = (parameter - currentParameter) / duration * deltaTime + currentParameter;
			currentParameter = grad;
			SetParameter(grad.x, grad.y);
		}

		//线性下降的参数设置
		public void SetParameterSlow(float x, float y, float duration)
		{
			ParameterValue.x = x;
			ParameterValue.y = y;
			Timer = duration;
		}

		public void SetParameterSlow(Vector2 parameter, float duration)
		{
			ParameterValue = parameter;
			Timer = duration;
		}
	}
}