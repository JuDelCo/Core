// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;

namespace Ju.Data
{
	internal class JDataHandleActionPair
	{
		public ILinkHandler handle;
		public Action<JNode> action;

		public JDataHandleActionPair(ILinkHandler handle, Action<JNode> action)
		{
			this.handle = handle;
			this.action = action;
		}
	}

	internal class JDataHandleActionPair<T>
	{
		public ILinkHandler handle;
		public Action<T> action;

		public JDataHandleActionPair(ILinkHandler handle, Action<T> action)
		{
			this.handle = handle;
			this.action = action;
		}
	}
}
