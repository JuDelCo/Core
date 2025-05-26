// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Data;
using Ju.Handlers;
using Ju.Services;

public static class IServiceDataExtensions
{
	public static void NodeSubscribe(this IService service, JNode node, Action<JNode, JNodeEvent> action)
	{
		var linkHandler = new ObjectLinkHandler<IService>(service);
		node.Subscribe(linkHandler, action);
	}

	public static void NodeSubscribe(this IService service, JNode node, JNodeEvent eventFilter, Action<JNode> action)
	{
		var linkHandler = new ObjectLinkHandler<IService>(service);
		node.Subscribe(linkHandler, (n, e) =>
		{
			if (e == eventFilter)
			{
				action(n);
			}
		});
	}

	public static void NodeSubscribe<T>(this IService service, JNode node, JNodeEvent eventFilter, Action<T> action) where T : JNode
	{
		service.NodeSubscribe(node, eventFilter, n =>
		{
			if (n is T jNode)
			{
				action(jNode);
			}
		});
	}

	public static void NodeSubscribe(this IService service, JNode node, Action action)
	{
		service.NodeSubscribe(node, (n, e) => action());
	}

	public static void NodeSubscribe(this IService service, JNode node, JNodeEvent eventFilter, Action action)
	{
		service.NodeSubscribe(node, eventFilter, n => action());
	}

	public static void DataSubscribe<T>(this IService service, JData<T> node, Action<JData<T>> action)
	{
		var linkHandler = new ObjectLinkHandler<IService>(service);
		node.Subscribe(linkHandler, action);
	}

	public static Action<T> DataBind<T>(this IService service, JData<T> node, Action<JData<T>> action)
	{
		var linkHandler = new ObjectLinkHandler<IService>(service);
		return node.Bind(linkHandler, action);
	}

	public static Action<TRemote> DataBind<T, TRemote>(this IService service, JData<T> node, Action<JData<T>> action, Func<TRemote, T> converter)
	{
		var linkHandler = new ObjectLinkHandler<IService>(service);
		return node.Bind(linkHandler, action, converter);
	}
}
