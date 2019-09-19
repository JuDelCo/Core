
namespace Ju
{
	public static class ObservableHelper
	{
		public static Observable<T> New<T>(T value)
		{
			return new Observable<T>(value);
		}

		public static Observable<T> ToObservable<T>(this T value)
		{
			return new Observable<T>(value);
		}
	}
}
