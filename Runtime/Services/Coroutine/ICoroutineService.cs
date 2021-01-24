// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections;
using Ju.Handlers;

namespace Ju.Services
{
	public interface ICoroutineService : IServiceLoad, ILoggableService
	{
		Coroutine StartCoroutine(ILinkHandler handle, IEnumerator routine);
	}
}
