using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Framework
{
	#region BindableProperty
	[System.Serializable]
	public class BindableProperty<T>
	{
		[SerializeField]
		private T m_value;
		private class BindablePropertyEvent : CustomEvent<T> { }
		public BindableProperty(T value) => m_value = value;
		public static Func<T, T, bool> Comparer { get; private set; } = (x, y) => x.Equals(y);

		private BindablePropertyEvent m_valueChangeEvent = new BindablePropertyEvent();
		public T Value
		{
			get => m_value;
			set
			{
				if (value == null && m_value == null) return;
				if (value != null && Comparer(value, m_value)) return;
				m_value = value;
				m_valueChangeEvent.Trigger(value);
			}
		}

		public void SetValueWithoutEvent(T newValue) => m_value = newValue;
		public IUnRegisterInfo RegisterAction(Action<T> action)
		{
			return m_valueChangeEvent.Register(action);
		}

		public IUnRegisterInfo RegisterActionWithExcute(Action<T> action)
		{
			action.Invoke(m_value);
			return m_valueChangeEvent.Register(action);
		}

		public void NotSetWithExcute() => m_valueChangeEvent.Trigger(m_value);
	}
	#endregion

	#region CustomModelSystem
	public interface ICustomModel
	{
		void Init();
	}

	public class CustomModelSystem
	{
		private IOCContainer m_container = new IOCContainer();

		private CustomModelSystem() { }

		private static CustomModelSystem m_instance;
		public static CustomModelSystem Instance
		{
			get
			{
				if(m_instance == null)
					m_instance = new CustomModelSystem();
				return m_instance;
			}
		}

		//用于JSON序列化
		public IOCContainer GetContainer() => m_container;
		public void SetContainer(IOCContainer container)
		{
			if(container !=  null)
				m_container = container;
		}

		public T RegisterModel<T>() where T : class, ICustomModel, new()
		{
			if (ModelExist<T>())
				return GetOrAddModel<T>();
			else
			{
				T t = new T();
				t.Init();
				m_container.Register<T>(t);
				return t;
			}
		}

		public void UnRegisterModel<T>() where T : class, ICustomModel, new()
		{
			m_container.UnRegister<T>();
		}

		public bool ModelExist<T>() where T : class, ICustomModel, new()
		{
			return m_container.InstanceExsit<T>();
		}

		public T GetOrAddModel<T>() where T : class, ICustomModel, new()
		{
			if (!ModelExist<T>())
				RegisterModel<T>();

			return m_container.Get<T>();
		}
	}
	#endregion
}
