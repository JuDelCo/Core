// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Promises
{
	public enum PromiseState
	{
		Pending,
		Rejected,
		Resolved,
	};

	public interface IRejectable
	{
		void Reject(Exception e);
	}

	public struct RejectAction
	{
		public Action<Exception> action;
		public IRejectable rejectable;
	}

	public interface IPromise : IRejectable
	{
		IPromise Then(Action onResolved, Action<Exception> onRejected);
		IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected);
		IPromise<T> Then<T>(Func<IPromise<T>> onResolved, Func<Exception, IPromise<T>> onRejected);
		IPromise ThenAll(Func<IEnumerable<IPromise>> chain);
		IPromise ThenSequence(Func<IEnumerable<Func<IPromise>>> chain);
		IPromise ThenRace(Func<IEnumerable<IPromise>> chain);
		IPromise Catch(Action<Exception> onRejected);
		IPromise ContinueWith(Func<IPromise> onComplete);
		IPromise Finally(Action onComplete);
		void Done();
		void Resolve();
	}

	public struct ResolveAction
	{
		public Action action;
		public IRejectable rejectable;
	}

	public interface IPromise<T> : IRejectable
	{
		IPromise<T> Then(Action<T> onResolved, Action<Exception> onRejected);
		IPromise Then(Func<T, IPromise> onResolved, Action<Exception> onRejected);
		IPromise<U> Then<U>(Func<T, IPromise<U>> onResolved, Func<Exception, IPromise<U>> onRejected);
		IPromise ThenAll(Func<T, IEnumerable<IPromise>> chain);
		IPromise ThenSequence(Func<T, IEnumerable<Func<IPromise>>> chain);
		IPromise ThenRace(Func<T, IEnumerable<IPromise>> chain);
		IPromise Catch(Action<Exception> onRejected);
		IPromise ContinueWith(Func<IPromise> onComplete);
		IPromise<T> Finally(Action onComplete);
		void Done();
		void Resolve(T value);
	}

	public struct ResolveAction<T>
	{
		public Action<T> action;
		public IRejectable rejectable;
	}
}
