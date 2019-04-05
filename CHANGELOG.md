# Changelog

### Table of Contents
##### Menu Mod
- [XL Shred Menu Mod](#xl-shred-menu-mod)
##### Mods for the Menu
- [XL Shred Auto Slowmo](#xl-shred-auto-slowmo)
- [XL Shred Custom Grind/Manual Pop](#xl-shred-custom-grindmanual-pop)
- [XL Shred Disable Autocatch](#xl-shred-disable-autocatch)
- [XL Shred Disable Push Reduction](#xl-shred-disable-push-reduction)
- [XL Shred Dynamic Camera](#xl-shred-dynamic-camera)
- [XL Shred Faster Spin](#xl-shred-faster-spin)
- [XL Shred Fixed Slowmo](#xl-shred-fixed-slowmo)
- [XL Shred Flip Mods](#xl-shred-flip-mods)
- [XL Shred Object Spawner](#xl-shred-object-spawner)
- [XL Shred Custom Pop Force](#xl-shred-custom-pop-force)
- [XL Shred Custom Push Speed](#xl-shred-custom-push-speed)
- [XL Shred Realstic Vert](#xl-shred-realistic-vert)
- [XL Shred Respawn Near Bail](#xl-shred-respawn-near-bail)
---
### XL Shred Menu Mod
##### v0.0.5
- Works with and requires latest version of Unity Mod Manager (0.15.0.0)
- Improved mod toggling from Unity Mod Manager (now when you disable from there the UMM actually unpatches the mod). This change is not for XLShredLib, just the menu mod, updates to other mods will eventually include this change.
- Changed folder structure in release to fix Unity Mod Manager uninstall behaviour.
- Some behind the scene changes for mod developers. Deprecated RegisterShowCursor, UnregisterShowCursor, RegisterTempHideMenu and UnregisterTempHideMenu
- Added the ability for mods to remove ui labels and custom elements by an id (and deprecated old functions that don't take an id).
##### v0.0.4
- Bumped the version number, you need to update the other mods to the latest versions if you install this.
- Removed the Replay Mod Menu Compatibility Mod. (integrated in Replay Mod now)
- Added LabelType and Toggles for mods that support it.
- Added some convenience methods in both ModMenu and XLShredLib.UI
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)

---
### Mods for the Menu
#### XL Shred Auto Slowmo
##### v0.0.3
- updated required version of XL Shred Menu Mod to v0.0.4
- changed labels to toggles
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
---
#### XL Shred Custom Grind/Manual Pop
##### v0.0.3
- updated required version of XL Shred Menu Mod to v0.0.4 (under the hood changes)
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
---
#### XL Shred Disable Autocatch
##### v0.0.1
- **RELEASED:** Allows you to disable autocatch
---
#### XL Shred Disable Push Reduction
##### v0.0.1
- **RELEASED:** Allows you to disable the reduction of push force that happens on the first two pushes.
---
#### XL Shred Dynamic Camera
##### v0.0.5
- Fixed bug causing the camera to not switch sides back and forth when going into switch / out of it after grinding something.
- Removed references to it just being dynamic grind camera (a misunderstanding from me I think? I dont remember if an old version called it just a dynamic grind cam). Although before the bug fix that's all it really was.
##### v0.0.4
- Fixed a bug that was stopping the settings from saving.
##### v0.0.3
- fixed a bug that caused the mod to not save whether or not the dynamic camera is on
- updated required version of XL Shred Menu Mod to v0.0.4
- changed labels to toggles
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
---
#### XL Shred Faster Spin
##### v0.0.3
- updated required version of XL Shred Menu Mod to v0.0.4
- changed labels to toggles
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
---
#### XL Shred Fixed Slowmo
##### v0.0.3
- Integrated with the latest version of the Replay Menu (no longer using the compatibility mod)
- updated required version of XL Shred Menu Mod to v0.0.4
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
##### v0.0.2
- Integrated with Replay Mod Menu Compatibility Mod
---
#### XL Shred Flip Mods
##### v0.0.4
- updated required version of XL Shred Menu Mod to v0.0.4
- changed labels to toggles
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
##### v0.0.3
- Fixed issue with player animation not properly flipping with change in flip direction.
---
#### XL Shred Object Spawner
##### v0.0.4
- Added positioning sensitivity setting.
- Added ability to roll objects (now allowing rotation in all axis)
- Reorganized placement positioning buttons.
- Allow editing of placed objects.
##### v0.0.3
- Minor updates to the UI, mostly behind the scenes.
- updated required version of XL Shred Menu Mod to v0.0.4 (needed for some of those changes)
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
---
#### XL Shred Custom Pop Force
##### v0.0.3
- Added alternative +/- minus bindings (num pad, and dedicated +/- keys)
- updated required version of XL Shred Menu Mod to v0.0.4 (under the hood changes)
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
---
#### XL Shred Custom Push Speed
##### v0.0.3
- Set the default push speed to that of the vanilla games.
- updated required version of XL Shred Menu Mod to v0.0.4 (under the hood changes)
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
---
#### XL Shred Realstic Vert
##### v0.0.3
- Completely changes what the mod does. Now it pretty much ensures off of ramps of a certain angle you will jump straight up, not past the ramp, and not backwards.
- Requires UMM 0.15.0.0 and XL Shred Mod Menu 0.0.5
- Fixes issue preventing flip tricks to manny in latest version of the game
##### v0.0.2
- updated required version of XL Shred Menu Mod to v0.0.4 (under the hood changes)
- Added repository link so the mod can auto check for updates (and potentially install them for you not sure)
##### v0.0.1
- **RELEASED:** if at the top of a fully vert ramp, you'll pop up, not away from the ramp
---
#### XL Shred Respawn Near Bail
##### v0.0.2
- Fixed problem where if you manually went back to a marker while bailing you would reset twice, once for the manual reset and then once for the reset from the bail.
##### v0.0.1
- **RELEASED:** Respawns you near your bail, so you don't have to set markers all the time.