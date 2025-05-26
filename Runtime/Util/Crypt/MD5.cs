// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System.Security.Cryptography;
using System.Text;

namespace Ju.Extensions
{
	public static class MD5StringExtensions
	{
		public static string CalculateMD5(this string self)
		{
			var bytes = Encoding.ASCII.GetBytes(self);

			return CalculateMD5(bytes);
		}

		public static string CalculateMD5(this byte[] self)
		{
			var md5 = MD5.Create();
			var hash = md5.ComputeHash(self);
			var sb = new StringBuilder();

			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("x2"));
			}

			return sb.ToString();
		}
	}
}
