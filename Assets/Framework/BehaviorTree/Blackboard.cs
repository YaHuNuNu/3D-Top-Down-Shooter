using System;
using System.Collections.Generic;

namespace BT
{
	public class SimpleData<T> where T : struct
	{
		public T value;
		public SimpleData() { }
	}

	#region Blackboard
	public class Blackboard
	{
		private Dictionary<string, Object> m_Data = new Dictionary<string, Object>();
		public void Add<T>(string key, T instance) where T : class
		{
			m_Data.Add(key, instance);
		}
		public object Get(string key)
		{
			return m_Data[key];
		}

		public void AddOrUpdate<T>(string key, T instance) where T : class
		{
			m_Data[key] = instance;
		}


		public bool TryToGet(string key, out object instance)
		{
			return m_Data.TryGetValue(key, out instance);
		}

		public bool ConstainsKey(string key)
		{
			return m_Data.ContainsKey(key);
		}

		public object GetOrAdd<T>(string key) where T : class, new()
		{
			if(ConstainsKey(key))
				return Get(key);
			else
			{
				T instance = new T();
				Add(key, instance);
				return instance;
			}	
		}

		public void Remove(string key)
		{
			m_Data.Remove(key);
		}
	}
	#endregion

	#region ExternalBlackboard
	public static class ExternalBlackboard 
	{
		private static Blackboard m_blackboard;
		public static Blackboard Blackboard 
		{
			get
			{
				if(m_blackboard == null)
				{
					m_blackboard = new Blackboard();
				}
				return m_blackboard;
			}
		}
	}
	#endregion
}