// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using Ju.Data;
using Ju.Handlers;
using UnityEngine;

namespace Ju.Extensions
{
	public static class BehaviourDataExtensions
	{
		public static void NodeSubscribe(this Behaviour behaviour, JNode node, Action<JNode> action, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			node.Subscribe(linkHandler, action);
		}

		public static void NodeSubscribe(this Behaviour behaviour, JNode node, Action action, bool alwaysActive = false)
		{
			behaviour.NodeSubscribe(node, (_) => action(), alwaysActive);
		}

		public static void DataSubscribe<T>(this Behaviour behaviour, JData<T> node, Action<JData<T>> action, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			node.Subscribe(linkHandler, action);
		}

		public static Action<T> DataBind<T>(this Behaviour behaviour, JData<T> node, Action<JData<T>> action, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return node.Bind(linkHandler, action);
		}

		public static Action<TRemote> DataBind<T, TRemote>(this Behaviour behaviour, JData<T> node, Action<JData<T>> action, Func<TRemote, T> converter, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return node.Bind(linkHandler, action, converter);
		}
	}
}

#endif
