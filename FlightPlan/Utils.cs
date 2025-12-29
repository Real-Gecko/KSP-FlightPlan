using System;
namespace FlightPlan
{
    public static class Utils
    {
        internal static void FocusOn(ManeuverNode node)
        {
            FocusOn(node.scaledSpaceTarget);
        }

        internal static void FocusOn(ITargetable target)
        {
            MapObject mapObject = null;
            if (target is Vessel vessel)
            {
                mapObject = vessel.mapObject;
            }
            else if (target is CelestialBody body)
            {
                mapObject = body.MapObject;
            }
            FocusOn(mapObject);
        }

        internal static void FocusOn(MapObject mapObject)
        {
            if (mapObject == null) return;
            if (HighLogic.LoadedSceneIsFlight)
                MapView.EnterMapView();
            PlanetariumCamera.fetch.SetTarget(mapObject);
        }
    }
}
