using System;
using System.Collections.Generic;
using Identifier = System.String;

namespace Ju.Services
{
	public class Container : IDisposable
	{
		private Dictionary<Type, Dictionary<Identifier, IService>> services = null;
		private Dictionary<Type, Dictionary<Identifier, Func<object>>> classFactories = null;

		public Container()
		{
			services = new Dictionary<Type, Dictionary<Identifier, IService>>();
			classFactories = new Dictionary<Type, Dictionary<Identifier, Func<object>>>();
		}

		public void Dispose()
		{
			services.Clear();
			classFactories.Clear();
		}

		public T Get<T>(Identifier id)
		{
			return (T)(Get(typeof(T), id));
		}

		public void Register<T>(Identifier id, IService service)
		{
			Register(typeof(T), id, service);
		}

		public void RegisterFactory<T>(Identifier id, Func<T> classConstructor)
		{
			RegisterFactory(typeof(T), id, () =>
			{
				return classConstructor();
			});
		}

		public void Unload<T>(Identifier id)
		{
			var type = typeof(T);

			if (services.ContainsKey(type))
			{
				if (services[type].ContainsKey(id))
				{
					var service = services[type][id];

					if (service is IServiceUnload)
					{
						((IServiceUnload)service).Unload();
					}

					services[type].Remove(id);

					if (services[type].Count == 0)
					{
						services.Remove(type);
					}
				}
			}
		}

		private object Get(Type type, Identifier id)
		{
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
				throw new NullReferenceException(string.Format("No class of type '{0}' with id '{1}' found", type.ToString(), id));
			}

			return instance;
		}

		private void Register(Type type, Identifier id, IService service)
		{
			if (CheckDuplicatedClass(type, id))
			{
				throw new Exception(string.Format("Tried to re-register a class of type '{0}' with ID '{1}'", type.ToString(), id));
			}

			if (!services.ContainsKey(type))
			{
				services.Add(type, new Dictionary<Identifier, IService>());
			}

			services[type].Add(id, service);
		}

		private void RegisterFactory(Type type, Identifier id, Func<object> classConstructor)
		{
			if (CheckDuplicatedClass(type, id))
			{
				throw new Exception(string.Format("Tried to re-register a factory of type '{0}' with ID '{1}'", type.ToString(), id));
			}

			if (!classFactories.ContainsKey(type))
			{
				classFactories.Add(type, new Dictionary<Identifier, Func<object>>());
			}

			classFactories[type].Add(id, classConstructor);
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
	}
}
