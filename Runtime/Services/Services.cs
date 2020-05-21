using System;
using System.Collections.Generic;
using Identifier = System.String;

namespace Ju
{
	public delegate void CoreApplicationQuitEvent();

	public class Services
	{
		public static event CoreApplicationQuitEvent OnApplicationQuit;

		private static readonly Identifier DEFAULT_ID = "base";
		private static Services instance;

		private Dictionary<Type, Dictionary<Identifier, Type>> servicesRegistered = new Dictionary<Type, Dictionary<Identifier, Type>>();
		private Dictionary<Type, Dictionary<Identifier, Type>> servicesLoaded = new Dictionary<Type, Dictionary<Identifier, Type>>();
		private Container container = new Container();

		public static void RegisterFactory<T>(Func<T> classConstructor)
		{
			InternalInstance().container.RegisterFactory<T>(DEFAULT_ID, classConstructor);
		}

		public static void RegisterFactory<T>(string id, Func<T> classConstructor)
		{
			InternalInstance().container.RegisterFactory<T>(id, classConstructor);
		}

		public static void RegisterService<T>() where T : IService, new()
		{
			RegisterService<T, T>(DEFAULT_ID);
		}

		public static void RegisterService<T1, T2>() where T2 : T1, IService, new()
		{
			RegisterService<T1, T2>(DEFAULT_ID);
		}

		public static void RegisterService<T>(string id) where T : IService, new()
		{
			RegisterService<T, T>(id);
		}

		public static void RegisterService<T1, T2>(string id) where T2 : T1, IService, new()
		{
			RegisterLazyService<T1, T2>(id);
			Get<T1>(id);
		}

		public static void RegisterLazyService<T>() where T : IService, new()
		{
			RegisterLazyService<T, T>(DEFAULT_ID);
		}

		public static void RegisterLazyService<T1, T2>() where T2 : T1, IService, new()
		{
			RegisterLazyService<T1, T2>(DEFAULT_ID);
		}

		public static void RegisterLazyService<T>(string id) where T : IService, new()
		{
			RegisterLazyService<T, T>(id);
		}

		public static void RegisterLazyService<T1, T2>(string id) where T2 : T1, IService, new()
		{
			var services = InternalInstance();
			var baseType = typeof(T1);
			var serviceType = typeof(T2);

			if (CheckDuplicatedType(baseType, id))
			{
				throw new Exception(string.Format("Tried to re-register a service of type '{0}' with ID '{1}'", serviceType.ToString(), id));
			}

			if (!services.servicesRegistered.ContainsKey(baseType))
			{
				services.servicesRegistered.Add(baseType, new Dictionary<Identifier, Type>());
			}
			services.servicesRegistered[baseType].Add(id, serviceType);
		}

		public static T Get<T>()
		{
			return Get<T>(DEFAULT_ID);
		}

		public static T Get<T>(string id)
		{
			var services = InternalInstance();
			var baseType = typeof(T);
			IService service = null;

			if (services.servicesLoaded.ContainsKey(baseType) && services.servicesLoaded[baseType].ContainsKey(id))
			{
				service = (IService)services.container.Get<T>(id);
			}
			else if (services.servicesRegistered.ContainsKey(baseType) && services.servicesRegistered[baseType].ContainsKey(id))
			{
				var serviceType = services.servicesRegistered[baseType][id];
				services.servicesRegistered[baseType].Remove(id);

				if (!services.servicesLoaded.ContainsKey(baseType))
				{
					services.servicesLoaded.Add(baseType, new Dictionary<Identifier, Type>());
				}
				services.servicesLoaded[baseType].Add(id, serviceType);

				service = (IService)Activator.CreateInstance(serviceType);
				services.container.Register<T>(id, service);

				if (typeof(ILoggableService).IsAssignableFrom(service.GetType()))
				{
					Get<ILogService>().SubscribeLoggable((ILoggableService)service);
				}

				service.Setup();
				service.Start();
			}
			else
			{
				throw new Exception(string.Format("No service of type '{0}' with ID '{1}' has been registered", baseType.ToString(), id));
			}

			return (T)service;
		}

		private static Services InternalInstance()
		{
			if (instance == null)
			{
				instance = (new Services());
				RegisterService<ILogService, LogService>();
			}

			return instance;
		}

		private static bool CheckDuplicatedType(Type baseType, Identifier id)
		{
			var services = InternalInstance();

			bool duplicated = false;

			if (services.servicesRegistered.ContainsKey(baseType))
			{
				duplicated |= services.servicesRegistered[baseType].ContainsKey(id);
			}

			if (services.servicesLoaded.ContainsKey(baseType))
			{
				duplicated |= services.servicesLoaded[baseType].ContainsKey(id);
			}

			return duplicated;
		}

		public static void Dispose()
		{
			if (OnApplicationQuit != null)
			{
				OnApplicationQuit();
				OnApplicationQuit = null;
			}

			if (instance != null)
			{
				instance.servicesRegistered.Clear();
				instance.servicesLoaded.Clear();
				instance.container.Dispose();
				instance = null;
			}
		}
	}
}