namespace FlightPlan
{
    internal enum EntryType
    {
        Encounter,
        Escape,
        Maneuver,
        Crater,
        Burn,
        SnacksLanded,
        SnacksOrbit,
        DivingCourse,
        None
    }

    internal struct PlanEntry
    {
        internal double UT;
        internal MapObject obj;
        internal EntryType type;
        internal double magnitude;
        internal PlanEntry(double UT, MapObject obj, EntryType type = EntryType.None, double magnitude = 0)
        {
            this.UT = UT;
            this.obj = obj;
            this.type = type;
            this.magnitude = magnitude;
        }
        internal string Message
        {
            get
            {
                string inTime = Config.showAsUT ?
                            $"At {KSPUtil.dateTimeFormatter.PrintDateCompact(UT, true, true)}" :
                            $"In {KSPUtil.dateTimeFormatter.PrintDateDeltaCompact(UT - Planetarium.GetUniversalTime(), true, true, true)}";

                switch (type)
                {
                    case EntryType.Encounter:
                        return $"{inTime} encounter {obj.celestialBody.RevealName()} ({Format.Number(magnitude, "m")})";
                    case EntryType.Escape:
                        return $"{inTime} escape to {obj.celestialBody.RevealName()}";
                    case EntryType.Maneuver:
                        return $"{inTime} perform maneuver {Format.Number(magnitude, "m/s")}";
                    //totalDeltaV += magnitude;
                    case EntryType.Burn:
                        return $"{inTime} burn in {obj.celestialBody.RevealName()}'s atmosphere";
                    case EntryType.Crater:
                        return $"{inTime} make a new crater on {obj.celestialBody.RevealName()}";
                    case EntryType.SnacksLanded:
                        return $"{inTime} eat snacks landed at {obj.celestialBody.RevealName()}";
                    case EntryType.DivingCourse:
                        return $"{inTime} diving course in {obj.celestialBody.RevealName()}'s waters";
                    case EntryType.SnacksOrbit:
                        return $"{inTime} eat snacks orbiting {obj.celestialBody.RevealName()}";
                    default:
                        return "";
                }
            }
        }
    }

    internal static class Globals
    {
        internal static string VesselName
        {
            get
            {
                return FlightGlobals.ActiveVessel?.RevealName() ?? "OOOPS!";
            }
        }

        internal static string SOIName
        {
            get
            {
                return FlightGlobals.ActiveVessel?.mainBody?.RevealName() ?? "Void, you're doomed!";
            }
        }

        internal static string TargetName
        {
            get
            {
                return FlightGlobals.ActiveVessel?.targetObject?.GetName() ?? "None, your life is meaningless";
            }
        }
    }
}
