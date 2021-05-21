// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Data;
using Ju.FSM;

public static class StateDataExtensions
{
	public static void NodeSubscribe(this State state, JNode node, Action<JNode> action)
	{
		var linkHandler = new StateLinkHandler(state);
		node.Subscribe(linkHandler, action);
	}

	public static void NodeSubscribe(this State state, JNode node, Action action)
	{
		state.NodeSubscribe(node, (_) => action());
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
