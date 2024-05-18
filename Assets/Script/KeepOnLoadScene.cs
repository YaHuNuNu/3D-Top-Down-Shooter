using UnityEngine;

public class KeepOnLoadScene : MonoBehaviour
{
	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
}
