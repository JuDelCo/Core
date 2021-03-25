// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Handlers
{
	public interface ILinkHandler
	{
		bool IsActive { get; }
		bool IsDestroyed { get; }
	}
}
