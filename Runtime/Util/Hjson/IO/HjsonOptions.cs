// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System.Collections.Generic;

namespace Ju.Hjson
{
	/// <summary>Options for Save.</summary>
	public class HjsonOptions
	{
		IHjsonDsfProvider[] dsf;

		/// <summary>Initializes a new instance of this class.</summary>
		public HjsonOptions()
		{
			EmitRootBraces = true;
		}

		/// <summary>Keep white space and comments.</summary>
		public bool KeepWsc { get; set; }

		/// <summary>Show braces at the root level (default true).</summary>
		public bool EmitRootBraces { get; set; }

		/// <summary>
		/// Gets or sets DSF providers.
		/// </summary>
		public IEnumerable<IHjsonDsfProvider> DsfProviders
		{
			get { return dsf ?? System.Linq.Enumerable.Empty<IHjsonDsfProvider>(); }
			set { dsf = System.Linq.Enumerable.ToArray(value); }
		}
	}
}
