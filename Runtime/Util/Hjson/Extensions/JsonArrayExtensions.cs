// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.FSM;
using Ju.Services;
using System.Collections.Specialized;

namespace Ju.Hjson
{
	public static class JsonArrayExtensions
	{
		public static void Subscribe(this JsonArray array, IService service, Action<NotifyCollectionChangedEventArgs> action)
		{
			array.Subscribe(new ObjectLinkHandler<IService>(service), action);
		}

		public static void Subscribe(this JsonArray array, State state, Action<NotifyCollectionChangedEventArgs> action)
		{
			array.Subscribe(new StateLinkHandler(state), action);
		}
	}
}
