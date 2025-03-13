// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

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

		public ObjectPool(int capacity) : this()
		{
			for (int i = 0; i < capacity; ++i)
			{
				Push(new T());
			}
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
