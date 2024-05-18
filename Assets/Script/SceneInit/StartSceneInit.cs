using Framework;
using UnityEngine;

public class StartSceneInit : MonoBehaviour
{
	private UpdateAfterChangeSence UACS;
	public Transform originPos;
	private void Awake()
	{
		UACS = GameObject.FindAnyObjectByType<UpdateAfterChangeSence>();
		float[] vec = new float[3];
		vec[0] = originPos.position.x;
		vec[1] = originPos.position.y;
		vec[2] = originPos.position.z;
		CustomModelSystem.Instance.GetOrAddModel<PlayerModel>().playerPos.Value = vec;
		UACS.SetHideObjectActive(true);
		UACS.UpdateEntityManager();
	}
}
