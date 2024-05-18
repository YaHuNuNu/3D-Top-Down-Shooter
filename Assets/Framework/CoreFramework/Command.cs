namespace Framework
{
	public interface ICustomCommand
	{
		void Execute();
	}

	public interface ICustomCommand<TResult>
	{
		TResult Execute();
	}

	public static class CustomCommandSystem
	{
		public static void SendCommand<T>() where T : ICustomCommand, new()
		{
			new T().Execute();
		}

		public static TResult SendCommand<T, TResult>() where T : ICustomCommand<TResult>, new()
		{
			return new T().Execute();
		}

		public static void SendCommand<T>(T instance) where T : ICustomCommand
		{
			instance.Execute();
		}

		public static TResult SendCommand<T, TResult>(T instance) where T : ICustomCommand<TResult>
		{
			return instance.Execute();
		}
	}
}

