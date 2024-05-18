using UnityEngine;

public class EntityManager : MonoBehaviour
{
	public static EntityManager Instance { get; private set; }
	[HideInInspector]
	public PlayerController playerCtl { get; private set; }
	public SwordEnemyController[] swordEnemyCtl { get; private set; }
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(Instance.gameObject);

		UpdateEntity();
	}

	public void UpdateEntity()
	{
		playerCtl = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();

		swordEnemyCtl = GameObject.FindGameObjectWithTag("SwordEnemy")?.GetComponents<SwordEnemyController>();
	}
}
