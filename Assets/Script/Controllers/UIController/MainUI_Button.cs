using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainUI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private Vector3 originScale;
	private Vector3 targetScale;
	private Vector3 currentScale;
	public float multiple = 0.92f;
	public float speed = 1.5f;

	private void Start()
	{
		originScale = transform.localScale;
		targetScale = originScale;
		currentScale = originScale;
	}
	private void Update()
	{
		currentScale = Vector3.MoveTowards(currentScale, targetScale, speed * Time.deltaTime);
		transform.localScale = currentScale;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		targetScale = originScale * multiple;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		targetScale = originScale;
	}

	public void Load()
	{
		SceneManager.LoadScene("TransitionScene");
	}

	public void DeleteFile()
	{
		File.Delete(Path.Combine(Application.persistentDataPath, "GameData.json"));
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
	}
}
