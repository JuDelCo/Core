using System;
using System.Runtime.CompilerServices;
using Ju.Services;

namespace Ju.Log
{
	public static class Log
	{
		public static void Assert(bool condition, object msg, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = -1)
		{
			if (!condition)
			{
				EventBus.Fire(new LogEvent(LogLevel.Error, msg.ToString(), GetContext(file, method, line)));
			}
		}

		public static void Debug(object msg, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = -1)
		{
			EventBus.Fire(new LogEvent(LogLevel.Debug, msg.ToString(), GetContext(file, method, line)));
		}

		public static void Info(object msg, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = -1)
		{
			EventBus.Fire(new LogEvent(LogLevel.Info, msg.ToString(), GetContext(file, method, line)));
		}

		public static void Notice(object msg, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = -1)
		{
			EventBus.Fire(new LogEvent(LogLevel.Notice, msg.ToString(), GetContext(file, method, line)));
		}

		public static void Warning(object msg, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = -1)
		{
			EventBus.Fire(new LogEvent(LogLevel.Warning, msg.ToString(), GetContext(file, method, line)));
		}

		public static void Error(object msg, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = -1)
		{
			EventBus.Fire(new LogEvent(LogLevel.Error, msg.ToString(), GetContext(file, method, line)));
		}

		public static void Exception(object msg, Exception e, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = -1)
		{
			EventBus.Fire(new LogEvent(LogLevel.Error, msg.ToString(), GetContext(file, method, line), e));
		}

		public static void ToggleContext(bool enable, bool folderTrim = false)
		{
			Log.contextEnabled = enable;
			Log.folderTrim = folderTrim;
		}

		private static string GetContext(string file, string method, int line)
		{
			return contextEnabled ? $"\n> {TruncateFolders(file)}:{line} > {method}()" : null;
		}

		private static string TruncateFolders(string path)
		{
#if UNITY_2019_3_OR_NEWER
			return folderTrim ? System.IO.Path.GetFileName(path) : (path.Contains("Assets") ? path.Remove(0, path.LastIndexOf("Assets")) : path);
#else
			return folderTrim ? System.IO.Path.GetFileName(path) : path;
#endif
		}

		internal static void Dispose()
		{
			eventBus = null;
		}

		private static IEventBusService eventBus = null;
		private static bool contextEnabled = true;
		private static bool folderTrim = false;

		private static IEventBusService EventBus
		{
			get
			{
				if (eventBus is null)
				{
					eventBus = ServiceContainer.Get<IEventBusService>();
				}

				return eventBus;
			}
		}
	}
}