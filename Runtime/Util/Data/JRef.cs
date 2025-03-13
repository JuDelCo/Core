// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Handlers;

namespace Ju.Data
{
	public class JRef : JNode
	{
		private readonly bool subscribeToRef;
		private readonly Action<JNode, JNodeEvent> cachedTrigger;
		private JNodeLinkHandler internalLinkHandler = null;
		private JNode reference = null;

		public JRef(JNode reference = null, bool subscribeToRef = false)
		{
			this.subscribeToRef = subscribeToRef;
			this.cachedTrigger = Trigger;

			if (reference != null)
			{
				Reference = reference;
			}
		}

		public override void Reset()
		{
			reference.Reset();
		}

		public override void Subscribe(ILinkHandler handle, Action<JNode, JNodeEvent> action)
		{
			base.Subscribe(handle, action);

			if (subscribeToRef && internalLinkHandler == null)
			{
				internalLinkHandler = new JNodeLinkHandler(this, false);
				reference.Subscribe(internalLinkHandler, cachedTrigger);
			}
		}

		public override JNode Clone()
		{
			return new JRef(this.Reference);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposedValue)
			{
				return;
			}

			if (disposing)
			{
				base.Dispose(disposing);

				Unsubscribe();

				if (reference != null)
				{
					reference.Dispose();
				}

				reference = null;
			}

			disposedValue = true;
		}

		public override IEnumerator<JNode> GetEnumerator()
		{
			return reference.GetEnumerator();
		}

		public override IEnumerable<JNode> Children
		{
			get => reference.Children;
		}

		protected override void OnSubscribersEmpty()
		{
			Unsubscribe();
		}

		public JNode Reference
		{
			get => reference;
			set
			{
				if (value == null)
				{
					throw new NullReferenceException();
				}

				Unsubscribe();

				reference = value;

				if (subscribeToRef && this.GetSubscriberCount() > 0)
				{
					internalLinkHandler = new JNodeLinkHandler(this, false);
					reference.Subscribe(internalLinkHandler, Trigger);
				}
			}
		}

		private void Unsubscribe()
		{
			if (internalLinkHandler != null)
			{
				internalLinkHandler.Dispose();
				internalLinkHandler = null;
			}
		}

		public override string ToString()
		{
			return $"Ref{{{reference}}}";
		}

		public override string ToString(int maxDepth)
		{
			if (maxDepth == 0)
			{
				return "Ref{...}";
			}

			return $"Ref{{{reference.ToString(maxDepth - 1)}}}";
		}
	}
}
