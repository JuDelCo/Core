// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Ju.Data;
using Ju.Handlers;
using Godot;

namespace Ju.Extensions
{
	public static class NodeDataExtensions
	{
		public static void NodeSubscribe(this Node node, JNode jNode, Action<JNode, JNodeEvent> action, bool alwaysActive = false)
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			jNode.Subscribe(linkHandler, action);
		}

		public static void NodeSubscribe(this Node node, JNode jNode, JNodeEvent eventFilter, Action<JNode> action, bool alwaysActive = false)
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			jNode.Subscribe(linkHandler, (n, e) =>
			{
				if (e == eventFilter)
				{
					action(n);
				}
			});
		}

		public static void NodeSubscribe<T>(this Node node, JNode jNode, JNodeEvent eventFilter, Action<T> action, bool alwaysActive = false) where T : JNode
		{
			node.NodeSubscribe(jNode, eventFilter, n =>
			{
				if (n is T jNode)
				{
					action(jNode);
				}
			}, alwaysActive);
		}

		public static void NodeSubscribe(this Node node, JNode jNode, Action action, bool alwaysActive = false)
		{
			node.NodeSubscribe(jNode, (n, e) => action(), alwaysActive);
		}

		public static void NodeSubscribe(this Node node, JNode jNode, JNodeEvent eventFilter, Action action, bool alwaysActive = false)
		{
			node.NodeSubscribe(jNode, eventFilter, n => action(), alwaysActive);
		}

		public static void DataSubscribe<T>(this Node node, JData<T> jNode, Action<JData<T>> action, bool alwaysActive = false)
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			jNode.Subscribe(linkHandler, action);
		}

		public static Action<T> DataBind<T>(this Node node, JData<T> jNode, Action<JData<T>> action, bool alwaysActive = false)
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			return jNode.Bind(linkHandler, action);
		}

		public static Action<TRemote> DataBind<T, TRemote>(this Node node, JData<T> jNode, Action<JData<T>> action, Func<TRemote, T> converter, bool alwaysActive = false)
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			return jNode.Bind(linkHandler, action, converter);
		}
	}
}

#endif
