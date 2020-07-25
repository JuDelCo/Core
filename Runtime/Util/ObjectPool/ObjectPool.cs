using System.Collections.Generic;

namespace Ju.Util
{
	public class ObjectPool<T> where T : new()
	{
		private readonly Stack<T> pool;

		public ObjectPool()
		{
			pool = new Stack<T>();
		}

		public T Get()
		{
			if (pool.Count != 0)
			{
				return pool.Pop();
			}

			return new T();
		}

		public void Push(T obj)
		{
			pool.Push(obj);
		}
	}
}
