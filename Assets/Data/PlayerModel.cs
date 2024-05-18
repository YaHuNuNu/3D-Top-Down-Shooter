using Framework;
using System;
using UnityEngine;

[System.Serializable]
public enum WeaponType
{
	Rifle = 0, Shotgun, Sniper
}

[System.Serializable]
public class PlayerModel : ICustomModel
{
	public BindableProperty<float> health = new BindableProperty<float>(1);

	public BindableProperty<float>[] ATK = new BindableProperty<float>[Enum.GetNames(typeof(WeaponType)).Length];

	public BindableProperty<int>[] currentBulletNum = new BindableProperty<int>[Enum.GetNames(typeof(WeaponType)).Length];

	public int[] defaultBulletNum = new int[Enum.GetNames(typeof(WeaponType)).Length];

	public BindableProperty<WeaponType> weaponType = new BindableProperty<WeaponType>(WeaponType.Rifle);

	public BindableProperty<float[]> playerPos = new BindableProperty<float[]>(new float[3]);
	public string CurrentScene = "StartScene";

	public void Init()
	{
		defaultBulletNum[0] = 30;
		defaultBulletNum[1] = 15;
		defaultBulletNum[2] = 15;
		for (int i = 0; i < currentBulletNum.Length; i++)
		{
			currentBulletNum[i] = new BindableProperty<int>(defaultBulletNum[i]);
			ATK[i] = new BindableProperty<float>(0);
		}

		ATK[0].SetValueWithoutEvent(6);
		ATK[1].SetValueWithoutEvent(3f);
		ATK[2].SetValueWithoutEvent(40);

	}
}