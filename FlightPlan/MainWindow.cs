using System.Collections.Generic;
using UnityEngine;
namespace FlightPlan
{
    public class MainWindow : DialogGUIVerticalLayout
    {
        private PopupDialog dialog;
        private DialogGUIVerticalLayout EntryList;

        public MainWindow()
        {
            EntryList = new DialogGUIVerticalLayout();
            AddChildren(new DialogGUIBase[] {
                new DialogGUIHorizontalLayout(
                    new DialogGUILabel("Vessel:",70),
                    new DialogGUILabel(()=>Globals.VesselName, 250),
                    TargetButton(FlightGlobals.ActiveVessel?.mapObject)
                ),
                new DialogGUIHorizontalLayout(
                    new DialogGUILabel("Current SOI:",70),
                    new DialogGUILabel(()=>Globals.SOIName, 250),
                    TargetButton(FlightGlobals.ActiveVessel?.mainBody?.MapObject)
                ),
                new DialogGUIHorizontalLayout(
                    new DialogGUILabel("Target:",70),
                    new DialogGUILabel(()=>Globals.TargetName, 250),
                    TargetButton(FlightGlobals.ActiveVessel?.targetObject)
                ),
                new DialogGUIToggle(Config.showAsUT, "Universal Time", (bool newValue)=>{Config.showAsUT = newValue; }),
                new DialogGUIScrollList(new Vector2(380, 220), false, true, EntryList)
                }
            );
        }

        internal PopupDialog Show()
        {
            if (dialog == null)
            {
                dialog = PopupDialog.SpawnPopupDialog(
                    new MultiOptionDialog(
                        "FlightPlan",
                        "",
                        "Flight Plan",
                        HighLogic.UISkin,
                        390,
                        this
                    ),
                    false,
                    HighLogic.UISkin,
                    false
                );
            }
            return dialog;
        }

        internal void Dismiss()
        {
            dialog?.Dismiss();
            dialog = null;
        }

        internal void UpdateFlightPlan(List<PlanEntry> flightPlan)
        {
            Stack<Transform> stack = new Stack<Transform>();
            stack.Push(EntryList.uiItem.gameObject.transform);
            foreach (var child in EntryList.children)
                child.uiItem.gameObject.DestroyGameObjectImmediate();
            EntryList.children.Clear();
            foreach (PlanEntry entry in flightPlan)
            {
                DialogGUIBase row = new DialogGUIHorizontalLayout(
                    new DialogGUILabel(entry.Message, 325),
                    TargetButton(entry.obj)
                );
                EntryList.AddChild(row);
                row.Create(ref stack, HighLogic.UISkin);
            }
        }

        private DialogGUIButton TargetButton(MapObject mapObject)
        {
            return new DialogGUIButton("->", () => { Utils.FocusOn(mapObject); }, 20, 20, false);
        }

        private DialogGUIButton TargetButton(ITargetable mapObject)
        {
            return new DialogGUIButton("->", () => { Utils.FocusOn(mapObject); }, 20, 20, false);
        }
    }
}
