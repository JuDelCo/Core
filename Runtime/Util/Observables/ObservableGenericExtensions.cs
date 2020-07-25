using Ju.Observables;

namespace Ju.Extensions
{
	public static class ObservableGenericExtensions
	{
		public static Observable<T> ToObservable<T>(this T value)
		{
			return new Observable<T>(value);
		}
	}
}
