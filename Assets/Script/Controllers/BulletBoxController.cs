using Framework;
using System;
using System.Collections;
using UnityEngine;

public class BulletBoxController : MonoBehaviour
{
	public int[] bulletNum = new int[Enum.GetNames(typeof(WeaponType)).Length];
	public int grenadeNum;
	public GameObject box;
	public GameObject FX;
	private Inventory inventory;
	private float duration;

	private void Awake()
	{
		inventory = CustomModelSystem.Instance.GetOrAddModel<Inventory>();
		duration = FX.GetComponent<ParticleSystem>().duration;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;
		this.GetComponent<BoxCollider>().enabled = false;
		for (int i = 0; i < bulletNum.Length; i++)
		{
			inventory.bulletSum[i].Value += bulletNum[i];
		}
		inventory.grenadeSum.Value += grenadeNum;

		StartCoroutine(_Timer(duration));
	}

	private IEnumerator _Timer(float time)
	{
		GameObject tempFX = GameObject.Instantiate(FX, transform);
		Vector3 pos = tempFX.transform.localPosition;
		pos.y += 0.002f;
		tempFX.transform.localPosition = pos;
		yield return new WaitForSeconds(time);
		Destroy(tempFX);
		Destroy(box);
	}
}
