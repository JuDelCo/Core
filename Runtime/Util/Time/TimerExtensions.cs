// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.FSM;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public partial class Timer<T> : ITimer where T : ITimeDeltaEvent
	{
		public Timer(float seconds, Action onCompleted, IService service) : this(seconds, onCompleted)
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			this.updateCondition = () => linkHandler.IsActive;
		}

		public Timer(float seconds, Action onCompleted, State state) : this(seconds, onCompleted)
		{
			var linkHandler = new StateLinkHandler(state);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}
