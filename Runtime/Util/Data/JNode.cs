// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using System.Collections.Generic;
using Ju.Handlers;

namespace Ju.Data
{
	public abstract class JNode : IDisposable, IEnumerable<JNode>
	{
		private List<JDataHandleActionPair> actions = null;
		private List<JDataHandleActionPair> cachedActions = null;
		private JNode parent = null;
		private bool isDetaching = false;
		protected bool disposedValue = false;
		protected uint callStackCounter = 0;

		public abstract void Reset();

		public virtual void Subscribe(ILinkHandler handle, Action<JNode, JNodeEvent> action)
		{
			if (actions == null)
			{
				actions = new List<JDataHandleActionPair>(2);
			}

			actions.Add(new JDataHandleActionPair(handle, action));
		}

		internal bool IsSubscribed(JNode node)
		{
			if (actions != null)
			{
				for (int i = (actions.Count - 1); i >= 0; --i)
				{
					if (actions[i].handle is JNodeLinkHandler handler)
					{
						if (handler.Node == node)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public virtual int GetSubscriberCount()
		{
			if (actions != null)
			{
				return actions.Count;
			}

			return 0;
		}

		public void Detach()
		{
			if (parent != null)
			{
				if (parent.IsDict())
				{
					isDetaching = true;
					parent.AsDict().Remove(this);
					isDetaching = false;
				}
				else if (parent.IsList())
				{
					isDetaching = true;
					parent.AsList().Remove(this);
					isDetaching = false;
				}

				if (actions != null)
				{
					for (int i = (actions.Count - 1); i >= 0; --i)
					{
						if (actions[i].handle is JNodeLinkHandler handler)
						{
							if (handler.Node == parent)
							{
								actions.RemoveAt(i);

								if (actions.Count <= 0)
								{
									OnSubscribersEmpty();
								}

								break;
							}
						}
					}
				}
			}

			parent = null;
		}

		public abstract JNode Clone();

		public bool IsDisposed()
		{
			return disposedValue;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposedValue)
			{
				return;
			}

			if (disposing)
			{
				Detach();

				if (actions != null)
				{
					actions.Clear();
					OnSubscribersEmpty();
				}
			}

			disposedValue = true;
		}

		public abstract IEnumerator<JNode> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public JNode Parent
		{
			get => parent;
			internal set
			{
				if (isDetaching)
				{
					return;
				}

				if (value == null)
				{
					Dispose();
				}
				else
				{
					Detach();
				}

				parent = value;
			}
		}

		public abstract IEnumerable<JNode> Children { get; }

		public string Path
		{
			get
			{
				if (parent != null)
				{
					if (parent.IsList())
					{
						return $"{parent.Path}.{parent.AsList().IndexOf(this)}";
					}
					else if (parent.IsDict())
					{
						return $"{parent.Path}.{parent.AsDict().GetKey(this)}";
					}
				}

				return "root";
			}
		}

		internal void Trigger(JNode node, JNodeEvent eventType)
		{
			if (actions == null || actions.Count == 0)
			{
				return;
			}

			if (cachedActions == null)
			{
				cachedActions = new List<JDataHandleActionPair>();
			}

			cachedActions.AddRange(actions);

			for (int i = 0; i < cachedActions.Count; ++i)
			{
				if (callStackCounter > 0)
				{
					throw new Exception("A JNode callback has modified the same value that triggered it.");
				}

				var handle = cachedActions[i].handle;

				if (!handle.IsActive)
				{
					if (handle.IsDestroyed)
					{
						actions.Remove(cachedActions[i]);

						if (actions.Count <= 0)
						{
							OnSubscribersEmpty();
						}
					}

					continue;
				}

				++callStackCounter;

				cachedActions[i].action(node, eventType);

				--callStackCounter;
			}

			cachedActions.Clear();
		}

		protected virtual void OnSubscribersEmpty()
		{
		}

		public abstract string ToString(int maxDepth);
	}
}
