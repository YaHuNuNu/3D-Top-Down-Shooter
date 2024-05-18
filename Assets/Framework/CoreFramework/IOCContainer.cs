using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
	public class IOCContainer
	{
		private Dictionary<Type, Object> m_dictionary = new Dictionary<Type, Object>();
		public Dictionary<Type, Object> GetDictionary() => m_dictionary;

		public void Register<T>(T instance) where T : class
		{
			m_dictionary[typeof(T)] = instance;
		}

		public void UnRegister<T>() where T : class
		{
			m_dictionary.Remove(typeof(T));
		}

		public T Get<T>() where T : class
		{
			if (m_dictionary.TryGetValue(typeof(T), out object instance))
				return instance as T;
			else
				return null;
		}

		public bool InstanceExsit<T>() where T : class
		{
			return m_dictionary.ContainsKey(typeof(T));
		}

		public IEnumerable<T> GetInstancesWithParentClass<T>() where T : class
		{
			return m_dictionary.Values.Where(instance => typeof(T).IsInstanceOfType(instance)).Cast<T>();
		}

		public void Clear()
		{
			m_dictionary.Clear();
		}
	}
}

