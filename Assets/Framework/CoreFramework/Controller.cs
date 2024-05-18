using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public interface IController
	{ }


	//Controller��չ�����ڼ򻯲���
	public static class ControllerExtension
	{
		#region SendCommand
		public static void SendCommand<T>(this IController self) where T : ICustomCommand, new()
		{
			CustomCommandSystem.SendCommand<T>();
		}

		public static TResult SendCommand<T, TResult>(this IController self) where T : ICustomCommand<TResult>, new()
		{
			return CustomCommandSystem.SendCommand<T, TResult>();
		}

		public static void SendCommand<T>(this IController self, T instance) where T : ICustomCommand
		{
			CustomCommandSystem.SendCommand<T>(instance);
		}

		public static TResult SendCommand<T, TResult>(this IController self, T instance) where T : ICustomCommand<TResult>
		{
			return CustomCommandSystem.SendCommand<T, TResult>(instance);
		}
		#endregion

		#region Event
		public static T RegisterEvent<T>(this IController slef) where T : class, ICustomEvent, new()
		{
			return CustomEventSystem.Instance.RegisterEvent<T>();
		}

		public static void UnRegisterEvent<T>(this IController slef) where T : class, ICustomEvent, new()
		{
			CustomEventSystem.Instance.UnRegisterEvent<T>();
		}

		public static bool EventExist<T>(this IController self) where T : class, ICustomEvent, new()
		{
			return CustomEventSystem.Instance.EventExist<T>();
		}

		public static T GetEvent<T>(this IController self) where T : class, ICustomEvent, new()
		{
			return CustomEventSystem.Instance.GetEvent<T>();
		}
		#endregion

		#region Model
		//Controller����ʹ�ã��޸����ݣ���Ӧ�ö�����ģ�����ע���Լ�ע������
		public static T GetOrAddModel<T>(this IController self) where T : class, ICustomModel, new()
		{
			return CustomModelSystem.Instance.GetOrAddModel<T>();
		}
		#endregion

		#region ObjectPool
		public static void PutNewPool(this IController self, string name, GameObject[] instances)
		{
			ObjectPool.Instance.PutNewPool(name, instances);
		}

		public static Queue<GameObject> GetQueue(this IController self, string name)
		{
			return ObjectPool.Instance.GetQueue(name);
		}
		#endregion
	}
}