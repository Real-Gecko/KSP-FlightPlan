﻿/*
 * UI Framework licensed under BSD 3-clause license
 * https://github.com/Real-Gecko/Unity5-UIFramework/blob/master/LICENSE.md
*/

using System;
using UnityEngine;

namespace FlightPlan
{
	[KSPAddon(KSPAddon.Startup.Instantly,true)]
	public class Core: MonoBehaviour
	{
		static private Core _instance = null;
		static public Core Instance
		{
			get { return _instance; }
		}

		private bool skinInitialized = false;

		public void Awake() {
			if (_instance != null) {
				Destroy (this);
				return;
			}
			_instance = this;
		}

		public void Start() {
			DontDestroyOnLoad (this);
		}

		public void OnDestroy() {
			if (_instance == this)
				_instance = null;
		}

		public void OnGUI() {
			if (skinInitialized)
				return;
			Palette.InitPalette ();
			Styles.InitStyles ();
			skinInitialized = true;
			Destroy (this); // Quit after initialized
		}
	}
}

