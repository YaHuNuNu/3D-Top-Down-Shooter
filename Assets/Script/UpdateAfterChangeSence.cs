using UnityEngine;

public class UpdateAfterChangeSence : MonoBehaviour
{
	public GameObject hideGameObject;
	private EntityManager entityManager;
	private void Start()
	{
		entityManager = EntityManager.Instance;
		//DontDestroyOnLoad�еĶ������һ��ʼ��inactive�ģ���ʹ����Ϊactive����awakeҲ����ִ�С���������м������
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
