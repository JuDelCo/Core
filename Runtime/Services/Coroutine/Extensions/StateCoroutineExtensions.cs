// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections;
using Ju.FSM;
using Ju.Services;
using Ju.Services.Internal;

public static class StateCoroutineExtensions
{
	public static Coroutine CoroutineStart(this State state, IEnumerator routine)
	{
		return ServiceCache.Coroutine.StartCoroutine(new StateLinkHandler(state), routine);
	}
}
