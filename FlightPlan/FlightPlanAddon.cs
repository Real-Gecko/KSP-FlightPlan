using System;
using UnityEngine;
using System.Collections.Generic;
using KSP.IO;
using KSP.UI.Screens;
using FlightPlan_KACWrapper;
using System.Linq;

namespace FlightPlan
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FlightPlanAddon : MonoBehaviour
    {

        static private FlightPlanAddon instance;

        private bool available = false;
        private bool active = false;
        private bool hidden = false;
        private int winId;
        private bool refresh = true;

        private readonly string msgUpgrade = "Upgrade Tracking Station to access Flight Plan!";

        private readonly List<PlanEntry> flightPlan = new List<PlanEntry>();
        private DateTime lastTime;

        private ApplicationLauncherButton appLauncherButton;
        private IButton toolbarButton;
        private MainWindow mainWindow;

        public void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
            Config.Load();
        }

        private void Start()
        {
            available = PSystemSetup.Instance.GetSpaceCenterFacility("TrackingStation").GetFacilityLevel() > 0;
            lastTime = DateTime.Now;

            winId = GUIUtility.GetControlID(FocusType.Passive);

            GameEvents.onGUIApplicationLauncherReady.Add(CreateLauncher);
            GameEvents.onHideUI.Add(OnHide);
            GameEvents.onShowUI.Add(OnUnHide);
            GameEvents.onGamePause.Add(OnHide);
            GameEvents.onGameUnpause.Add(OnUnHide);

            KACWrapper.InitKACWrapper();
            if (KACWrapper.APIReady)
                KACWrapper.KAC.onAlarmStateChanged += KACWrapper_KAC_onAlarmStateChanged;
            mainWindow = new MainWindow();
        }

        private void OnDestroy()
        {
            Config.Save();
            UnlockControls();

            DestroyLauncher();

            GameEvents.onGUIApplicationLauncherReady.Remove(CreateLauncher);
            GameEvents.onHideUI.Remove(OnHide);
            GameEvents.onShowUI.Remove(OnUnHide);
            GameEvents.onGamePause.Remove(OnHide);
            GameEvents.onGameUnpause.Remove(OnUnHide);

            if (KACWrapper.APIReady)
                KACWrapper.KAC.onAlarmStateChanged -= KACWrapper_KAC_onAlarmStateChanged;

            if (instance == this)
                instance = null;
        }

        void KACWrapper_KAC_onAlarmStateChanged(KACWrapper.KACAPI.AlarmStateChangedEventArgs e)
        {

        }

        private void CreateLauncher()
        {
            if (ToolbarManager.ToolbarAvailable)
            {
                toolbarButton = ToolbarManager.Instance.add("FlightPlan", "AppLaunch");
                toolbarButton.TexturePath = "FlightPlan/Textures/flight-plan-icon-toolbar";
                toolbarButton.ToolTip = "Flight Plan Window";
                toolbarButton.Visible = true;
                toolbarButton.OnClick += (ClickEvent e) =>
                {
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
            if (appLauncherButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(appLauncherButton);
            }
            if (toolbarButton != null)
            {
                toolbarButton.Destroy();
                toolbarButton = null;
            }
        }


        public void onAppTrue()
        {
            if (!available)
                ScreenMessages.PostScreenMessage(msgUpgrade);
            else
                active = true;
            mainWindow.Show();
        }

        public void onAppFalse()
        {
            active = false;
            mainWindow.Dismiss();
            refresh = true;
            UnlockControls();
        }

        private void OnHide()
        {
            hidden = true;
            UnlockControls();
        }

        private void OnUnHide()
        {
            hidden = false;
        }

        internal virtual void onToggle()
        {
            if (!available)
            {
                ScreenMessages.PostScreenMessage(msgUpgrade);
                return;
            }

            active = !active;
            if (active) mainWindow.Show();
            else mainWindow.Dismiss();
            if (!active)
            {
                refresh = true;
                UnlockControls();
            }
        }

        private void Update()
        {
            // Refresh once per second to reduce garbage generation
            if (lastTime.AddSeconds(1) > DateTime.Now || !active || hidden)
                return;

            if (FlightGlobals.ActiveVessel != null)
            {
                BuildFlightPlan(FlightGlobals.ActiveVessel);
                refresh = true;
                lastTime = DateTime.Now;
                mainWindow.UpdateFlightPlan(flightPlan);
            }
        }

        private void OnGUI()
        {
            return;
            if (active && !hidden)
            {
                if (refresh)
                {
                    Config.winRect.height = 0;
                    Config.winRect.width = 0;
                    refresh = false;
                }

                Config.winRect = GUILayout.Window(winId, Config.winRect, RenderWindow, "Flight Plan");

                if (Config.winRect.Contains(Event.current.mousePosition))
                {
                    LockControls();
                }
                else
                {
                    UnlockControls();
                }
            }
        }

        private ControlTypes LockControls()
        {
            return InputLockManager.SetControlLock(ControlTypes.ALLBUTTARGETING, this.name);
        }

        private void UnlockControls()
        {
            InputLockManager.RemoveControlLock(this.name);
        }

        private void RenderWindow(int windowId)
        {
            Vessel vessel = FlightGlobals.ActiveVessel;
            GUILayout.BeginVertical(GUILayout.MinWidth(400));

            if (vessel != null)
            {
                GUILayout.Label("Vessel:");
                TargetFocusButton(vessel);

                GUILayout.Label("Current SOI:");
                TargetFocusButton(vessel.mainBody);

                if (vessel.targetObject != null)
                {
                    GUILayout.Label("Target:");
                    TargetFocusButton(vessel.targetObject);
                }

                GUILayout.Label("Plan entries:");
                RenderFlightPlan();
            }

            GUILayout.Label("", GUILayout.Height(5));
            Config.showAsUT = GUILayout.Toggle(Config.showAsUT, "Universal Time");
            GUILayout.Label("", GUILayout.Height(5));

            if (GUILayout.Button("Close"))
            {
                if (appLauncherButton != null)
                    appLauncherButton.SetFalse();
                else
                    onToggle();
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void TargetFocusButton(ITargetable ob)
        {
            MapObject mo = null;
            if (ob is Vessel v)
            {
                mo = v.mapObject;
            }
            else if (ob is CelestialBody b)
            {
                mo = b.MapObject;
            }

            if (mo != null)
            {
                if (GUILayout.Button(mo.Discoverable.RevealName()))
                {
                    if (HighLogic.LoadedSceneIsFlight)
                        MapView.EnterMapView();
                    PlanetariumCamera.fetch.SetTarget(mo);
                }
            }
        }

        private void BuildFlightPlan(Vessel vessel)
        {
            flightPlan.Clear();

            // Eat snacks while landed
            if (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.PRELAUNCH)
            {
                flightPlan.Add(new PlanEntry(0, vessel.mainBody.MapObject, EntryType.SnacksLanded));
                return;
            }

            // Diving course while splashed
            if (vessel.situation == Vessel.Situations.SPLASHED)
            {
                flightPlan.Add(new PlanEntry(0, vessel.mainBody.MapObject, EntryType.DivingCourse));
                return;
            }

            int maneuverCount = vessel.patchedConicSolver.maneuverNodes.Count;

            Orbit orb = vessel.orbit;
            List<Orbit> plan;

            double ooopsUT = 0;

            if (maneuverCount > 0)
                plan = vessel.patchedConicSolver.flightPlan;
            else
            {
                plan = new List<Orbit>();
                while (orb != null)
                {
                    plan.Add(orb);
                    orb = orb.nextPatch;
                }
            }


            // Filter out nearby patches with the same reference body appeared after maneuver added
            CelestialBody prev = vessel.mainBody;
            foreach (Orbit patch in plan)
            {
                // Trust me, I'm a programmer
                if (patch.PeR == 0) continue;

                if (patch.referenceBody != prev)
                {
                    if (patch.referenceBody == prev.referenceBody)
                        flightPlan.Add(new PlanEntry(patch.StartUT, patch.referenceBody.MapObject, EntryType.Escape));
                    else
                        flightPlan.Add(new PlanEntry(patch.StartUT, patch.referenceBody.MapObject, EntryType.Encounter, patch.PeA));

                    prev = patch.referenceBody;
                }

                // Assuming that burn in atmosphere is 60 seconds earlier than periapsis
                // For entries order, but not for correct prediction 
                if (patch.PeA <= patch.referenceBody.atmosphereDepth && patch.referenceBody.atmosphere)
                {
                    flightPlan.Add(new PlanEntry(patch.StartUT + patch.GetTimeToPeriapsis() - 60, patch.referenceBody.MapObject, EntryType.Burn));
                }

                if (patch.PeA <= 0)
                {
                    ooopsUT = patch.StartUT + patch.GetTimeToPeriapsis();
                    flightPlan.Add(new PlanEntry(ooopsUT, patch.referenceBody.MapObject, EntryType.Crater));
                    break;
                }
            }

            // Push maneuvers into flight plan
            if (maneuverCount > 0)
            {
                foreach (ManeuverNode man in vessel.patchedConicSolver.maneuverNodes)
                {
                    if (ooopsUT > 0 && man.UT > ooopsUT)
                        continue;

                    flightPlan.Add(new PlanEntry(man.UT, man.scaledSpaceTarget, EntryType.Maneuver, man.DeltaV.magnitude));
                }
            }

            // Sort by time
            flightPlan.Sort((s1, s2) => s1.UT.CompareTo(s2.UT));

            // Nothing to do? Eat snacks!!!
            if (flightPlan.Count == 0)
                flightPlan.Add(new PlanEntry(0, vessel.mainBody.MapObject, EntryType.SnacksOrbit));
        }

        private void RenderFlightPlan()
        {
            double totalDeltaV = 0;
            string text = "";
            foreach (PlanEntry entry in flightPlan)
            {
                switch (entry.type)
                {
                    case EntryType.Encounter:
                        text = " encounter " + entry.obj.celestialBody.RevealName() + " (" + Format.Number(entry.magnitude, "m") + ")";
                        break;
                    case EntryType.Escape:
                        text = " escape to " + entry.obj.celestialBody.RevealName();
                        break;
                    case EntryType.Maneuver:
                        text = " perform maneuver ";
                        text += Format.Number(entry.magnitude, "m/s");
                        totalDeltaV += entry.magnitude;
                        break;
                    case EntryType.Burn:
                        text = " burn in " + entry.obj.celestialBody.RevealName() + "'s atmosphere";
                        break;
                    case EntryType.Crater:
                        text = " make a new crater on " + entry.obj.celestialBody.RevealName();
                        break;
                    case EntryType.SnacksLanded:
                        text = " eat snacks landed at " + entry.obj.celestialBody.RevealName();
                        break;
                    case EntryType.DivingCourse:
                        text = " diving course in " + entry.obj.celestialBody.RevealName() + "'s ocean";
                        break;
                    case EntryType.SnacksOrbit:
                        text = " eat snacks orbiting " + entry.obj.celestialBody.RevealName();
                        break;
                }
                GUILayout.BeginHorizontal();

                if (entry.UT > 0)
                {
                    GUILayout.Label(
                        (Config.showAsUT ?
                            "At " + KSPUtil.dateTimeFormatter.PrintDateCompact(entry.UT, true, true) :
                            "In " + KSPUtil.dateTimeFormatter.PrintDateDeltaCompact(
                                entry.UT - Planetarium.GetUniversalTime(),
                                true,
                                true,
                                true
                            )
                        ),
                        GUILayout.Width(150)
                    );
                }

                if (GUILayout.Button(text, GUILayout.MinWidth(230)))
                {
                    if (!MapView.MapIsEnabled)
                        MapView.EnterMapView();
                    if (entry.obj != null)
                        PlanetariumCamera.fetch.SetTarget(entry.obj);
                }

                if (KACWrapper.APIReady)
                {
                    KACButton(entry, text);
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.Label("", GUILayout.Height(10));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Total Δv: ");
            GUILayout.Label(Format.Number(totalDeltaV, "m/s"));
            GUILayout.EndHorizontal();
        }

        private void KACButton(PlanEntry entry, string text)
        {
            KACWrapper.KACAPI.AlarmTypeEnum alarmType;
            switch (entry.type)
            {
                case EntryType.Encounter:
                    alarmType = KACWrapper.KACAPI.AlarmTypeEnum.SOIChange;
                    break;
                case EntryType.Escape:
                    alarmType = KACWrapper.KACAPI.AlarmTypeEnum.SOIChange;
                    break;
                case EntryType.Maneuver:
                    alarmType = KACWrapper.KACAPI.AlarmTypeEnum.Maneuver;
                    break;
                case EntryType.Burn:
                    alarmType = KACWrapper.KACAPI.AlarmTypeEnum.Closest;
                    break;
                case EntryType.Crater:
                    alarmType = KACWrapper.KACAPI.AlarmTypeEnum.Periapsis;
                    break;
                default:
                    alarmType = KACWrapper.KACAPI.AlarmTypeEnum.Raw;
                    break;
            }

            if (GUILayout.Button("A", GUILayout.Width(20)))
            {
                String tmpID = KACWrapper.KAC.CreateAlarm(
                    alarmType,
                    FlightGlobals.ActiveVessel.vesselName,
                    entry.UT - 600
                );

                KACWrapper.KACAPI.KACAlarm alarmNew = KACWrapper.KAC.Alarms.First(a => a.ID == tmpID);
                alarmNew.Notes = FlightGlobals.ActiveVessel.vesselName + "\n" +
                    "Is about to " + text;
                alarmNew.AlarmMargin = 600;
                alarmNew.AlarmAction = KACWrapper.KACAPI.AlarmActionEnum.KillWarp;
                alarmNew.VesselID = FlightGlobals.ActiveVessel.id.ToString();
                ScreenMessages.PostScreenMessage("KAC alarm added for " + FlightGlobals.ActiveVessel.vesselName);
            }
        }
    }
}

