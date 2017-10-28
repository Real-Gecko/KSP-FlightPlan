# KSP-FlightPlan
Renders active vessel's flight plan.
This mod uses KACWrapper.cs by [TriggerAu](https://github.com/TriggerAu)
This mod uses ToolbarWrapper.cs by [Maik Schreiber](https://github.com/blizzy78)

## Changelog
### Version 1.0.4
- Switched to UICore for GUI drawing
- KAC alarm will be set -10 minutes before plan entry
- Screen message will apperar after alarm creation
- Fixed showAsUT config parameter not being read correctly
- Periapsis is shown on "Encounter" entry type
- Made doubles formatting a little bit saner

### Version 1.0.3
- Recompile for KSP 1.3.1

### Version 1.0.2
- Recompile for KSP 1.3.0

### Version 1.0.1
- Fixed NRE raised when Tracking Station is level 1, message will appear advicing to to upgrade it.
- Predict potential crash after entering celestial body atmosphere
- "Eat Snacks" entry type
- "Diving Course" entry type
- Plan entries time may be shown as universal and as countdown
- Use KSP dateTimeFormatter for date/time formatting
- KAC integration
- Updated included UIFramework to 0.2.1
