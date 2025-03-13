// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Extensions;
using Identifier = System.String;

namespace Ju.Services
{
	using Ju.Services.Internal;
	using Ju.Util;

	[Serializable]
	public class ServiceContainer
	{
		public static event Action OnDisposed = delegate { };

		private static readonly Identifier DEFAULT_ID = "base";
		internal static ServiceContainer instance;

		[Serializable] internal class DictIdObj : SerializableDictionary<Identifier, object> { }
		[Serializable] internal class DictIdType : SerializableDictionaryClassStruct<Identifier, SerializableType> { }
		[Serializable] internal class ServicesRegistered : SerializableDictionaryStructClass<SerializableType, DictIdObj> { }
		[Serializable] internal class ServicesLoaded : SerializableDictionaryStructClass<SerializableType, DictIdType> { }

#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeReference]
#endif
		internal ServicesRegistered servicesRegistered;
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeReference]
#endif
		internal ServicesLoaded servicesLoaded;
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeReference]
#endif
		internal Container container;

		public ServiceContainer()
		{
			servicesRegistered = new ServicesRegistered();
			servicesLoaded = new ServicesLoaded();
			container = new Container();
		}

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

			// Check for duplicates
			{
				bool duplicated = false;

				if (services.servicesRegistered.ContainsKey(baseType))
				{
					duplicated |= services.servicesRegistered[baseType].ContainsKey(id);
				}

				if (services.servicesLoaded.ContainsKey(baseType))
				{
					duplicated |= services.servicesLoaded[baseType].ContainsKey(id);
				}

				if (duplicated)
				{
					throw new Exception($"Tried to re-register a service of type '{serviceType.GetFriendlyName()}' with ID '{id}'");
				}
			}

			services.servicesRegistered.GetOrInsertNew(baseType).Add(id, new T2());
		}

		public static void UnloadService<T>() where T : IService, new()
		{
			UnloadService<T>(DEFAULT_ID);
		}

		public static void UnloadService<T>(string id) where T : IService, new()
		{
			var services = InternalInstance();
			var baseType = typeof(T);

			if (baseType == typeof(IEventBusService))
			{
				throw new Exception("IEventBusService is a special service and can't be unloaded");
			}

			if (services.servicesRegistered.ContainsKey(baseType))
			{
				services.servicesRegistered[baseType].Remove(id);

				if (services.servicesRegistered[baseType].Count == 0)
				{
					services.servicesRegistered.Remove(baseType);
				}
			}

			if (services.servicesLoaded.ContainsKey(baseType))
			{
				services.servicesLoaded[baseType].Remove(id);

				if (services.servicesLoaded[baseType].Count == 0)
				{
					services.servicesLoaded.Remove(baseType);
				}
			}

			services.container.Unload<T>(id);
		}

		public static T Get<T>()
		{
			return Get<T>(DEFAULT_ID);
		}

		public static T Get<T>(string id)
		{
			var services = InternalInstance();
			var baseType = typeof(T);
			IService service;

			if (services.servicesLoaded.ContainsKey(baseType) && services.servicesLoaded[baseType].ContainsKey(id))
			{
				service = (IService)services.container.Get<T>(id);
			}
			else if (services.servicesRegistered.ContainsKey(baseType) && services.servicesRegistered[baseType].ContainsKey(id))
			{
				service = (IService)services.servicesRegistered[baseType][id];

				services.servicesRegistered[baseType].Remove(id);
				services.servicesLoaded.GetOrInsertNew(baseType).Add(id, service.GetType());

				services.container.Register<T>(id, service);

				if (service is IServiceLoad serviceLoad)
				{
					serviceLoad.Load();
				}
			}
			else
			{
				throw new Exception($"No service of type '{baseType.GetFriendlyName()}' with ID '{id}' has been registered");
			}

			return (T)service;
		}

		private static ServiceContainer InternalInstance()
		{
			if (instance == null)
			{
				instance = (new ServiceContainer());
				RegisterService<IEventBusService, EventBusService>();
			}

			return instance;
		}

		public static void Dispose()
		{
			if (instance != null)
			{
				ServiceCache.EventBus.StopCurrentEventPropagation();
				instance.container.Dispose();
				ServiceCache.Dispose();

				instance.servicesRegistered.Clear();
				instance.servicesLoaded.Clear();
				instance = null;

				OnDisposed();
				OnDisposed = delegate { };
			}
		}
	}
}
