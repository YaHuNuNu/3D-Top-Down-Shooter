using System.Collections;
using UnityEngine;
using Framework;

public class TransitionSceneController : MonoBehaviour
{
	private void Awake()
	{
		StartCoroutine(LoadNextScene());
	}

	private IEnumerator LoadNextScene()
	{
		yield return new WaitForEndOfFrame();
		CustomLoadScene.Load(CustomModelSystem.Instance.GetOrAddModel<PlayerModel>().CurrentScene);
	}
}
