// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Hjson
{
	public static class JsonStringExtensions
	{
		public static bool IsValidHjson(this string hjson)
		{
			var result = false;

			if (!string.IsNullOrEmpty(hjson))
			{
				hjson = hjson.Trim();

				if ((hjson.StartsWith("{") && hjson.EndsWith("}")) || (hjson.StartsWith("[") && hjson.EndsWith("]")))
				{
					try
					{
						var obj = HjsonValue.Parse(hjson);
						result = true;
					}
					catch (InvalidOperationException) { }
					catch (ArgumentOutOfRangeException) { }
					catch (ArgumentNullException) { }
					catch (ArgumentException) { }
					catch (Exception) { }
				}
			}

			return result;
		}
	}
}
