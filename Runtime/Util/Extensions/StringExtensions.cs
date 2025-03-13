// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.IO;

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

		public static string SanitizePath(this string self)
		{
			var invalidCharacters = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

			foreach (char c in invalidCharacters)
			{
				self = self.Replace(c.ToString(), "");
			}

			return self;
		}

		public static string RemoveInvisibleCharacters(this string self)
		{
			return new string(self.Filter(x => !invisibleCharacters.Contains(x)).ToArray());
		}

		private static readonly char[] invisibleCharacters =
		{
			'\x00', // Ascii Table 0-31 (excluding tab, newline, return)
			'\x01',
			'\x02',
			'\x03',
			'\x04',
			'\x05',
			'\x06',
			'\x07',
			'\x08',
			'\x09',
			'\x0B',
			'\x0C',
			'\x0D',
			'\x0E',
			'\x0F',
			'\x10',
			'\x12',
			'\x13',
			'\x14',
			'\x16',
			'\x17',
			'\x18',
			'\x19',
			'\x1A',
			'\x1B',
			'\x1C',
			'\x1D',
			'\x1E',
			'\x1F',
			'\xA0',   // Non breaking space
			'\xAD',   // Soft hyphen
			'\u2000', // En quad
			'\u2001', // Em quad
			'\u2002', // En space
			'\u2003', // Em space
			'\u2004', // Three per em space
			'\u2005', // Four per em space
			'\u2006', // Six per em space
			'\u2007', // Figure space
			'\u2008', // Punctuation space
			'\u2009', // Thin space
			'\u200A', // Hair space
			'\u200B', // Zero width space
			'\u200C', // Zero width non-joiner
			'\u200D', // Zero width joiner
			'\u200E',
			'\u200F',
			'\u2010', // Hyphen
			'\u2011', // Non breaking hyphen
			'\u2012', // Figure dash
			'\u2013', // En dash
			'\u2014', // Em dash
			'\u2015', // Horizontal bar
			'\u2016', // Double vertical line
			'\u2017', // Double low line
			'\u2018', // Left single quotation mark
			'\u2019', // Right single quotation mark
			'\u201A', // Single low-9 quotation mark
			'\u201B', // Single high reversed-9 quotation mark
			'\u201C', // Left double quotation mark
			'\u201D', // Right double quotation mark
			'\u201E', // Double low-9 quotation mark
			'\u201F', // Double high reversed-9 quotation mark
			'\u2028', // Line separator
			'\u2029', // Paragraph separator
			'\u202F', // Narrow no-break space
			'\u205F', // Medium mathematical space
			'\u2060', // Word joiner
			'\u2420', // Symbol for space
			'\u2422', // Blank symbol
			'\u2423', // Open box
			'\u3000', // Ideographic space
			'\uFEFF'  // Zero width no-break space
		};
	}
}
