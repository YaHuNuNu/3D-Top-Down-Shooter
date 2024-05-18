using UnityEngine;

public class UpdateAfterChangeSence : MonoBehaviour
{
	public GameObject hideGameObject;
	private EntityManager entityManager;
	private void Start()
	{
		entityManager = EntityManager.Instance;
		//DontDestroyOnLoad中的对象如果一开始是inactive的，则即使设置为active，其awake也不会执行。在这里进行间接设置
		hideGameObject.SetActive(false);
	}
	public void SetHideObjectActive(bool active)
	{
		hideGameObject.SetActive(active);
	}

	public void UpdateEntityManager()
	{
		entityManager.UpdateEntity();
	}


}
