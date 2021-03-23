// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Promises;
using Ju.Time;

namespace Ju.Services
{
	public interface ITaskService
	{
		void RunOnMainThread(Action action, float delay = 0f);

		IPromise WaitUntil(ILinkHandler handle, Func<bool> condition);
		IPromise WaitWhile(ILinkHandler handle, Func<bool> condition);
		IPromise WaitForSeconds<T>(ILinkHandler handle, float seconds) where T : ITimeDeltaEvent;
		IPromise WaitForTicks<T>(ILinkHandler handle, int ticks) where T : ITimeEvent;
	}
}
