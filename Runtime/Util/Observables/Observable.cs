// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

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
