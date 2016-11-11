/*
 * UI Framework licensed under BSD 3-clause license
 * https://github.com/Real-Gecko/Unity5-UIFramework/blob/master/LICENSE.md
*/


using System;
using UnityEngine;

namespace FlightPlan
{
	internal class Styles
	{
		internal static GUIStyle label;
		internal static GUIStyle button;
		internal static GUIStyle window;
		internal static GUIStyle scrollView;
		internal static GUIStyle verticalScrollbar;
		internal static GUIStyle verticalScrollbarThumb;
		internal static GUIStyle selectionGrid;
		internal static GUIStyle toggle;

		internal static void InitStyles() {
			// Common label style
			label = new GUIStyle ();
			label.name = "UIFLabel";
			label.font = GUI.skin.font;
			label.fontSize = 14;
			label.fontStyle = FontStyle.Bold;
			label.stretchWidth = false;
			label.alignment = TextAnchor.MiddleLeft;
			label.margin = new RectOffset (0, 2, 0, 2);
			label.padding = new RectOffset (2, 2, 2, 2);

			// Common button style
			button = new GUIStyle ();
			button.name = "UIFButton";
			button.font = GUI.skin.font;
			button.fontSize = 14;
			button.fontStyle = FontStyle.Bold;
			button.margin = new RectOffset (0, 2, 0, 2);
			button.padding = new RectOffset(5, 5, 5, 5);
			button.normal.background = Palette.tGray;
			button.hover.background = Palette.tLightGray;
			button.hover.textColor = Color.white;

			// Selection grid buttons
			selectionGrid = new GUIStyle (button);
			selectionGrid.name = "UFISelectionGrid";
			selectionGrid.alignment = TextAnchor.MiddleCenter;
			selectionGrid.normal.textColor = Palette.darkGray;
			selectionGrid.onNormal.background = Palette.tGrimGray;
			selectionGrid.onNormal.textColor = Palette.green;

			// Window style
			window = new GUIStyle();
			window.name = "UIFWindow";
			window.normal.background = Palette.tDarkGray;

			window.fontSize = 14;
			window.fontStyle = FontStyle.Bold;
			window.normal.textColor = Palette.yellow;
			window.alignment = TextAnchor.UpperCenter;

			window.border = new RectOffset (5, 5, 18, 5);
			window.margin = new RectOffset (0, 0, 0, 0);
			window.padding = new RectOffset (5, 5, 20, 5);
			window.overflow = new RectOffset (0, 0, 0, 0);
			window.wordWrap = false;
			window.contentOffset = new Vector2 (0.0f, -18.0f);
			window.stretchWidth = true;
			window.stretchHeight = true;

			// Scrollview style
			scrollView = new GUIStyle();
			scrollView.name = "UIFScrollView";
			scrollView.font = GUI.skin.font;
			scrollView.fontSize = 14;
			scrollView.fontStyle = FontStyle.Bold;
			scrollView.margin = new RectOffset (0, 5, 0, 5);
			scrollView.padding = new RectOffset(0, 0, 0, 0);

			// Scrollbar style
			verticalScrollbar = new GUIStyle();
			verticalScrollbar.name = "UIFVerticalScrollbar";
			verticalScrollbar.margin = new RectOffset (0, 5, 0, 5);
			verticalScrollbar.padding = new RectOffset(0, 0, 0, 0);
			verticalScrollbar.normal.background = Palette.tGrimGray;
			verticalScrollbar.fixedWidth = 15;
			verticalScrollbar.fixedHeight = 0;
			verticalScrollbar.stretchWidth = true;
			verticalScrollbar.stretchHeight = true;

			// Vertical scrollbar thumb
			verticalScrollbarThumb = new GUIStyle();
			verticalScrollbarThumb.name = "UIFVerticalScrollbarThumb";
//			verticalScrollbarThumb.margin = new RectOffset (2, 2, 2, 2);
//			verticalScrollbarThumb.padding = new RectOffset(4, 4, 4, 4);
			verticalScrollbarThumb.normal.background = Palette.tGray;
			verticalScrollbarThumb.hover.background = Palette.tLightGray;
			verticalScrollbarThumb.fixedWidth = 15;
			verticalScrollbarThumb.fixedHeight = 0;
			verticalScrollbarThumb.stretchWidth = false;
			verticalScrollbarThumb.stretchHeight = true;
//			verticalScrollbarThumb.border = new RectOffset (6, 6, 6, 6);
//			verticalScrollbarThumb.margin = new RectOffset (0, 0, 0, 0);
//			verticalScrollbarThumb.padding = new RectOffset (0, 0, 6, 6);

			// Some stubs
			new GUIStyle().name = "UIFVerticalScrollBarUpButton";
			new GUIStyle().name = "UIFVerticalScrollBarDownButton";

//			Debug.Log("Skin toggle \n" + VarDump.var_dump(GUI.skin.toggle, 0, 3));

//			toggle = new GUIStyle (selectionGrid);
//			toggle.name = "toggle";
//			toggle.contentOffset = new Vector2(26, 0);
//			toggle.fixedWidth = 22;
//			toggle.fixedHeight = 22;
//			toggle.stretchWidth = true;
//			toggle.stretchHeight = false;
//			toggle.alignment = TextAnchor.MiddleLeft;
//			toggle.normal.background = Palette.tGrimGray;
//
//			toggle.onNormal.background = Palette.tGreen;
//			selectionGrid.alignment = TextAnchor.MiddleCenter;
//			selectionGrid.normal.textColor = Palette.darkGray;
//			selectionGrid.onNormal.background = Palette.tGrimGray;
//			selectionGrid.onNormal.textColor = Palette.green;

			toggle = new GUIStyle ();
			toggle.name = "UFIToggle";
			toggle.fontSize = 14;
			toggle.fontStyle = FontStyle.Bold;
			toggle.alignment = TextAnchor.MiddleLeft;
			toggle.normal.textColor = Palette.red;
			toggle.normal.background = Palette.tDarkGray;
			toggle.onNormal.textColor = Palette.green;
			toggle.onNormal.background = Palette.tDarkGray;
			toggle.margin = new RectOffset (0, 2, 0, 2);
			toggle.padding = new RectOffset(5, 5, 5, 5);
			toggle.fixedWidth = 0;
			toggle.fixedHeight = 0;
			toggle.stretchWidth = true;
			toggle.stretchHeight = false;
			toggle.font = HighLogic.Skin.font;
//			toggle.font = Font.CreateDynamicFontFromOSFont ("Georgia", 14);
//			toggle.font = HighLogic.Skin.font;
//			toggle.normal.background = Palette.tGrimGray;
//			toggle.onNormal.background = Palette.tGreen;
		}
	}
}

