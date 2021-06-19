// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Services
{
	public interface ICacheService : IService
	{
		void Set<T>(T obj, string id, bool overwrite = true);
		T Get<T>(string id) where T : class;
		void Unset<T>(string id);

		IEnumerable<Type> GetTypes();

		void ListAdd<T>(T obj, string id);
		List<T> ListGet<T>(string id);
		void ListRemove<T>(T obj, string id);

		IEnumerable<Type> ListGetTypes();
	}
}
