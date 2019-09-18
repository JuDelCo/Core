using System;
using System.Collections.Generic;

namespace Ju
{
	public class DataService : IDataService
	{
		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private Dictionary<Type, object> sharedItems;
		private Dictionary<Type, object> items;

		public void Setup()
		{
			sharedItems = new Dictionary<Type, object>();
			items = new Dictionary<Type, object>();
		}

		public void Start()
		{
		}

		public void SetShared<T>(T obj)
		{
			var type = typeof(T);

			if (sharedItems.ContainsKey(type))
			{
				OnLogWarning("A shared object of type {0} was already stored");
			}

			sharedItems[type] = obj;
		}

		public T GetShared<T>() where T : class
		{
			var type = typeof(T);
			T instance = null;

			if (sharedItems.ContainsKey(type))
			{
				instance = (T)sharedItems[type];
			}

			return instance;
		}

		public void RemoveShared<T>()
		{
			var type = typeof(T);

			if (sharedItems.ContainsKey(type))
			{
				sharedItems.Remove(type);
			}
		}

		public void Add<T>(T obj)
		{
			var type = typeof(T);
			List<T> list = null;

			if (!items.ContainsKey(type))
			{
				list = new List<T>();
				items[type] = list;
			}
			else
			{
				list = (List<T>)items[type];
			}

			list.Add(obj);
		}

		public List<T> GetList<T>()
		{
			var type = typeof(T);
			List<T> list = null;

			if (items.ContainsKey(type))
			{
				list = (List<T>)items[type];
			}

			return list;
		}

		public void Remove<T>(T obj)
		{
			var type = typeof(T);

			if (items.ContainsKey(type))
			{
				var list = (List<T>)items[type];

				list.Remove(obj);

				if (list.Count == 0)
				{
					items.Remove(type);
				}
			}
		}
	}
}
