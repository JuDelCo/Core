using System;
using Ju.FSM;
using Ju.Handlers;
using Ju.Observables;
using Ju.Services;

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
}
