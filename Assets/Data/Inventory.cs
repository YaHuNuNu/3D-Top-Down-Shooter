using Framework;
using System;

[System.Serializable]
public class Inventory : ICustomModel
{
	public BindableProperty<int>[] bulletSum = new BindableProperty<int>[Enum.GetNames(typeof(WeaponType)).Length];
	public BindableProperty<int> grenadeSum = new BindableProperty<int>(5);

	public void Init()
	{
		for (int i = 0; i < bulletSum.Length; i++)
			bulletSum[i] = new BindableProperty<int>(0);
	}
}
