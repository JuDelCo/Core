// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;
using Ju.Extensions;

namespace Ju.Data
{
	public class JList<T> : JList, IList<T> where T : JNode
	{
		public JList() : this(true, 0, true)
		{
		}

		public JList(bool reseteable, bool subscribeToChildren = true) : this(reseteable, 0, subscribeToChildren)
		{
		}

		public JList(bool reseteable, int capacity, bool subscribeToChildren = true) : base(reseteable, capacity, subscribeToChildren)
		{
		}

		public new T this[int index]
		{
			get => base[index] as T;
			set => base[index] = value;
		}

		public void Add(T node)
		{
			this.Add((JNode)node);
		}

		public void Add(IEnumerable<T> nodes)
		{
			this.Add(nodes.Map(n => n as JNode));
		}

		public void Insert(int index, T node)
		{
			this.Insert(index, (JNode)node);
		}

		public bool Contains(T node)
		{
			return this.Contains((JNode)node);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.CopyTo(array.Map(n => n as JNode).ToArray(), arrayIndex);
		}

		public new IEnumerator<T> GetEnumerator()
		{
			return ((JList)this).Map(n => n as T).GetEnumerator();
		}

		public int IndexOf(T node)
		{
			return this.IndexOf((JNode)node);
		}

		public bool Remove(T node)
		{
			return this.Remove((JNode)node);
		}
	}
}
