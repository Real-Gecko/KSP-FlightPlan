using System;
using UnityEngine;
using System.Collections.Generic;
using KSP.IO;
using KSP.UI.Screens;

namespace FlightPlan
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class FlightPlan: MonoBehaviour
	{
		private enum EntryType {
			Encounter,
			Escape,
			Maneuver,
			Crater,
			Burn,
			None
		}

		private struct PlanEntry {
			internal double UT;
			internal MapObject obj;
			internal EntryType type;
			internal double magnitude;
			internal PlanEntry(double UT, MapObject obj, EntryType type = EntryType.None, double magnitude = 0) {
				this.UT = UT;
				this.obj = obj;
				this.type = type;
				this.magnitude = magnitude;
			}
		}

		static private FlightPlan instance;

		private bool	active = false;
		private bool	hidden = false;
		private Rect	winRect = new Rect();
		private int		winId;
		protected bool	refresh = true;

		private PluginConfiguration config;

		private List<PlanEntry> flightPlan = new List<PlanEntry>();
		private DateTime lastTime;

		private ApplicationLauncherButton appLauncherButton;
		private IButton toolbarButton;

		public void Awake() {
			if (instance != null) {
				Destroy (this);
				return;
			}
			instance = this;
		}

		private void Start() {
			lastTime = DateTime.Now;
			config = PluginConfiguration.CreateForType<FlightPlan> ();
			config.load ();

			winId = GUIUtility.GetControlID (FocusType.Passive);
			winRect = config.GetValue<Rect> (this.name, new Rect());

			if (winRect.x == 0 && winRect.y == 0) {
				winRect.center = new Vector2 (Screen.width / 2, Screen.height / 2);
			}

			GameEvents.onGUIApplicationLauncherReady.Add(onLauncherReady);
			GameEvents.onHideUI.Add (OnHide);
			GameEvents.onShowUI.Add (OnUnHide);
			GameEvents.onGamePause.Add (OnHide);
			GameEvents.onGameUnpause.Add (OnUnHide);
		}

		private void OnDestroy()
		{
			config.SetValue (this.name, winRect);
			config.save ();

			UnlockControls ();

			DestroyLauncher ();

			GameEvents.onGUIApplicationLauncherReady.Remove(onLauncherReady);
			GameEvents.onHideUI.Remove (OnHide);
			GameEvents.onShowUI.Remove (OnUnHide);
			GameEvents.onGamePause.Remove (OnHide);
			GameEvents.onGameUnpause.Remove (OnUnHide);

			if (instance == this)
				instance = null;
		}

		public void onLauncherReady()
		{
			CreateLauncher ();
		}

		private void CreateLauncher() {
			if (ToolbarManager.ToolbarAvailable) {
				toolbarButton = ToolbarManager.Instance.add ("FlightPlan", "AppLaunch");
				toolbarButton.TexturePath = "FlightPlan/Textures/flight-plan-icon-toolbar";
				toolbarButton.ToolTip = "Flight Plan Window";
				toolbarButton.Visible = true;
				toolbarButton.OnClick += (ClickEvent e) => {
					onToggle();
				};
			}
			else if (appLauncherButton == null)
			{
				appLauncherButton = ApplicationLauncher.Instance.AddModApplication(
					onAppTrue,
					onAppFalse,
					null,
					null,
					null,
					null,
					ApplicationLauncher.AppScenes.SPACECENTER |
					ApplicationLauncher.AppScenes.TRACKSTATION |
					ApplicationLauncher.AppScenes.FLIGHT |
					ApplicationLauncher.AppScenes.MAPVIEW,
					GameDatabase.Instance.GetTexture("FlightPlan/Textures/flight-plan-icon", false)
				);
			}
		}

		public void DestroyLauncher()
		{
			if (appLauncherButton != null) {
				ApplicationLauncher.Instance.RemoveModApplication (appLauncherButton);
			}
			if (toolbarButton != null) {
				toolbarButton.Destroy ();
				toolbarButton = null;
			}
		}


		public void onAppTrue()
		{
			active = true;
		}

		public void onAppFalse()
		{
			active = false;
			refresh = true;
			UnlockControls ();
		}

		private void OnHide() {
			hidden = true;
			UnlockControls ();
		}

		private void OnUnHide() {
			hidden = false;
		}

		internal virtual void onToggle()
		{
			active = !active;
			if (!active) {
				refresh = true;
				UnlockControls ();
			}
		}

		private void Update() {
			// Refresh once per second to reduce garbage generation
			if (lastTime.AddSeconds (1) > DateTime.Now)
				return;
			if (FlightGlobals.ActiveVessel != null) {
				BuildFlightPlan (FlightGlobals.ActiveVessel);
				refresh = true;
			}
		}

		private void OnGUI() {
			if (active && !hidden)
			{
				if (refresh) {
					winRect.height = 0;
					winRect.width = 0;
					refresh = false;
				}

				winRect = Layout.Window (winId, winRect, RenderWindow, "Flight Plan");

				if (winRect.Contains (Event.current.mousePosition)) {
					LockControls ();
				} else {
					UnlockControls();
				}
			}
		}

		private ControlTypes LockControls()
		{
			return InputLockManager.SetControlLock (ControlTypes.ALLBUTTARGETING, this.name);
		}

		private void UnlockControls()
		{
			InputLockManager.RemoveControlLock(this.name);
		}

		private void RenderWindow(int windowId)
		{
			Vessel vessel = FlightGlobals.ActiveVessel;
			GUILayout.BeginVertical (GUILayout.MinWidth(400));

			if (FlightGlobals.ActiveVessel != null) {
				Layout.Label ("Vessel:");
				TargetFocusButton (vessel);

				Layout.Label ("Current SOI:");
				TargetFocusButton (vessel.mainBody);

				if (vessel.targetObject != null) {
					Layout.Label ("Target:");
					TargetFocusButton (vessel.targetObject);
				}

				Layout.Label ("Plan entries:", Palette.blue);
				RenderFlightPlan ();
			}

			Layout.HR (10);

			if (Layout.Button ("Close", Palette.green)) {
				if (appLauncherButton != null)
					appLauncherButton.SetFalse ();
				else
					onToggle ();
			}
			GUILayout.EndVertical ();
			GUI.DragWindow ();
		}

		private void TargetFocusButton(ITargetable ob)
		{
			MapObject mo = null;
			if (ob is Vessel) {
				Vessel v = (Vessel)ob;
				mo = v.mapObject;
			} else if (ob is CelestialBody) {
				CelestialBody b = (CelestialBody)ob;
				mo = b.MapObject;
			}

			if (mo != null) {
				if (Layout.Button (mo.Discoverable.RevealName (), Palette.green)) {
					if (HighLogic.LoadedSceneIsFlight)
						MapView.EnterMapView();						
					PlanetariumCamera.fetch.SetTarget (mo);
				}
			}
		}

		private void BuildFlightPlan(Vessel vessel) {
			lastTime = DateTime.Now;

			flightPlan.Clear ();

			int maneuverCount = vessel.patchedConicSolver.maneuverNodes.Count;

			Orbit orb = vessel.orbit;
			List<Orbit> plan;

			double ooopsUT = 0;

			if (maneuverCount > 0)
				plan = vessel.patchedConicSolver.flightPlan;
			else {
				plan = new List<Orbit> ();
				while (orb != null) {
					plan.Add (orb);
					orb = orb.nextPatch;
				}
			}


			// Filter out nearby patches with the same reference body appeared after maneuver added
			CelestialBody prev = vessel.mainBody;
			foreach (Orbit patch in plan) {
				// Trust me, I'm a programmer
				if(patch.PeR == 0) continue;

				if (patch.referenceBody != prev) {
					if (patch.referenceBody == prev.referenceBody)
						flightPlan.Add(new PlanEntry(patch.StartUT, patch.referenceBody.MapObject, EntryType.Escape));
					else
						flightPlan.Add(new PlanEntry(patch.StartUT, patch.referenceBody.MapObject, EntryType.Encounter));

					prev = patch.referenceBody;
				}

				if (patch.PeA <= patch.referenceBody.atmosphereDepth && patch.referenceBody.atmosphere) {
					flightPlan.Add (new PlanEntry (patch.StartUT + patch.GetTimeToPeriapsis (), patch.referenceBody.MapObject, EntryType.Burn));
				} else if (patch.PeA <= 0) {
					ooopsUT = patch.StartUT + patch.GetTimeToPeriapsis ();
					flightPlan.Add (new PlanEntry (ooopsUT, patch.referenceBody.MapObject, EntryType.Crater));
					break;
				}
			}

			// Push maneuvers into flight plan
			if (maneuverCount > 0) {
				foreach (ManeuverNode man in vessel.patchedConicSolver.maneuverNodes) {
					if (ooopsUT > 0 && man.UT > ooopsUT)
						continue;

					flightPlan.Add (new PlanEntry (man.UT, man.scaledSpaceTarget, EntryType.Maneuver, man.DeltaV.magnitude));
				}
			}

			// Sort by time
			flightPlan.Sort ((s1, s2) => s1.UT.CompareTo (s2.UT));
		}

		private void RenderFlightPlan() {
			double UT = Planetarium.GetUniversalTime ();
			double totalDeltaV = 0;
			string text = "";
			Color color = Palette.gray;
			foreach (PlanEntry entry in flightPlan) {
				switch (entry.type) {
				case EntryType.Encounter:
					text = " encounter " + entry.obj.celestialBody.theName;
					color = Palette.green;
					break;
				case EntryType.Escape:
					text = " escape to " + entry.obj.celestialBody.theName;
					color = Color.white;
					break;
				case EntryType.Maneuver:
					text = " maneuver ";
					text += Format.Number (entry.magnitude, "m/s");
					totalDeltaV += entry.magnitude;
					color = Palette.yellow;
					break;
				case EntryType.Burn:
					text = " burn in " + entry.obj.celestialBody.theName + "'s atmosphere";
					color = Palette.red;
					break;
				case EntryType.Crater:
					text = " make a new crater on " + entry.obj.celestialBody.theName;
					color = Palette.red;
					break;
				}
//				text += " in " + Format.Time (entry.UT - UT);
				GUILayout.BeginHorizontal ();
				Layout.LabelRight ("In " + Format.Time (entry.UT - UT), color, GUILayout.Width (150));
				if (Layout.ButtonLeft (text, color, GUILayout.MinWidth(250))) {
					if (!MapView.MapIsEnabled)
						MapView.EnterMapView ();
					if (entry.obj != null)
						PlanetariumCamera.fetch.SetTarget (entry.obj);
				}
				GUILayout.EndHorizontal ();
			}
			Layout.HR (10);
			Layout.LabelAndText ("Total Δv", Format.Number(totalDeltaV, "m/s"));
		}
	}
}

