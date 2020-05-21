using System;
using System.Collections.Generic;

namespace Ju
{
	public class Promise<T> : IPromise<T>
	{
		public PromiseState CurrentState { get; private set; }

		private T resolveValue;
		private List<ResolveAction<T>> resolveActions = new List<ResolveAction<T>>();

		private Exception rejectException;
		private List<RejectAction> rejectActions = new List<RejectAction>();

		public Promise()
		{
			CurrentState = PromiseState.Pending;
		}

		public Promise(Action<Action<T>, Action<Exception>> resolver) : this()
		{
			try
			{
				resolver(Resolve, Reject);
			}
			catch (Exception e)
			{
				Reject(e);
			}
		}

		public IPromise<T> Then(Action<T> onResolved)
		{
			return Then(onResolved, null);
		}

		public IPromise<T> Then(Action<T> onResolved, Action<Exception> onRejected)
		{
			if (onResolved != null)
			{
				if (CurrentState == PromiseState.Resolved)
				{
					onResolved(resolveValue);
				}
				else if (CurrentState == PromiseState.Pending)
				{
					resolveActions.Add(new ResolveAction<T> { action = onResolved, rejectable = this });
				}
			}

			if (onRejected != null)
			{
				Catch(onRejected);
			}

			return this;
		}

		public IPromise Then(Func<T, IPromise> onResolved)
		{
			return Then(onResolved, null);
		}

		public IPromise Then(Func<T, IPromise> onResolved, Action<Exception> onRejected)
		{
			if (CurrentState == PromiseState.Resolved)
			{
				return onResolved(resolveValue);
			}
			else if (CurrentState == PromiseState.Rejected)
			{
				return Promise.Rejected(rejectException);
			}

			var resultPromise = new Promise();

			if (onResolved != null)
			{
				Action<T> resolveAction = (_) =>
				{
					var promise = onResolved(resolveValue);
					promise.Then(resultPromise.Resolve);
					promise.Catch(resultPromise.Reject);
				};

				resolveActions.Add(new ResolveAction<T> { action = resolveAction, rejectable = resultPromise });
			}

			Action<Exception> rejectAction;

			if (onRejected != null)
			{
				rejectAction = (e) =>
				{
					onRejected(e);
					resultPromise.Reject(e);
				};
			}
			else
			{
				rejectAction = resultPromise.Reject;
			}

			rejectActions.Add(new RejectAction { action = rejectAction, rejectable = resultPromise });

			return resultPromise;
		}

		public IPromise<U> Then<U>(Func<T, IPromise<U>> onResolved)
		{
			return Then(onResolved, null);
		}

		public IPromise<U> Then<U>(Func<T, IPromise<U>> onResolved, Func<Exception, IPromise<U>> onRejected)
		{
			if (CurrentState == PromiseState.Resolved)
			{
				return onResolved(resolveValue);
			}
			else if (CurrentState == PromiseState.Rejected)
			{
				return Promise.Rejected<U>(rejectException);
			}

			var resultPromise = new Promise<U>();

			if (onResolved != null)
			{
				Action<T> resolveAction = (_) =>
				{
					var promise = onResolved(resolveValue);
					promise.Then(resultPromise.Resolve);
					promise.Catch(resultPromise.Reject);
				};

				resolveActions.Add(new ResolveAction<T> { action = resolveAction, rejectable = resultPromise });
			}

			Action<Exception> rejectAction;

			if (onRejected != null)
			{
				rejectAction = (e) =>
				{
					var promise = onRejected(e);
					promise.Then(resultPromise.Resolve);
					promise.Catch(resultPromise.Reject);
				};
			}
			else
			{
				rejectAction = resultPromise.Reject;
			}

			rejectActions.Add(new RejectAction { action = rejectAction, rejectable = resultPromise });

			return resultPromise;
		}

		public IPromise ThenAll(Func<T, IEnumerable<IPromise>> chain)
		{
			return Then(_ => Promise.All(chain(resolveValue)));
		}

		public IPromise ThenSequence(Func<T, IEnumerable<Func<IPromise>>> chain)
		{
			return Then(_ => Promise.Sequence(chain(resolveValue)));
		}

		public IPromise ThenRace(Func<T, IEnumerable<IPromise>> chain)
		{
			return Then(_ => Promise.Race(chain(resolveValue)));
		}

		public IPromise Catch(Action<Exception> onRejected)
		{
			var resultPromise = new Promise();

			if (CurrentState != PromiseState.Pending)
			{
				resultPromise.Resolve();

				if (CurrentState == PromiseState.Rejected)
				{
					onRejected(rejectException);
				}

				return resultPromise;
			}

			Action<T> resolveAction = (_) =>
			{
				resultPromise.Resolve();
			};

			resolveActions.Add(new ResolveAction<T> { action = resolveAction, rejectable = resultPromise });

			Action<Exception> rejectAction = (e) =>
			{
				onRejected(e);
				resultPromise.Resolve();
			};

			rejectActions.Add(new RejectAction { action = rejectAction, rejectable = resultPromise });

			return resultPromise;
		}

		public IPromise ContinueWith(Func<IPromise> onComplete)
		{
			var resultPromise = new Promise();

			Then(_ => resultPromise.Resolve());
			Catch(_ => resultPromise.Resolve());

			return resultPromise.Then(onComplete);
		}

		public IPromise<T> Finally(Action onComplete)
		{
			if (CurrentState != PromiseState.Pending)
			{
				onComplete();
			}
			else
			{
				resolveActions.Add(new ResolveAction<T> { action = (_) => { onComplete(); }, rejectable = this });
				rejectActions.Add(new RejectAction { action = (_) => { onComplete(); }, rejectable = this });
			}

			return this;
		}

		public void Done()
		{
			if (rejectActions.Count == 0)
			{
				Catch(e => Promise.NotifyUnhandledException(e));
			}
		}

		public void Done(Action<T> onResolved)
		{
			Then(onResolved, null);
			Done();
		}

		public void Done(Action<T> onResolved, Action<Exception> onRejected)
		{
			Then(onResolved, onRejected);
			Done();
		}

		public void Resolve(T value)
		{
			if (CurrentState != PromiseState.Pending)
			{
				throw new Exception("Can't resolve a promise that is not in pending state");
			}

			resolveValue = value;
			CurrentState = PromiseState.Resolved;

			for (int i = 0; i < resolveActions.Count; ++i)
			{
				try
				{
					resolveActions[i].action(value);
				}
				catch (Exception e)
				{
					resolveActions[i].rejectable.Reject(e);
				}
			}

			resolveActions.Clear();
			rejectActions.Clear();
		}

		public void Reject(Exception e)
		{
			if (CurrentState != PromiseState.Pending)
			{
				throw new Exception("Can't reject a promise that is not in pending state");
			}

			rejectException = e;
			CurrentState = PromiseState.Rejected;

			for (int i = 0; i < rejectActions.Count; ++i)
			{
				try
				{
					rejectActions[i].action(e);
				}
				catch (Exception ex)
				{
					rejectActions[i].rejectable.Reject(ex);
				}
			}

			resolveActions.Clear();
			rejectActions.Clear();
		}
	}
}
