// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Data;
using Ju.Handlers;

namespace Ju.Services.Extensions
{
	public static class IServiceDataExtensions
	{
		public static void NodeSubscribe(this IService service, JNode node, Action<JNode> action)
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			node.Subscribe(linkHandler, action);
		}

		public static void NodeSubscribe(this IService service, JNode node, Action action)
		{
			service.NodeSubscribe(node, (_) => action());
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
}
