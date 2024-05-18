using UnityEngine;

public class DoorTriggerFunctions : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
			CustomLoadScene.Load("LevelScene");
	}
}
