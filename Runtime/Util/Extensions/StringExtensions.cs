// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Linq;

namespace Ju.Extensions
{
	public static class StringExtensions
	{
		public static bool ContainsCaseInsensitive(this string self, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
		{
			return (self.IndexOf(value, stringComparison) >= 0);
		}

		public static string Truncate(this string self, int maxLength)
		{
			if (string.IsNullOrEmpty(self))
			{
				return self;
			}

			return (self.Length <= maxLength ? self : self.Substring(0, maxLength));
		}

		public static int GetSimpleHash(this string self)
		{
			return self.Select(c => (int)c).Sum();
		}

		public static int GetDeterministicHashCode(this string self)
		{
			unchecked
			{
				int hash1 = (5381 << 16) + 5381;
				int hash2 = hash1;

				for (int i = 0, length = self.Length; i < length; i += 2)
				{
					hash1 = ((hash1 << 5) + hash1) ^ self[i];

					if (i == self.Length - 1)
					{
						break;
					}

					hash2 = ((hash2 << 5) + hash2) ^ self[i + 1];
				}

				return hash1 + (hash2 * 1566083941);
			}
		}
	}
}
