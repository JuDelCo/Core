using System;
using System.Collections.Generic;
using System.Linq;

namespace Ju
{
	public delegate void PromiseUnhandledExceptionEvent(Exception e);

	public partial class Promise : IPromise
	{
		public static event PromiseUnhandledExceptionEvent OnUnhandledException = delegate { };

		internal static void NotifyUnhandledException(Exception e)
		{
			OnUnhandledException(e);
		}

		public static Promise Resolved()
		{
			var promise = new Promise();
			promise.Resolve();

			return promise;
		}

		public static Promise Rejected(Exception e)
		{
			var promise = new Promise();
			promise.Reject(e);

			return promise;
		}

		public static Promise<T> Resolved<T>(T value)
		{
			var promise = new Promise<T>();
			promise.Resolve(value);

			return promise;
		}

		public static Promise<T> Rejected<T>(Exception e)
		{
			var promise = new Promise<T>();
			promise.Reject(e);

			return promise;
		}

		public static IPromise All(params IPromise[] promises)
		{
			return All(promises.AsEnumerable());
		}

		public static IPromise All(IEnumerable<IPromise> promises)
		{
			var promiseList = promises.ToList();
			var counter = promiseList.Count;

			if (counter == 0)
			{
				return Promise.Resolved();
			}

			var resultPromise = new Promise();

			foreach (var promise in promiseList)
			{
				promise.Then(() =>
				{
					if (--counter <= 0 && resultPromise.CurrentState == PromiseState.Pending)
					{
						resultPromise.Resolve();
					}
				})
				.Catch((e) =>
				{
					if (resultPromise.CurrentState == PromiseState.Pending)
					{
						resultPromise.Reject(e);
					}
				});
			}

			return resultPromise;
		}

		public static IPromise Sequence(params Func<IPromise>[] functions)
		{
			return Sequence(functions.AsEnumerable());
		}

		public static IPromise Sequence(IEnumerable<Func<IPromise>> functions)
		{
			var resultPromise = new Promise();
			var startValue = (IPromise)Promise.Resolved();

			functions.Reduce(startValue, (previousPromise, function) =>
			{
				return previousPromise.Then(() => function());
			})
			.Then(resultPromise.Resolve)
			.Catch(resultPromise.Reject);

			return resultPromise;
		}

		public static IPromise Race(params IPromise[] promises)
		{
			return Race(promises.AsEnumerable());
		}

		public static IPromise Race(IEnumerable<IPromise> promises)
		{
			var promiseList = promises.ToList();

			if (promiseList.Count == 0)
			{
				return Promise.Resolved();
			}

			var resultPromise = new Promise();

			foreach (var promise in promiseList)
			{
				promise.Then(() =>
				{
					if (resultPromise.CurrentState == PromiseState.Pending)
					{
						resultPromise.Resolve();
					}
				})
				.Catch((e) =>
				{
					if (resultPromise.CurrentState == PromiseState.Pending)
					{
						resultPromise.Reject(e);
					}
				});
			}

			return resultPromise;
		}

		public static IPromise<T> Race<T>(params IPromise<T>[] promises)
		{
			return Race(promises.AsEnumerable());
		}

		public static IPromise<T> Race<T>(IEnumerable<IPromise<T>> promises)
		{
			var promiseList = promises.ToList();

			if (promiseList.Count == 0)
			{
				throw new Exception("No promises found in the list");
			}

			var resultPromise = new Promise<T>();

			foreach (var promise in promiseList)
			{
				promise.Then(value =>
				{
					if (resultPromise.CurrentState == PromiseState.Pending)
					{
						resultPromise.Resolve(value);
					}
				})
				.Catch((e) =>
				{
					if (resultPromise.CurrentState == PromiseState.Pending)
					{
						resultPromise.Reject(e);
					}
				});
			}

			return resultPromise;
		}
	}
}
