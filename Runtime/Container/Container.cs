using System;
using System.Collections.Generic;
using Identifier = System.String;

namespace Ju
{
public class Container : IDisposable
{
	private Dictionary<Type, Dictionary<Identifier, IService>> services = null;
	private Dictionary<Type, Dictionary<Identifier, Func<object>>> classFactories = null;
	private Dictionary<Type, Dictionary<Identifier, object>> instanceContainer = null;

	public Container()
	{
		services = new Dictionary<Type, Dictionary<Identifier, IService>>();
		classFactories = new Dictionary<Type, Dictionary<Identifier, Func<object>>>();
		instanceContainer = new Dictionary<Type, Dictionary<Identifier, object>>();
	}

	public void Dispose()
	{
		services.Clear();
		classFactories.Clear();
		instanceContainer.Clear();
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

	private object Get(Type type, Identifier id)
	{
		object instance = null;

		if (classFactories.ContainsKey(type) && classFactories[type].ContainsKey(id))
		{
			try
			{
				instance = classFactories[type][id]();
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("Error creating an instance of type '{0}' with id '{1}' using a factory method", type.ToString(), id), e);
			}
		}
		else
		{
			if (services.ContainsKey(type) && services[type].ContainsKey(id))
			{
				if (instanceContainer.ContainsKey(type) && instanceContainer[type].ContainsKey(id))
				{
					instance = instanceContainer[type][id];
				}
				else
				{
					if (!instanceContainer.ContainsKey(type))
					{
						instanceContainer.Add(type, new Dictionary<Identifier, object>());
					}

					try
					{
						instance = services[type][id];
					}
					catch (Exception e)
					{
						throw new Exception(string.Format("Error creating an instance of type '{0}' with id '{1}'", type.ToString(), id), e);
					}

					instanceContainer[type].Add(id, instance);
				}
			}
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
