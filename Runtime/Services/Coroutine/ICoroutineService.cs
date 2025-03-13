// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System.Collections;
using Ju.Handlers;

namespace Ju.Services
{
	public interface ICoroutineService
	{
		Coroutine StartCoroutine(ILinkHandler handle, IEnumerator routine);
	}
}
