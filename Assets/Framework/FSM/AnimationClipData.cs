using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
	[Serializable]
	public struct ClipData
	{
		public String StateName;
		public AnimationClip clip;
	}

	[Serializable]
	public struct Blend1DClipData
	{
		public String StateName;
		public AnimationClip[] clips;
		public float[] positions;
	}

	[Serializable]
	public struct Blend2DClipData
	{
		public String StateName;
		public AnimationClip[] clips;
		public Vector2[] positions;
	}

	[Serializable]
	public struct AnimatorController 
	{
		public String StateName;
		public RuntimeAnimatorController AC;
	}

	[CreateAssetMenu(fileName = "New ClipData", menuName = "ScriptableObject/AnimationClipData", order = 0)]
	public class AnimationClipData : ScriptableObject
	{
		public AvatarMask avatarMask;
		public List<ClipData> clipDatas;
		public List<Blend1DClipData> blend1DClipDatas;
		public List<Blend2DClipData> blend2DClipDatas;
		public List<AnimatorController> ACs;
	}
}
