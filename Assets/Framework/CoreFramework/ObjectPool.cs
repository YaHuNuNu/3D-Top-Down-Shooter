using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	private static ObjectPool m_instance;
	public static ObjectPool Instance
	{
		get
		{
			if(m_instance == null)
				m_instance = new ObjectPool();
			return m_instance;
		}
	}

	private Dictionary<string, Queue<GameObject>> m_dictionary = new Dictionary<string, Queue<GameObject>>();

	public void PutNewPool(string name, GameObject[] instances)
	{
		Queue<GameObject> queue;
		if (PoolExist(name))
			queue = m_dictionary[name];
		else
			queue = new Queue<GameObject>();

		foreach (GameObject obj in instances)
		{
			queue.Enqueue(obj);
		}

		m_dictionary[name] = queue;
	}

	public bool PoolExist(string name)
	{
		return m_dictionary.ContainsKey(name);
	}

	public Queue<GameObject> GetQueue(string name)
	{
		m_dictionary.TryGetValue(name, out Queue<GameObject> queue);
		return queue;
	}
}