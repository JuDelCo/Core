// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Promises;

public static class PromiseUtilitiesExtensions
{
	public static IPromise Then(this IPromise promise, Action onResolved)
	{
		return promise.Then(onResolved, null);
	}

	public static IPromise Then(this IPromise promise, Func<IPromise> onResolved)
	{
		return promise.Then(onResolved, null);
	}

	public static IPromise<T> Then<T>(this IPromise promise, Func<IPromise<T>> onResolved)
	{
		return promise.Then(onResolved, null);
	}

	public static void Done(this IPromise promise, Action onResolved)
	{
		promise.Then(onResolved, null).Done();
	}

	public static void Done(this IPromise promise, Action onResolved, Action<Exception> onRejected)
	{
		promise.Then(onResolved, onRejected).Done();
	}

	public static IPromise<T> Then<T>(this IPromise<T> promise, Action<T> onResolved)
	{
		return promise.Then(onResolved, null);
	}

	public static IPromise Then<T>(this IPromise<T> promise, Func<T, IPromise> onResolved)
	{
		return promise.Then(onResolved, null);
	}

	public static IPromise<U> Then<T, U>(this IPromise<T> promise, Func<T, IPromise<U>> onResolved)
	{
		return promise.Then(onResolved, null);
	}

	public static void Done<T>(this IPromise<T> promise, Action<T> onResolved)
	{
		promise.Then(onResolved, null).Done();
	}

	public static void Done<T>(this IPromise<T> promise, Action<T> onResolved, Action<Exception> onRejected)
	{
		promise.Then(onResolved, onRejected).Done();
	}
}
