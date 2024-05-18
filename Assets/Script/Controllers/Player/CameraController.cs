using Cinemachine;
using Framework;
using UnityEngine;

public class CameraController : MonoBehaviour, IController
{
	public CinemachineVirtualCamera virtualCamera;
	private CinemachineFramingTransposer m_Transposer;
	private PlayerModel m_PlayerModel;
	private void Awake()
	{
		m_PlayerModel = this.GetOrAddModel<PlayerModel>();
		m_Transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		m_PlayerModel.weaponType.RegisterActionWithExcute(ChangeCameraDistance);

	}

	private void ChangeCameraDistance(WeaponType type)
	{
		float d = 0;
		switch (type)
		{
			case WeaponType.Rifle: d = 8; break;
			case WeaponType.Shotgun: d = 6; break;
			case WeaponType.Sniper: d = 12; break;
		}

		m_Transposer.m_CameraDistance = d;
	}
}
