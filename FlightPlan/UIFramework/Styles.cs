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
		internal static GUIStyle textCommon;

		internal static GUIStyle box;
		internal static GUIStyle label;
		internal static GUIStyle textField;
		internal static GUIStyle textArea;
		internal static GUIStyle button;
		internal static GUIStyle toggle;
		internal static GUIStyle window;
		internal static GUIStyle horizontalSlider;
		internal static GUIStyle horizontalSliderThumb;
		internal static GUIStyle verticalSlider;
		internal static GUIStyle verticalSliderThumb;
		internal static GUIStyle horizontalScrollbar;
		internal static GUIStyle horizontalScrollbarThumb;
		internal static GUIStyle horizontalScrollbarLeftButton;
		internal static GUIStyle horizontalScrollbarRightButton;
		internal static GUIStyle verticalScrollbar;
		internal static GUIStyle verticalScrollbarThumb;
		internal static GUIStyle verticalScrollbarUpButton;
		internal static GUIStyle verticalScrollbarDownButton;
		internal static GUIStyle scrollView;

		internal static GUIStyle selectionGrid;

		internal const int fontSize = 14;
		internal static Font mainFont = GUI.skin.font;
		internal static FontStyle fontStyle = FontStyle.Bold;

		internal static void InitStyles() {
			// Defaults

			// normal, hover, active, onNormal, onHover, onActive, focused, onFocused
			// --- background = null
			// --- textColor = RGBA(0.000, 0.000, 0.000, 1.000)
			//
			// border = RectOffset (l:0 r:0 t:0 b:0)
			// margin = RectOffset (l:0 r:0 t:0 b:0)
			// padding = RectOffset (l:0 r:0 t:0 b:0)
			// overflow = RectOffset (l:0 r:0 t:0 b:0)
			// clipOffset = Vector2 (0.0, 0.0)
			//
			// font = null
			// imagePosition = ImagePosition.ImageLeft
			// alignment = TextAnchor.UpperLeft
			// wordWrap = false
			// clipping = TextClipping.Overflow
			// contentOffset = Vector2 (0.0, 0.0)
			// fixedWidth = 0
			// fixedHeight = 0
			// stretchWidth = true
			// stretchHeight = false
			// fontSize = 0
			// fontStyle = FontStyle.Normal
			// richText = true

			// Common text style
			textCommon = new GUIStyle();
			textCommon.name = "UIFTextCommon";

			textCommon.font = GUI.skin.font;
			textCommon.fontSize = fontSize;
			textCommon.fontStyle = fontStyle;

			textCommon.normal.textColor = Palette.white;

			textCommon.margin = Offsets.square2;
			textCommon.padding = Offsets.square4;

			// Box style
			box = new GUIStyle(textCommon);
			box.name = "UIFBox";

			box.normal.background = Palette.tGray20;

			box.alignment = TextAnchor.MiddleCenter;

			box.wordWrap = true;
			box.clipping = TextClipping.Clip;

			// Label style
			label = new GUIStyle (textCommon);
			label.name = "UIFLabel";

			label.normal.background = Palette.tTransparent;

			label.wordWrap		= true;
			label.stretchWidth	= false;

			// TextField style
			textField = new GUIStyle(textCommon);
			textField.name = "UIFTextField";

			textField.normal.background = Palette.tGray10;
			textField.normal.textColor = Palette.dimWhite;

			textField.hover.background = Palette.tGray10;
			textField.hover.textColor = Palette.white;

			textField.onNormal.background = Palette.tGray10;
			textField.onHover.textColor = Palette.white;

			textField.focused.background = Palette.tGray10;
			textField.focused.textColor = Palette.white;

			textField.imagePosition = ImagePosition.TextOnly;
			textField.clipping = TextClipping.Clip;

			// TextArea style
			textArea = new GUIStyle(textField);
			textArea.name = "UIFTextArea";

			textArea.wordWrap = true;

			// Button style
			button = new GUIStyle (textCommon);
			button.name = "UIFButton";

			button.normal.background = Palette.tGray50;
			button.normal.textColor = Palette.white;

			button.hover.background = Palette.tGray55;
			button.hover.textColor = Palette.white;

			// Toggle
			toggle = new GUIStyle (button);
			toggle.name = "UIFToggle";

			toggle.normal.textColor = Palette.red;
			toggle.normal.background = Palette.tTransparent;

			toggle.hover.textColor = Palette.red;
			toggle.hover.background = Palette.tTransparent;

			toggle.onNormal.textColor = Palette.green;
			toggle.onNormal.background = Palette.tTransparent;

			toggle.border = new RectOffset (15, 15, 8, 8);

			toggle.clipping = TextClipping.Clip;

			toggle.alignment = TextAnchor.MiddleLeft;

			// Window style
			window = new GUIStyle(textCommon);
			window.name = "UIFWindow";

			window.normal.background = Palette.tBorder;
//			window.normal.background = Palette.tGray30;
			window.normal.textColor = Palette.yellow;

			window.border = Offsets.square2;
			window.padding = new RectOffset (8, 8, 28, 8);

			window.alignment = TextAnchor.UpperCenter;
			window.clipping = TextClipping.Clip;
			window.contentOffset = new Vector2 (0.0f, -22.0f);
			window.fontSize = 16;

			// HorizontalSlider
			horizontalSlider = new GUIStyle();
			horizontalSlider.name = "UIFHorizontalSlider";

			horizontalSlider.normal.background = Palette.tGray10;

			horizontalSlider.margin = Offsets.square2;
			horizontalSlider.padding = Offsets.square2;

			horizontalSlider.imagePosition = ImagePosition.ImageOnly;
			horizontalSlider.clipping = TextClipping.Clip;

			horizontalSlider.fixedHeight = 17;

			// HorizontalSliderThumb
			horizontalSliderThumb = new GUIStyle();
			horizontalSliderThumb.name = "UIFHorizontalSliderThumb";

			horizontalSliderThumb.normal.background = Palette.tGray30;

			horizontalSliderThumb.hover.background = Palette.tGray40;

			horizontalSliderThumb.fixedWidth = 30;
			horizontalSliderThumb.fixedHeight = 13;

			//Vertical slider
			verticalSlider = new GUIStyle();
			verticalSlider.name = "UIFVerticalSlider";

			verticalSlider.normal.background = Palette.tGray10;
			verticalSlider.normal.textColor = Palette.transparent;

			verticalSlider.margin = Offsets.square2;
			verticalSlider.padding = Offsets.square2;

			horizontalSlider.imagePosition = ImagePosition.ImageOnly;
			horizontalSlider.clipping = TextClipping.Clip;

			verticalSlider.fixedWidth = 17;
			verticalSlider.stretchWidth = false;
			verticalSlider.stretchHeight = true;

			// Vertical slider thumb
			verticalSliderThumb = new GUIStyle();
			verticalSliderThumb.name = "UIFVerticalSliderThumb";

			verticalSliderThumb.normal.background = Palette.tGray30;

			verticalSliderThumb.hover.background = Palette.tGray40;

			verticalSliderThumb.fixedWidth = 13;
			verticalSliderThumb.fixedHeight = 30;

			// Horizontal scroll bar
			horizontalScrollbar = new GUIStyle();
			horizontalScrollbar.name = "UIFHorizontalScrollbar";

			horizontalScrollbar.normal.background = Palette.tGray10;
			horizontalScrollbar.normal.textColor = Palette.transparent;

			horizontalScrollbar.margin = Offsets.square2;
			horizontalScrollbar.padding = Offsets.square2;

			horizontalScrollbar.imagePosition = ImagePosition.ImageOnly;
			horizontalScrollbar.clipping = TextClipping.Clip;

			horizontalScrollbar.fixedHeight = 19;

			// Horizontal Scrollbar Thumb
			horizontalScrollbarThumb = new GUIStyle();
			horizontalScrollbarThumb.name = "UIFHorizontalScrollbarThumb";

			horizontalScrollbarThumb.normal.background = Palette.tGray30;

			horizontalScrollbarThumb.hover.background = Palette.tGray40;

			horizontalScrollbarThumb.fixedHeight = 15;

			// Horizontal scrollbar left button
			horizontalScrollbarLeftButton = new GUIStyle ();
			horizontalScrollbarLeftButton.name = "UIFHorizontalScrollbarLeftButton";

			// Horizontal scrollbar right button
			horizontalScrollbarRightButton = new GUIStyle ();
			horizontalScrollbarRightButton.name = "UIFHorizontalScrollbarRightButton";

			// Vertical scrollbar
			verticalScrollbar = new GUIStyle();
			verticalScrollbar.name = "UIFVerticalScrollbar";

			verticalScrollbar.normal.background = Palette.tGray10;

			verticalScrollbar.margin = Offsets.square2;
			verticalScrollbar.padding = Offsets.square2;

			verticalScrollbar.clipping = TextClipping.Clip;
			verticalScrollbar.fixedWidth = 19;

			// Vertical scrollbar thumb
			verticalScrollbarThumb = new GUIStyle();
			verticalScrollbarThumb.name = "UIFVerticalScrollbarThumb";

			verticalScrollbarThumb.normal.background = Palette.tGray30;

			verticalScrollbarThumb.hover.background = Palette.tGray40;

			verticalScrollbarThumb.fixedWidth = 15;
			verticalScrollbarThumb.stretchWidth = false;

			// verticalScrollbarUpButton
			verticalScrollbarUpButton = new GUIStyle();
			verticalScrollbarUpButton.name = "UIFVerticalScrollbarUpButton";

			//verticalScrollbarDownButton
			verticalScrollbarDownButton = new GUIStyle ();
			verticalScrollbarDownButton.name = "UIFVerticalScrollbarDownButton";

			// Scrollview style
			scrollView = new GUIStyle(textCommon);
			scrollView.name = "UIFScrollView";

			scrollView.normal.background = Palette.tGray20;

			scrollView.padding = Offsets.square2;

			scrollView.clipping = TextClipping.Clip;

			// Selection grid buttons
			selectionGrid = new GUIStyle (button);
			selectionGrid.name = "UIFSelectionGrid";
			selectionGrid.alignment = TextAnchor.MiddleCenter;
		}

		/// <summary>
		/// Do not use this function in your mod, as it overrides default unity skin
		/// </summary>
		internal static void OverrideUnity() {
			GUI.skin.box = box;
			GUI.skin.label = label;
			GUI.skin.textField = textField;
			GUI.skin.textArea = textArea;
			GUI.skin.button = button;
			GUI.skin.toggle = toggle;
			GUI.skin.window	= window;
			GUI.skin.horizontalSlider = horizontalSlider;
			GUI.skin.horizontalSliderThumb = horizontalSliderThumb;
			GUI.skin.verticalSlider = verticalSlider;
			GUI.skin.verticalSliderThumb = verticalSliderThumb;
			GUI.skin.horizontalScrollbar = horizontalScrollbar;
			GUI.skin.horizontalScrollbarThumb = horizontalScrollbarThumb;
			GUI.skin.horizontalScrollbarLeftButton = horizontalScrollbarLeftButton;
			GUI.skin.horizontalScrollbarRightButton = horizontalScrollbarRightButton;
			GUI.skin.verticalScrollbar = verticalScrollbar;
			GUI.skin.verticalScrollbarThumb = verticalScrollbarThumb;
			GUI.skin.verticalScrollbarUpButton = verticalScrollbarUpButton;
			GUI.skin.verticalScrollbarDownButton = verticalScrollbarDownButton;
			GUI.skin.scrollView = scrollView;

			GUI.skin.font = mainFont;
		}

		/// <summary>
		/// Overrides the KSP skin.
		/// </summary>
		internal static void OverrideKSP() {
			HighLogic.Skin.box = box;
			HighLogic.Skin.label = label;
			HighLogic.Skin.textField = textField;
			HighLogic.Skin.textArea = textArea;
			HighLogic.Skin.button = button;
			HighLogic.Skin.toggle = toggle;
			HighLogic.Skin.window	= window;
			HighLogic.Skin.horizontalSlider = horizontalSlider;
			HighLogic.Skin.horizontalSliderThumb = horizontalSliderThumb;
			HighLogic.Skin.verticalSlider = verticalSlider;
			HighLogic.Skin.verticalSliderThumb = verticalSliderThumb;
			HighLogic.Skin.horizontalScrollbar = horizontalScrollbar;
			HighLogic.Skin.horizontalScrollbarThumb = horizontalScrollbarThumb;
			HighLogic.Skin.horizontalScrollbarLeftButton = horizontalScrollbarLeftButton;
			HighLogic.Skin.horizontalScrollbarRightButton = horizontalScrollbarRightButton;
			HighLogic.Skin.verticalScrollbar = verticalScrollbar;
			HighLogic.Skin.verticalScrollbarThumb = verticalScrollbarThumb;
			HighLogic.Skin.verticalScrollbarUpButton = verticalScrollbarUpButton;
			HighLogic.Skin.verticalScrollbarDownButton = verticalScrollbarDownButton;
			HighLogic.Skin.scrollView = scrollView;

			HighLogic.Skin.font = mainFont;
		}
	}
}

