using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
	[CreateAssetMenu(fileName = "New ReplaceClipData", menuName = "ScriptableObject/ReplaceAnimationClipData", order = 1)]
	public class ReplaceAnimationClipData : ScriptableObject
	{
		public string replaceName;
		public List<ClipData> clipDatas;
		public List<Blend1DClipData> blend1DClipDatas;
		public List<Blend2DClipData> blend2DClipDatas;
		public List<AnimatorController> ACs;
	}
}