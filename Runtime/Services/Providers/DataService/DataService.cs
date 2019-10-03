using System;
using System.Collections.Generic;
using Identifier = System.String;

namespace Ju
{
	public class DataService : IDataService
	{
		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private Dictionary<Type, Dictionary<Identifier, object>> sharedItems;
		private Dictionary<Type, Dictionary<Identifier, object>> listItems;

		private static readonly Identifier DEFAULT_ID = "base";

		public void Setup()
		{
			sharedItems = new Dictionary<Type, Dictionary<Identifier, object>>();
			listItems = new Dictionary<Type, Dictionary<Identifier, object>>();
		}

		public void Start()
		{
		}

		public void Set<T>(T obj)
		{
			Set(obj, DEFAULT_ID);
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

		public T Get<T>() where T : class
		{
			return Get<T>(DEFAULT_ID);
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

		public void Unset<T>()
		{
			Unset<T>(DEFAULT_ID);
		}

		public void Unset<T>(string id)
		{
			var type = typeof(T);

			if (sharedItems.ContainsKey(type))
			{
				if (sharedItems[type].ContainsKey(id))
				{
					sharedItems[type].Remove(id);
				}
			}
		}

		public void ListAdd<T>(T obj)
		{
			ListAdd(obj, DEFAULT_ID);
		}

		public void ListAdd<T>(T obj, string id)
		{
			var type = typeof(T);
			List<T> list = null;

			if (!listItems.ContainsKey(type))
			{
				listItems.Add(type, new Dictionary<Identifier, object>());
			}

			if (!listItems[type].ContainsKey(id))
			{
				list = new List<T>();
				listItems[type][id] = list;
			}
			else
			{
				list = (List<T>)listItems[type][id];
			}

			list.Add(obj);
		}

		public List<T> ListGet<T>()
		{
			return ListGet<T>(DEFAULT_ID);
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

		public void ListRemove<T>(T obj)
		{
			ListRemove(obj, DEFAULT_ID);
		}

		public void ListRemove<T>(T obj, string id)
		{
			var type = typeof(T);

			if (listItems.ContainsKey(type))
			{
				if (listItems[type].ContainsKey(id))
				{
					var list = (List<T>)listItems[type][id];

					list.Remove(obj);
				}
			}
		}
	}
}
