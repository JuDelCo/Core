// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Handlers
{
	public struct KeepLinkHandler : ILinkHandler
	{
		public bool IsActive => true;
		public bool IsDestroyed => false;
	}
}
