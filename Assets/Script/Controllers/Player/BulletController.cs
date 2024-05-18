using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IController
{
	public float speed;
	private Rigidbody m_rb;
	private BoxCollider m_collider;
	private Queue<GameObject> bulletPool;
	public GameObject hitFXPrefab;
	private GameObject hitFX;

	private PlayerModel playerModel;

	private Coroutine bulletCoroutine;

	private void Awake()
	{
		m_rb = GetComponent<Rigidbody>();
		m_collider = GetComponent<BoxCollider>();
		playerModel = this.GetOrAddModel<PlayerModel>();
		hitFX = GameObject.Instantiate(hitFXPrefab, this.transform);
		hitFX.SetActive(false);
	}

	private void Start()
	{
		bulletPool = this.GetQueue("Bullet");
	}

	private void OnEnable()
	{
		bulletCoroutine = StartCoroutine(BulletTimer(1.5f));
		m_rb.constraints = RigidbodyConstraints.None;
		m_rb.velocity = transform.forward * speed;
	}
	private void OnDisable()
	{
		m_rb.velocity = Vector3.zero;
	}

	private void OnCollisionEnter(Collision collision)
	{

		GenerateDamage(collision);
		m_rb.constraints = RigidbodyConstraints.FreezeAll;
		hitFX.transform.position = collision.contacts[0].point;
		hitFX.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
		hitFX.SetActive(true);
		hitFX.GetComponent<ParticleSystem>().Play();
		StartCoroutine(FXTimer(1));
		StopCoroutine(bulletCoroutine);
	}

	private void GenerateDamage(Collision collision)
	{
		if (collision.gameObject.tag == "SwordEnemy")
		{
			float ATK = playerModel.ATK[(int)playerModel.weaponType.Value].Value;
			collision.gameObject.GetComponent<EntityController>().BeDamaged(ATK);
		}
	}

	private IEnumerator BulletTimer(float time)
	{
		yield return new WaitForSeconds(time);
		bulletPool.Enqueue(this.gameObject);
		this.gameObject.SetActive(false);
	}

	private IEnumerator FXTimer(float time)
	{
		yield return new WaitForSeconds(time);
		hitFX.GetComponent<ParticleSystem>().Stop();
		hitFX.SetActive(false);

		bulletPool.Enqueue(this.gameObject);
		this.gameObject.SetActive(false);
	}

}
