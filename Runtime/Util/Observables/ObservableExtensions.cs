using System;

namespace Ju
{
	public static class ObservableExtensions
	{
		public static void Subscribe<T>(this Observable<T> observable, IService service, Action<T> action)
		{
			observable.Subscribe(new KeepLinkHandler(), action);
		}

		public static void Subscribe<T>(this Observable<T> observable, State state, Action<T> action)
		{
			observable.Subscribe(new StateLinkHandler(state), action);
		}

		public static Observable<T> ToObservable<T>(this T value)
		{
			return new Observable<T>(value);
		}
	}
}
