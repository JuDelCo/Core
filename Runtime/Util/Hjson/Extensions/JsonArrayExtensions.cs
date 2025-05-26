// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Specialized;
using Ju.Handlers;
using Ju.Hjson;
using Ju.FSM;
using Ju.Services;

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
