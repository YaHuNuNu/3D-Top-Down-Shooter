using Framework;
using System.Collections.Generic;

[System.Serializable]
public class SwordEnemyModel
{
	public int ID;
	public BindableProperty<float> health = new BindableProperty<float>(1);
	public BindableProperty<float> ATK = new BindableProperty<float>(10);
}

[System.Serializable]
public class SwordEnemyModelBase : ICustomModel
{
	public Dictionary<int, SwordEnemyModel> m_Instances = new Dictionary<int, SwordEnemyModel>();

	public SwordEnemyModel GetByID(int id)
	{
		if (m_Instances.ContainsKey(id))
		{
			return m_Instances[id];
		}
		else
		{
			var temp = new SwordEnemyModel();
			temp.ID = id;
			m_Instances.Add(id, temp);
			return temp;
		}

	}
	public void Init()
	{

	}
}