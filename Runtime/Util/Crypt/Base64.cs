// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Text;

namespace Ju.Crypt
{
	public static class Base64StringExtensions
	{
		public static string Base64Encode(this string self)
		{
			var bytes = Encoding.UTF8.GetBytes(self);

			return Convert.ToBase64String(bytes);
		}

		public static string Base64Decode(this string self)
		{
			var bytes = Convert.FromBase64String(self);

			return Encoding.UTF8.GetString(bytes);
		}
	}
}
