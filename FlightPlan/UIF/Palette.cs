/*
 * UI Framework licensed under BSD 3-clause license
 * https://github.com/Real-Gecko/Unity5-UIFramework/blob/master/LICENSE.md
*/


using System;
using UnityEngine;

namespace FlightPlan
{
	internal struct Palette {
		// Colors
		internal static Color red = new Color (1.0f, 0.8f, 0.8f);
		public static Color green = new Color (0.8f, 1.0f, 0.8f);
		public static Color blue = new Color (0.7f, 0.7f, 1.0f);
		public static Color yellow = new Color(1.0f, 1.0f, 0.5f);
		public static Color gray = new Color (0.5f, 0.5f, 0.5f);
		public static Color lightGray = new Color(0.55f, 0.55f, 0.55f);
		public static Color darkGray = new Color(0.3f, 0.3f, 0.3f);
		public static Color grimGray = new Color(0.2f, 0.2f, 0.2f);

		// Color filled textures
		public static Texture2D tGreen = new Texture2D(1, 1);
		public static Texture2D tGray = new Texture2D(1, 1);
		public static Texture2D tLightGray = new Texture2D(1, 1);
		public static Texture2D tDarkGray = new Texture2D(1, 1);
		public static Texture2D tGrimGray = new Texture2D(1, 1);

		internal static void InitPalette() {
			tGreen.SetPixel (0, 0, green);
			tGreen.Apply ();

			tGray.SetPixel(0, 0, gray);
			tGray.Apply();

			tLightGray.SetPixel(0, 0, lightGray);
			tLightGray.Apply();

			tDarkGray.SetPixel(0, 0, darkGray);
			tDarkGray.Apply();

			tGrimGray.SetPixel(0, 0, grimGray);
			tGrimGray.Apply();
		}
	}
}

