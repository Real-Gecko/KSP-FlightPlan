using UnityEngine;
using KSP.IO;

namespace FlightPlan
{
    internal static class Config
    {
        private static PluginConfiguration fromDisk;

        public static Rect winRect = new Rect();
        public static bool showAsUT = false;

        public static void Load()
        {
            fromDisk = PluginConfiguration.CreateForType<FlightPlanAddon>();
            fromDisk.load();
            winRect = fromDisk.GetValue<Rect>("FlightPlan", new Rect(0, 0, Screen.width / 2, Screen.height / 2));
            showAsUT = fromDisk.GetValue<bool>("showAsUT", false);
        }

        public static void Save()
        {
            fromDisk.SetValue("FlightPlan", winRect);
            fromDisk.SetValue("showAsUT", showAsUT);
            fromDisk.save();
        }
    }
}
