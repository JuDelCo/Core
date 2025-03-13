// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System.Linq;

namespace Ju.Crypt
{
	public static class SimpleHashStringExtensions
	{
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
