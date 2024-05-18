using Framework;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour, IController
{
	public Image HP_Image;
	public GameObject[] Weapon_Icon;
	public TextMeshProUGUI[] Weapon_Num;

	private PlayerModel playerModel;
	private Inventory inventory;

	private void Start()
	{
		playerModel = this.GetOrAddModel<PlayerModel>();
		inventory = this.GetOrAddModel<Inventory>();

		playerModel.health.RegisterActionWithExcute(SetPlayerHP);
		playerModel.weaponType.RegisterActionWithExcute(ChangeWeaponIcon);
		playerModel.currentBulletNum[(int)WeaponType.Rifle].RegisterActionWithExcute(SetRifleNum);
		playerModel.currentBulletNum[(int)WeaponType.Shotgun].RegisterActionWithExcute(SetShotgunNum);
		playerModel.currentBulletNum[(int)WeaponType.Sniper].RegisterActionWithExcute(SetSniperNum);

		inventory.grenadeSum.RegisterActionWithExcute(SetGrenadeSum);
		inventory.bulletSum[(int)WeaponType.Rifle].RegisterActionWithExcute(SetRifleSum);
		inventory.bulletSum[(int)WeaponType.Shotgun].RegisterActionWithExcute(SetShotgunSum);
		inventory.bulletSum[(int)WeaponType.Sniper].RegisterActionWithExcute(SetSniperSum);

	}

	private void SetRifleNum(int obj)
	{
		Weapon_Num[0].text = obj.ToString() + "\n" + inventory.bulletSum[0].Value.ToString();
	}
	private void SetShotgunNum(int obj)
	{
		Weapon_Num[1].text = obj.ToString() + "\n" + inventory.bulletSum[1].Value.ToString();
	}
	private void SetSniperNum(int obj)
	{
		Weapon_Num[2].text = obj.ToString() + "\n" + inventory.bulletSum[2].Value.ToString();
	}

	private void SetGrenadeSum(int obj)
	{
		Weapon_Num[3].text = obj.ToString();
	}

	private void SetRifleSum(int obj)
	{
		Weapon_Num[0].text = playerModel.currentBulletNum[0].Value.ToString() + "\n" +  obj.ToString();
	}
	private void SetShotgunSum(int obj)
	{
		Weapon_Num[1].text = playerModel.currentBulletNum[1].Value.ToString() + "\n" + obj.ToString();
	}
	private void SetSniperSum(int obj)
	{
		Weapon_Num[2].text = playerModel.currentBulletNum[2].Value.ToString() + "\n" + obj.ToString();
	}



	private void ChangeWeaponIcon(WeaponType type)
	{
		for (int i = 0; i < Weapon_Icon.Length; i++)
			Weapon_Icon[i].SetActive(false);

		Weapon_Icon[(int)type].SetActive(true);
	}

	private void SetPlayerHP(float hp)
	{
		HP_Image.fillAmount = hp;
	}
}
