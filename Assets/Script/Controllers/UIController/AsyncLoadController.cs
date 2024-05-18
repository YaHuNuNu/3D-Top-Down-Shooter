using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadController : MonoBehaviour
{
	private string sceneName;
	public Image slider;
	public TextMeshProUGUI percentage;
	private AsyncOperation operation;
	private InputCtrls inputCtrls;
	private float minLoadSpeed = 1 / 0.5f;
	private void Start()
	{
		inputCtrls = InputManager.instance.InputCtrls;
		sceneName = CustomLoadScene.sceneName;
		StartCoroutine(Load());
	}

	private IEnumerator Load()
	{
		operation = SceneManager.LoadSceneAsync(sceneName);
		operation.allowSceneActivation = false;
		float progress = 0;
		while (!operation.isDone)
		{
			float deltaP = operation.progress >= 0.8999f ? 1 - progress : operation.progress - progress;
			progress += deltaP > Time.deltaTime * minLoadSpeed ? Time.deltaTime * minLoadSpeed : deltaP;

			if (progress >= 1)
				progress = 1;
			slider.fillAmount = progress;
			if (progress == 1)
			{
				percentage.text = "Press The Left Mouse Button!";
				if (inputCtrls.Base.Fire.triggered)
					operation.allowSceneActivation = true;

			}
			else
				percentage.text = (progress * 100).ToString("f0") + "%";
			yield return null;
		}
	}
}
