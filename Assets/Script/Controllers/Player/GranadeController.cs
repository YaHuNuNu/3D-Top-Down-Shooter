using Framework;
using UnityEngine;

public class GranadeController : MonoBehaviour, IController
{
	public float speed;
	public float delayTime;
	public float throwAngle;
	private float m_radian;
	public GameObject explosionFXPrefab;
	private PlayerController playerCtl;
	private Rigidbody m_rb;
	private BoxCollider m_Collider;

	private void Awake()
	{
		playerCtl = EntityManager.Instance.playerCtl;
		m_rb = GetComponent<Rigidbody>();
		m_Collider = GetComponent<BoxCollider>();
		m_radian = throwAngle * 3.1415926f / 180;
	}

	private void OnEnable()
	{
		Vector3 throwDir = playerCtl.transform.forward * Mathf.Cos(m_radian) + playerCtl.transform.up * Mathf.Sin(m_radian);
		m_rb.velocity = throwDir * speed;
		Destroy(gameObject, delayTime);
	}

	private void OnDestroy()
	{
		GameObject FX = GameObject.Instantiate(explosionFXPrefab);
		FX.transform.position = transform.position;
		Destroy(FX, 1);
	}
}
