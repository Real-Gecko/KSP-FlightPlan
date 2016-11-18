using System;
using UnityEngine;

namespace FlightPlan
{
	internal static class Format
	{
		/// <summary>
		/// Takes a number and formats it for display. Uses standard metric prefixes or scientific notation.
		/// </summary>
		/// <returns>The number string.</returns>
		/// <param name="value">Value.</param>
		internal static string Number(double value, string units = "")
		{
			string prefix = " ";
			if (value > 1e12d || (value < 1e-2d && value > 0.0d)) {
				return value.ToString ("e6") + prefix;
			}
			else if (value > 1e7d) {
				prefix = " M";
				value /= 1e6d;
			}
			else if (value > 1e4d) {
				prefix = " k";
				value /= 1e3d;
			}
			string result = value.ToString ("N2") + prefix + units;
			return result;
		}
	}
}

