using System;

namespace Framework
{
	#region UnRegisterInfo
	public interface IUnRegisterInfo
	{
		void UnRegister();
	}

	public struct UnRegisterInfo : IUnRegisterInfo
	{
		private Action m_UnRegisterFunc;

		public UnRegisterInfo(Action UnRegisterFunc)
		{
			m_UnRegisterFunc = UnRegisterFunc;
		}
		public void UnRegister()
		{
			m_UnRegisterFunc?.Invoke();
			m_UnRegisterFunc = null;
		}
	}
	#endregion

	#region CustomEvent
	public interface ICustomEvent
	{

	}

	public abstract class CustomEvent : ICustomEvent
	{
		private Action m_eventAction;
		public IUnRegisterInfo Register(Action eventAction)
		{
			m_eventAction += eventAction;
			return new UnRegisterInfo(() => UnRegister(eventAction));
		}

		public void UnRegister(Action eventAction) => m_eventAction -= eventAction;

		public void Trigger() => m_eventAction?.Invoke();
	}

	public abstract class CustomEvent<T> : ICustomEvent
	{
		private Action<T> m_eventAction;
		public IUnRegisterInfo Register(Action<T> eventAction)
		{
			m_eventAction += eventAction;
			return new UnRegisterInfo(() => UnRegister(eventAction));
		}

		public void UnRegister(Action<T> eventAction) => m_eventAction -= eventAction;

		public void Trigger(T t) => m_eventAction?.Invoke(t);
	}

	public abstract class CustomEvent<T1, T2> : ICustomEvent
	{
		private Action<T1, T2> m_eventAction;
		public IUnRegisterInfo Register(Action<T1, T2> eventAction)
		{
			m_eventAction += eventAction;
			return new UnRegisterInfo(() => UnRegister(eventAction));
		}

		public void UnRegister(Action<T1, T2> eventAction) => m_eventAction -= eventAction;

		public void Trigger(T1 t1, T2 t2) => m_eventAction?.Invoke(t1, t2);
	}

	public abstract class CustomEvent<T1, T2, T3> : ICustomEvent
	{
		private Action<T1, T2, T3> m_eventAction;
		public IUnRegisterInfo Register(Action<T1, T2, T3> eventAction)
		{
			m_eventAction += eventAction;
			return new UnRegisterInfo(() => UnRegister(eventAction));
		}

		public void UnRegister(Action<T1, T2, T3> eventAction) => m_eventAction -= eventAction;

		public void Trigger(T1 t1, T2 t2, T3 t3) => m_eventAction?.Invoke(t1, t2, t3);
	}
	#endregion

	#region CustomEventSystem
	public class CustomEventSystem
	{
		private IOCContainer m_container = new IOCContainer();

		private CustomEventSystem() { }
		private static CustomEventSystem m_instance;

		public static CustomEventSystem Instance
		{
			get
			{
				if (m_instance == null)
					m_instance = new CustomEventSystem();
				return m_instance;
			}
		}


		public T RegisterEvent<T>() where T : class, ICustomEvent, new()
		{
			if (EventExist<T>())
				return GetEvent<T>();
			else
			{
				T t = new T();
				m_container.Register<T>(t);
				return t;
			}
		}

		public void UnRegisterEvent<T>() where T : class, ICustomEvent, new()
		{
			m_container.UnRegister<T>();
		}

		public bool EventExist<T>() where T : class, ICustomEvent, new()
		{
			return m_container.InstanceExsit<T>();
		}

		//注册Action和触发时调用这个
		public T GetEvent<T>() where T : class, ICustomEvent, new()
		{
			if (!EventExist<T>())
				RegisterEvent<T>();

			return m_container.Get<T>();
		}
	}
	#endregion
}
