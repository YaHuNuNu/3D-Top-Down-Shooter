using Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour, IController
{
	public GameObject gunParent;
	public GameObject[] weaponObjects;
	public Transform[] gunPoints;
	public GameObject bulletPrefab;
	public GameObject granadePrefab;
	private PlayerModel playerModel;
	private int m_index;
	private Queue<GameObject> bulletPool;

	//ö±µ¯Ç¹É¢²¼
	[Range(0, 1)]
	public float shotgunRange;
	private const float PI_2 = 2 * 3.1415926f;

	private void Awake()
	{
		GameObject parent = new GameObject("BulletPool");
		parent.AddComponent<KeepOnLoadScene>();
		GameObject[] instances = new GameObject[200];
		for (int i = 0; i < instances.Length; i++)
		{
			instances[i] = GameObject.Instantiate(bulletPrefab, parent.transform);
			instances[i].SetActive(false);
		}
		this.PutNewPool("Bullet", instances);
		bulletPool = this.GetQueue("Bullet");
	}

	private void Start()
	{
		weaponObjects[0].SetActive(true);
		playerModel = this.GetOrAddModel<PlayerModel>();
		playerModel.weaponType.RegisterActionWithExcute(EnableWeapon);
		this.GetEvent<ShootEvent>().Register(Shoot);
		this.GetEvent<Event_ExitThrowEvent>().Register(HideGunModel);
		this.GetEvent<AnimationEvent.ThrowAnimEvent>().Register(GenerateGranade);
	}

	private void GenerateGranade()
	{
		GameObject granade = GameObject.Instantiate(granadePrefab);
		granade.transform.position = transform.position;
	}

	private void HideGunModel(bool bl)
	{
		gunParent.SetActive(!bl);
	}

	private void Shoot(WeaponType weaponType)
	{
		switch (weaponType)
		{
			case WeaponType.Rifle: RifleShoot(); break;
			case WeaponType.Shotgun: ShotgunShoot(); break;
			case WeaponType.Sniper: SbiperShoot(); break;
		}
	}

	private void RifleShoot()
	{
		GameObject bullet = bulletPool.Dequeue();
		bullet.transform.position = gunPoints[m_index].position;
		bullet.transform.rotation = gunPoints[m_index].rotation;
		bullet.SetActive(true);
	}
	private void ShotgunShoot()
	{
		for (int i = 0; i < 20; i++)
		{
			float angle = Random.Range(0, PI_2);
			float radius = Random.Range(0, shotgunRange);
			Vector3 dir;
			dir.x = radius * Mathf.Cos(angle);
			dir.y = radius * Mathf.Sin(angle);
			dir.z = Mathf.Sqrt(1 - radius * radius);
			dir = gunPoints[m_index].TransformDirection(dir);

			GameObject bullet = bulletPool.Dequeue();
			bullet.transform.position = gunPoints[m_index].position;
			bullet.transform.rotation = Quaternion.LookRotation(dir);
			bullet.SetActive(true);
		}
	}
	private void SbiperShoot()
	{
		GameObject bullet = bulletPool.Dequeue();
		bullet.transform.position = gunPoints[m_index].position;
		bullet.transform.rotation = gunPoints[m_index].rotation;
		bullet.SetActive(true);
	}

	private void EnableWeapon(WeaponType weapon)
	{
		foreach (var weaponObject in weaponObjects)
			weaponObject.SetActive(false);
		m_index = (int)weapon;
		weaponObjects[m_index].SetActive(true);
	}
}
