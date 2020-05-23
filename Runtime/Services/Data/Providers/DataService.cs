using System;
using System.Collections;
using System.Collections.Generic;
using Identifier = System.String;

namespace Ju
{
	public class DataService : IDataService
	{
		public event DataServiceListEvent OnListAdd = delegate { };
		public event DataServiceListEvent OnListRemove = delegate { };

		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private Dictionary<Type, Dictionary<Identifier, object>> sharedItems;
		private Dictionary<Type, Dictionary<Identifier, object>> listItems;

		public void Setup()
		{
			sharedItems = new Dictionary<Type, Dictionary<Identifier, object>>();
			listItems = new Dictionary<Type, Dictionary<Identifier, object>>();
		}

		public void Start()
		{
		}

		public void Set<T>(T obj, string id)
		{
			var type = typeof(T);

			if (!sharedItems.ContainsKey(type))
			{
				sharedItems.Add(type, new Dictionary<Identifier, object>());
			}

			if (sharedItems[type].ContainsKey(id))
			{
				OnLogWarning("A shared object of type {0} was already stored");
			}

			sharedItems[type][id] = obj;
		}

		public T Get<T>(string id) where T : class
		{
			var type = typeof(T);
			T instance = null;

			if (sharedItems.ContainsKey(type))
			{
				if (sharedItems[type].ContainsKey(id))
				{
					instance = (T)sharedItems[type][id];
				}
			}

			return instance;
		}

		public void Unset<T>(string id)
		{
			var type = typeof(T);

			if (sharedItems.ContainsKey(type))
			{
				if (sharedItems[type].ContainsKey(id))
				{
					sharedItems[type].Remove(id);

					if (sharedItems[type].Count == 0)
					{
						listItems.Remove(type);
					}
				}
			}
		}

		public List<Type> GetTypes()
		{
			var types = new List<Type>();

			foreach (var key in sharedItems.Keys)
			{
				types.Add(key);
			}

			return types;
		}

		public void ListAdd<T>(T obj, string id)
		{
			var type = obj.GetType();

			IList list = null;

			if (!listItems.ContainsKey(type))
			{
				listItems.Add(type, new Dictionary<Identifier, object>());
			}

			if (!listItems[type].ContainsKey(id))
			{
				list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
				listItems[type][id] = list;
			}
			else
			{
				list = (IList)listItems[type][id];
			}

			list.Add(obj);

			OnListAdd(type, obj);
		}

		public List<T> ListGet<T>(string id)
		{
			var type = typeof(T);
			List<T> list = null;

			if (listItems.ContainsKey(type))
			{
				if (listItems[type].ContainsKey(id))
				{
					list = (List<T>)listItems[type][id];
				}
			}

			return list;
		}

		public void ListRemove<T>(T obj, string id)
		{
			var type = obj.GetType();

			if (listItems.ContainsKey(type))
			{
				if (listItems[type].ContainsKey(id))
				{
					var list = (IList)listItems[type][id];

					list.Remove(obj);

					if (list.Count == 0)
					{
						listItems[type].Remove(id);

						if (listItems[type].Count == 0)
						{
							listItems.Remove(type);
						}
					}

					OnListRemove(type, obj);
				}
			}
		}

		public List<Type> ListGetTypes()
		{
			var types = new List<Type>();

			foreach (var key in listItems.Keys)
			{
				types.Add(key);
			}

			return types;
		}
	}
}
