// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using Ju.FSM;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public partial class Clock<T> : IClock where T : ITimeDeltaEvent
	{
		public Clock(IService service) : this()
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			this.updateCondition = () => linkHandler.IsActive;
		}

		public Clock(float elapsedSeconds, IService service) : this(elapsedSeconds)
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			this.updateCondition = () => linkHandler.IsActive;
		}

		public Clock(State state) : this()
		{
			var linkHandler = new StateLinkHandler(state);
			this.updateCondition = () => linkHandler.IsActive;
		}

		public Clock(float elapsedSeconds, State state) : this(elapsedSeconds)
		{
			var linkHandler = new StateLinkHandler(state);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}
