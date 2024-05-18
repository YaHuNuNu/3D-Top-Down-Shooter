using UnityEngine;
using UnityEngine.SceneManagement;
public static class CustomLoadScene
{

	private const string loadSceneName = "LoadScene";
	public static string sceneName;

	public static void Load(string _sceneName)
	{
		sceneName = _sceneName;
		SceneManager.LoadScene(loadSceneName);
	}
}
