// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using Ju.Handlers;
using Ju.Promises;
using Ju.Services;
using Ju.Time;

public static class ITaskServiceExtensions
{
	public static IPromise WaitForNextUpdate(this ITaskService taskService, ILinkHandler handle)
	{
		return taskService.WaitForSeconds<LoopUpdateEvent>(handle, 0f);
	}

	public static IPromise WaitForNextFixedUpdate(this ITaskService taskService, ILinkHandler handle)
	{
		return taskService.WaitForTicks<LoopFixedUpdateEvent>(handle, 1);
	}
}
