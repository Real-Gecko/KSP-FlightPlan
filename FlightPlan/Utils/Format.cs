using System;
using UnityEngine;

namespace FlightPlan
{
	internal static class Format
	{
		public const double		MINUTE  = 60.0;
		public const double 	HOUR = 60.0 * 60.0; 
		public const double 	DAY  = 6.0 * 60.0 * 60.0; // Kerbin day's are 6 hours.
		public const double 	YEAR = 426.0 * DAY; // Kerbin years - 426 d 0 h 30 min
		public const double     ORBIT = 426.0 * DAY + 32.0 * MINUTE + 24.6; // Kerbin years - 426 d 0 h 32 min 24.6 s

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

		/// <summary>
		/// Gets the time string.
		/// </summary>
		/// <returns>The time string.</returns>
		/// <param name="value">Value.</param>
		internal static string UTTime(double value)
		{
			return Time (value + YEAR + DAY);
		}

		/// <summary>
		/// Gets the time string.
		/// </summary>
		/// <returns>The time string.</returns>
		/// <param name="value">Value.</param>
		internal static string Time(double value)
		{
			string result = "";
			if (value < 0.0) {
				result += "-";
				value = Math.Abs (value);
			}
			if (value > YEAR) {
				int years = (int)value / (int)YEAR;
				value -= (double)(years * YEAR);
				result += years + " y,";
			}
			if (value > DAY) {
				int days = (int)value / ((int)DAY);
				value -= (double)(days * DAY);
				result += days + " d,";
			}
			if (value > HOUR) {
				int hours = (int)value / ((int)HOUR);
				value -= (double)(hours * HOUR);
				result += hours.ToString("00") + ":";
			}

			if (value > MINUTE) {
				int mins = (int)value / 60;
				value -= (double)(mins * 60);
				result += mins.ToString("00") + ":";
			}
			result += value.ToString ("00");
			return result;
		}
	}
}

