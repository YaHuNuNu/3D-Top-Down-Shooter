using Framework;
using System;
using UnityEngine;

public class CameraAimController : MonoBehaviour
{
	private Vector2 cursorPos;
	private InputCtrls playerCtrls;
	private PlayerController playerCtl;
	[SerializeField]
	public float maxDistanceDelta;

	private float minDistance = 0.5f;
	private float maxDistance;
	private float constrainDistance = 0.6f;
	private float m_maxDistance;

	private void Awake()
	{
		playerCtrls = InputManager.instance.InputCtrls;
		playerCtl = EntityManager.Instance.playerCtl;
	}
	private void Start()
	{	
		CustomModelSystem.Instance.GetOrAddModel<PlayerModel>().weaponType.RegisterActionWithExcute(DistanceChange);
	}

	private void OnEnable()
	{
		transform.position = new Vector3(1.5f, playerCtl.transform.position.y + 1.35f, 1.5f);
	}

	private void DistanceChange(WeaponType type)
	{
		switch(type)
		{
			case WeaponType.Rifle:maxDistance = 3f; break;
			case WeaponType.Shotgun: maxDistance = 2f; break;
			case WeaponType.Sniper: maxDistance = 4f; break;
		}
	}

	private void Update()
	{
		if (playerCtl.moveSpeed.z < -1f)
			m_maxDistance = constrainDistance;
		else
			m_maxDistance = maxDistance;

		cursorPos = playerCtrls.Base.CursorPos.ReadValue<Vector2>();
		Ray ray = Camera.main.ScreenPointToRay(cursorPos);

		if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
		{
			Vector2 playerPos;
			playerPos.x = playerCtl.transform.position.x;
			playerPos.y = playerCtl.transform.position.z;
			Vector2 currentPos;
			currentPos.x = transform.position.x;
			currentPos.y = transform.position.z;
			Vector2 targetPos;
			targetPos.x = hitInfo.point.x;
			targetPos.y = hitInfo.point.z;

			//将CameraAim向鼠标指向的点匀速移动
			currentPos = Vector2.MoveTowards(currentPos, targetPos, maxDistanceDelta * Time.deltaTime);
			//保证cameraAim始终在范围内
			float distance = Vector2.Distance(playerPos, currentPos);
			Vector2 dir = (currentPos - playerPos).normalized;
			if (distance < minDistance)
				currentPos = playerPos + dir * minDistance;
			else if (distance > m_maxDistance)
				currentPos = playerPos + dir * m_maxDistance;
			transform.position = new Vector3(currentPos.x, playerCtl.transform.position.y + 1.35f, currentPos.y);
		}
	}
}
