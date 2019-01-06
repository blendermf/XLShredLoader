# XLShredLoader

A collection of mods for Skater XL that use the Unity Mod Manager (reworked from the XLShredMenu mod)

### Available Mods
- [Menu Mod v0.0.4](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredMenuMod-0.0.4.zip) (including XLShredLib which mods hook into)
- [Dynamic Grind Camera v0.0.4](https://github.com/blendermf/XLShredLoader/releases/download/dynamic-camera-0.0.4/XLShredDynamicCamera-0.0.4.zip)
- [Auto Slowmo v0.0.3](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredAutoSlowmo-0.0.3.zip)
- [Fixed Slowmo v0.0.3](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredFixedSlowmo-0.0.3.zip)
- [Faster Spin v0.0.3](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredFasterSpin-0.0.3.zip) (in air and/or on grinds)
- [Custom Pop Force v0.0.3](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredPopForce-0.0.3.zip)
- [Custom Grind/Manual Pop Force v0.0.3](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredCustomGrindManualPop-0.0.3.zip)
- [Custom Push Speed v0.0.3](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredPushSpeed-0.0.3.zip)
- [Flip Mods v0.0.4](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredFlipMods-0.0.4.zip) (realistic flip rotation and/or flipped switch flip direction)
- [Object Spawner v0.0.3](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredObjectSpawner-0.0.3.zip)
- [Realistic Vert v0.0.2](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredRealisticVert-0.0.2.zip) (if at the top of a fully vert ramp, you'll pop up, not away from the ramp)
- [Disable Autocatch v0.0.1](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredDisableAutocatch-0.0.1.zip)

### Other Compatible Mods (not on this git)
- [Replay Editor (Unity Mod Manager Edition) v0.0.3](https://github.com/DanielKIWI/SkaterXL-Modding/releases/tag/XLShredReplayEditor-0.0.3) [(Source)](https://github.com/DanielKIWI/SkaterXL-Modding/tree/XLShredReplayEditor-0.0.3/XLShredMods/XLShredReplayEditor)
- [Adjust Audio Pitch v0.0.1](https://github.com/DanielKIWI/SkaterXL-Modding/releases/tag/XLShredAdjustAudioPitch-0.0.1) [(Source)](https://github.com/DanielKIWI/SkaterXL-Modding/tree/XLShredAdjustAudioPitch-0.0.1/XLShredMods/XLShredAdjustAudioPitch) (pitches audio according to timescale changes like slowmo)

### Changelog

[Click here for all the changes for all the mods](https://github.com/blendermf/XLShredLoader/blob/master/CHANGELOG.md)

### Installation

#### Mod Manager Not Installed:

*IMPORTANT NOTE:* If you have previously installed a modified Assembly-CSharp.dll, restore it to its orignal state before installing Unity Mod Manager.

1. Download the latest [Unity Mod Manager for Skater XL](https://github.com/blendermf/XLShredLoader/releases/download/0.0.2/UnityModManagerSkaterXL.zip), the [Menu Mod](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredMenuMod-0.0.4.zip), and which ever of the [other mods you want](https://github.com/blendermf/XLShredLoader#available-mods).
2. Unzip Unity Mod Manager wherever is convenient for you (it doesn't matter).
3. Open Unity Mod Manager, make sure Skater XL is selected. At this point it may have detected your installation, if not the SkaterXL button will be red, you can click that and pick the folder manually.
4. Click the Install button. If it works it should show an installed version number. If it doesn't work check the log.
5. Go to the Mods tab, drag all the mod zips you want to install onto the box that says "Drop zip files here". If everything works the status should say ok for each of them.
6. Launch the game, have fun! Press F8 to bring up the menu, and the rest is explained there.

#### Mod Manager Already Installed:

1.  Download the [Menu Mod](https://github.com/blendermf/XLShredLoader/releases/download/menu-mod-0.0.4/XLShredMenuMod-0.0.4.zip), and which ever of the [other mods you want](https://github.com/blendermf/XLShredLoader#available-mods).
2. Open Unity Mod Manager, go to the Mods tab, drag all the mod zips you want to install onto the box that says "Drop zip files here". If everything works the status should say ok for each of them.
3. Launch the game, have fun! Press F8 to bring up the menu, and the rest is explained there.

### Useful tips

In the provided Unity Mod Manager there is a config file. If you update the Mod Manager, the config file will be replaced. You will lose the config file that includes Skater XL, so I included a backup file you can copy and rename.

### Building

#### Requirements

- [Visual Studio Community 2017](https://visualstudio.microsoft.com/vs/community/)(free), or any other 2017 version (others may work, but that's all I've tested)
- The .NET desktop development workload for Visual Studio (installed via the Visual Studio Installer app)
- (Optional) git to clone the repo. You can also just download the source as a zip.

Follow the installation steps for installing the Mod Loader.

Clone the repo:

```
git clone https://github.com/blendermf/XLShredLoader.git
```

Open the solution (.sln) file.

Under 'Solution Items' in the Solution Explorer, modify config.json to have the correct path for your Skater XL installation (notice the double slashes, those are needed). For example:

```json
{
  "game_directory": "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Skater XL"
}
```

You will now see a bunch of errors (probably). They should go away once you build the solution (the build events use the directory we put in config.json to copy the references into the references folder)

Once they are built you should see folders under `game_directory/Mods` for each mod. You can now run the game and you should see the mods.

Check out the source for the different mods, to see how you can make your own mod that hooks into mod menu.

You can use this solution and duplicate a project as a starting point, and then just build your mod (by building the project not the solution), just make sure you build the XLShredMenuMod first so you have the proper requirements installed in the Mods folder.