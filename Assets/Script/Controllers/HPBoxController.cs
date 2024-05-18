using Framework;
using System.Collections;
using UnityEngine;

public class HPBoxController : MonoBehaviour
{
	[Range(0, 1)]
	public float recoverValue;
	public GameObject box;
	public GameObject FX;
	private PlayerModel playerModel;
	private float duration;
	private void Awake()
	{
		playerModel = CustomModelSystem.Instance.GetOrAddModel<PlayerModel>();
		duration = FX.GetComponent<ParticleSystem>().duration;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;
		this.GetComponent<BoxCollider>().enabled = false;
		playerModel.health.Value = Mathf.Clamp(playerModel.health.Value + recoverValue, 0, 1);

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
