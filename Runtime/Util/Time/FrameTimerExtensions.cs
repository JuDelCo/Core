// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.FSM;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public partial class FrameTimer<T> : IFrameTimer where T : ITimeEvent
	{
		public FrameTimer(int frames, Action onCompleted, IService service) : this(frames, onCompleted)
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			this.updateCondition = () => linkHandler.IsActive;
		}

		public FrameTimer(int frames, Action onCompleted, State state) : this(frames, onCompleted)
		{
			var linkHandler = new StateLinkHandler(state);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}
