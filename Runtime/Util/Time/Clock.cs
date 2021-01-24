// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class Clock<T> : IClock, IDisposable where T : ILoopTimeEvent
	{
		private readonly DisposableLinkHandler linkHandler;
		private Span elapsed;
		private readonly Func<bool> updateCondition;

		public Clock()
		{
			linkHandler = new DisposableLinkHandler(false);
			ServiceContainer.Get<IEventBusService>().Subscribe<T>(linkHandler, e => Update(e.DeltaTime));
		}

		public Clock(float elapsedSeconds) : this()
		{
			elapsed = Span.Seconds(elapsedSeconds);
		}

		public Clock(Func<bool> updateCondition) : this()
		{
			this.updateCondition = updateCondition;
		}

		public Clock(float elapsedSeconds, Func<bool> updateCondition) : this(elapsedSeconds)
		{
			this.updateCondition = updateCondition;
		}

		public void Dispose()
		{
			linkHandler.Dispose();
			GC.SuppressFinalize(this);
		}

		public Span Reset()
		{
			var elapsedTime = elapsed;

			elapsed = Span.zero;

			return elapsedTime;
		}

		public Span GetElapsedTime()
		{
			return elapsed;
		}

		private void Update(float deltaTime)
		{
			if (updateCondition != null)
			{
				if (!updateCondition())
				{
					return;
				}
			}

			elapsed += Span.Seconds(deltaTime);
		}
	}
}
