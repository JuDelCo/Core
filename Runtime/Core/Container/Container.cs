// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Extensions;
using Ju.Util;
using Identifier = System.String;

namespace Ju.Services
{
	[Serializable]
	public class Container : IDisposable
	{
		[Serializable] internal class DictIdService : SerializableDictionary<Identifier, IService> { }
		[Serializable] internal class DictIdFactory : SerializableDictionary<Identifier, Func<object>> { }
		[Serializable] internal class ServicesLoaded : SerializableDictionaryStructClass<SerializableType, DictIdService> { }
		[Serializable] internal class FactoriesLoaded : SerializableDictionaryStructClass<SerializableType, DictIdFactory> { }

#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeReference]
#endif
		internal ServicesLoaded services = null;
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeReference]
#endif
		internal FactoriesLoaded classFactories = null;

		public Container()
		{
			services = new ServicesLoaded();
			classFactories = new FactoriesLoaded();
		}

		public void Dispose()
		{
			services.ForEachReverse(typeService =>
			{
				typeService.Value.ForEachReverse(idService =>
				{
					Unload(typeService.Key, idService.Key);
				});
			});

			services.Clear();
			classFactories.Clear();
		}

		public T Get<T>(Identifier id)
		{
			var type = typeof(T);

			object instance = null;

			if (classFactories.ContainsKey(type) && classFactories[type].ContainsKey(id))
			{
				instance = classFactories[type][id]();
			}
			else if (services.ContainsKey(type) && services[type].ContainsKey(id))
			{
				instance = services[type][id];
			}

			if (instance == null)
			{
				throw new NullReferenceException($"No class of type '{type.GetFriendlyName()}' with id '{id}' found");
			}

			return (T)instance;
		}

		public void Register<T>(Identifier id, IService service)
		{
			var type = typeof(T);

			if (CheckDuplicatedClass(type, id))
			{
				throw new Exception($"Tried to re-register a class of type '{type.GetFriendlyName()}' with ID '{id}'");
			}

			services.GetOrInsertNew(type).Add(id, service);
		}

		public void RegisterFactory<T>(Identifier id, Func<T> classConstructor)
		{
			var type = typeof(T);

			if (CheckDuplicatedClass(type, id))
			{
				throw new Exception($"Tried to re-register a factory of type '{type.GetFriendlyName()}' with ID '{id}'");
			}

			classFactories.GetOrInsertNew(type).Add(id, () =>
			{
				return classConstructor();
			});
		}

		public void Unload<T>(Identifier id)
		{
			Unload(typeof(T), id);
		}

		private bool CheckDuplicatedClass(Type type, Identifier id)
		{
			bool duplicated = false;

			if (services.ContainsKey(type))
			{
				duplicated |= services[type].ContainsKey(id);
			}

			if (classFactories.ContainsKey(type))
			{
				duplicated |= classFactories[type].ContainsKey(id);
			}

			return duplicated;
		}

		private void Unload(Type type, Identifier id)
		{
			if (services.ContainsKey(type))
			{
				if (services[type].ContainsKey(id))
				{
					var service = services[type][id];

					if (service is IServiceUnload unload)
					{
						unload.Unload();
					}

					services[type].Remove(id);

					if (services[type].Count == 0)
					{
						services.Remove(type);
					}
				}
			}
		}
	}
}
