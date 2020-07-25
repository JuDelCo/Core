using System;
using System.Collections.Generic;

namespace Ju.Promises
{
	public partial class Promise : IPromise
	{
		public PromiseState CurrentState { get; private set; }

		private List<ResolveAction> resolveActions = new List<ResolveAction>();

		private Exception rejectException;
		private List<RejectAction> rejectActions = new List<RejectAction>();

		public Promise()
		{
			CurrentState = PromiseState.Pending;
		}

		public Promise(Action<Action, Action<Exception>> resolver) : this()
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

		public IPromise Then(Action onResolved, Action<Exception> onRejected)
		{
			if (onResolved != null)
			{
				if (CurrentState == PromiseState.Resolved)
				{
					onResolved();
				}
				else if (CurrentState == PromiseState.Pending)
				{
					resolveActions.Add(new ResolveAction { action = onResolved, rejectable = this });
				}
			}

			if (onRejected != null)
			{
				Catch(onRejected);
			}

			return this;
		}

		public IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected)
		{
			if (CurrentState == PromiseState.Resolved)
			{
				return onResolved();
			}
			else if (CurrentState == PromiseState.Rejected)
			{
				return Promise.Rejected(rejectException);
			}

			var resultPromise = new Promise();

			if (onResolved != null)
			{
				Action resolveAction = () =>
				{
					var promise = onResolved();
					promise.Then(resultPromise.Resolve);
					promise.Catch(resultPromise.Reject);
				};

				resolveActions.Add(new ResolveAction { action = resolveAction, rejectable = resultPromise });
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

		public IPromise<T> Then<T>(Func<IPromise<T>> onResolved, Func<Exception, IPromise<T>> onRejected)
		{
			if (CurrentState == PromiseState.Resolved)
			{
				return onResolved();
			}
			else if (CurrentState == PromiseState.Rejected)
			{
				return Promise.Rejected<T>(rejectException);
			}

			var resultPromise = new Promise<T>();

			if (onResolved != null)
			{
				Action resolveAction = () =>
				{
					var promise = onResolved();
					promise.Then(resultPromise.Resolve);
					promise.Catch(resultPromise.Reject);
				};

				resolveActions.Add(new ResolveAction { action = resolveAction, rejectable = resultPromise });
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

		public IPromise ThenAll(Func<IEnumerable<IPromise>> chain)
		{
			return Then(() => Promise.All(chain()), null);
		}

		public IPromise ThenSequence(Func<IEnumerable<Func<IPromise>>> chain)
		{
			return Then(() => Promise.Sequence(chain()), null);
		}

		public IPromise ThenRace(Func<IEnumerable<IPromise>> chain)
		{
			return Then(() => Promise.Race(chain()), null);
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

			Action resolveAction = () =>
			{
				resultPromise.Resolve();
			};

			resolveActions.Add(new ResolveAction { action = resolveAction, rejectable = resultPromise });

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

			Then(() => resultPromise.Resolve(), null);
			Catch(_ => resultPromise.Resolve());

			return resultPromise.Then(onComplete);
		}

		public IPromise Finally(Action onComplete)
		{
			if (CurrentState != PromiseState.Pending)
			{
				onComplete();
			}
			else
			{
				resolveActions.Add(new ResolveAction { action = onComplete, rejectable = this });
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

		public void Resolve()
		{
			if (CurrentState != PromiseState.Pending)
			{
				throw new Exception("Can't resolve a promise that is not in pending state");
			}

			CurrentState = PromiseState.Resolved;

			for (int i = 0, count = resolveActions.Count; i < count; ++i)
			{
				try
				{
					resolveActions[i].action();
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

			for (int i = 0, count = rejectActions.Count; i < count; ++i)
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
