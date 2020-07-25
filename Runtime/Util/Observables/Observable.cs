
namespace Ju.Observables
{
	public static class Observable
	{
		public static Observable<T> New<T>(T value)
		{
			return new Observable<T>(value);
		}
	}
}
