using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DATAClass
{
	public Inventory inventory;
	public PlayerModel playerModel;
	public SwordEnemyModelBase enemyBase;
}


public class GameLoad_Save : MonoBehaviour
{
	private string dataPath;

	private void Awake()
	{
		dataPath = Path.Combine(Application.persistentDataPath, "GameData.json");
		CustomModelSystem temp = CustomModelSystem.Instance;
	}

	public void LoadGame()
	{
		DATAClass data = LoadData();
		if (data != null)
		{
			Dictionary<Type, System.Object> dict = new Dictionary<Type, System.Object>();
			IOCContainer container = new IOCContainer();
			container.Register(data.inventory);
			container.Register(data.playerModel);
			container.Register(data.enemyBase);
			CustomModelSystem.Instance.SetContainer(container);
		}
	}

	private void OnApplicationQuit()
	{
		if (SceneManager.GetActiveScene().name != "LevelScene")
			return;
		var dict = CustomModelSystem.Instance.GetContainer().GetDictionary();
		DATAClass data = new DATAClass();
		data.inventory = dict[typeof(Inventory)] as Inventory;
		data.playerModel = dict[typeof(PlayerModel)] as PlayerModel;
		data.enemyBase = dict[typeof(SwordEnemyModelBase)] as SwordEnemyModelBase;
		data.playerModel.CurrentScene = SceneManager.GetActiveScene().name;
		Vector3 pos = EntityManager.Instance.playerCtl.transform.position;
		float[] vec = new float[3];
		vec[0] = pos.x;
		vec[1] = pos.y;
		vec[2] = pos.z;
		data.playerModel.playerPos.SetValueWithoutEvent(vec);
		SaveData(data);
	}



	private void SaveData(DATAClass data)
	{
		JsonSerializerSettings settings = new JsonSerializerSettings();
		settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
		using (StreamWriter sw = new StreamWriter(dataPath))
		{
			//保存数据
			sw.WriteLine(jsonData);
			//关闭文档
			sw.Close();
			sw.Dispose();
		}
	}

	private DATAClass LoadData()
	{
		if (File.Exists(dataPath))
		{
			string jsonData;
			using (StreamReader sr = File.OpenText(dataPath))
			{
				//数据保存
				jsonData = sr.ReadToEnd();
				sr.Close();
			}
			return JsonConvert.DeserializeObject<DATAClass>(jsonData);
		}
		else
		{
			//Debug.LogError("Save file not found at " + dataPath);
			return null;
		}
	}
}
