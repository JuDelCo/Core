// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

namespace Ju.Services
{
	public delegate bool UnityServiceQuitRequestedEvent();
	public delegate void UnityServiceQuitEvent();

	public interface IUnityService : IServiceLoad, ILoggableService
	{
		event UnityServiceQuitRequestedEvent OnApplicationWantsToQuit;
		event UnityServiceQuitEvent OnApplicationQuit;
	}
}

#endif
