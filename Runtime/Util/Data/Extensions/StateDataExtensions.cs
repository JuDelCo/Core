// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Data;
using Ju.FSM;

public static class StateDataExtensions
{
	public static void NodeSubscribe(this State state, JNode node, Action<JNode, JNodeEvent> action)
	{
		var linkHandler = new StateLinkHandler(state);
		node.Subscribe(linkHandler, action);
	}

	public static void NodeSubscribe(this State state, JNode node, JNodeEvent eventFilter, Action<JNode> action)
	{
		var linkHandler = new StateLinkHandler(state);
		node.Subscribe(linkHandler, (n, e) =>
		{
			if (e == eventFilter)
			{
				action(n);
			}
		});
	}

	public static void NodeSubscribe<T>(this State state, JNode node, JNodeEvent eventFilter, Action<T> action) where T : JNode
	{
		state.NodeSubscribe(node, eventFilter, n =>
		{
			if (n is T jNode)
			{
				action(jNode);
			}
		});
	}

	public static void NodeSubscribe(this State state, JNode node, Action action)
	{
		state.NodeSubscribe(node, (n, e) => action());
	}

	public static void NodeSubscribe(this State state, JNode node, JNodeEvent eventFilter, Action action)
	{
		state.NodeSubscribe(node, eventFilter, n => action());
	}

	public static void DataSubscribe<T>(this State state, JData<T> node, Action<JData<T>> action)
	{
		var linkHandler = new StateLinkHandler(state);
		node.Subscribe(linkHandler, action);
	}

	public static Action<T> DataBind<T>(this State state, JData<T> node, Action<JData<T>> action)
	{
		var linkHandler = new StateLinkHandler(state);
		return node.Bind(linkHandler, action);
	}

	public static Action<TRemote> DataBind<T, TRemote>(this State state, JData<T> node, Action<JData<T>> action, Func<TRemote, T> converter)
	{
		var linkHandler = new StateLinkHandler(state);
		return node.Bind(linkHandler, action, converter);
	}
}
