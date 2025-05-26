// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.FSM;
using Ju.Handlers;
using Ju.Observables;
using Ju.Services;

public static class ObservableExtensions
{
	public static void Subscribe<T>(this Observable<T> observable, IService service, Action<T> action)
	{
		observable.Subscribe(new ObjectLinkHandler<IService>(service), action);
	}

	public static void Subscribe<T>(this Observable<T> observable, State state, Action<T> action)
	{
		observable.Subscribe(new StateLinkHandler(state), action);
	}
}
