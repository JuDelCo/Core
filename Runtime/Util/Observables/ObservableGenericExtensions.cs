// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

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
