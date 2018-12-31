# XLShredLoader

A collection of mods for Skater XL that use the Unity Mod Manager (reworked from the XLShredMenu mod)

### Available Mods
- Menu Mod (including XLShredLib which mods hook into)
- Dynamic Grind Camera
- Auto Slowmo
- Fixed Slowmo
- Faster Spin (in air and/or on grinds)
- Custom Pop Force (from ground)
- Custom Pop Force (grinds and/or manuals)
- Custom Push Speed
- Flip Mods (realistic flip rotation and/or flipped switch flip direction)
- Object Spawner

### Installation

1. Go to the [release page](https://github.com/blendermf/XLShredLoader/releases) and download the latest Unity Mod Manager for Skater XL, the Menu Mod, and which ever of the other mods you want.
2. Unzip Unity Mod Manager wherever is convenient for you (it doesn't matter).
3. Open Unity Mod Manager, make sure Skater XL is selected. At this point it may have detected your installation, if not the SkaterXL button will be red, you can click that and pick the folder manually.
4. Click the Install button. If it works it should show an installed version number. If it doesn't work check the log.
5. Go to the Mods tab, drag all the mod zips you want to install onto the box that says "Drop zip files here". If everything works the status should say ok for each of them.
6. Launch the game, have fun! Press F8 to bring up the menu, and the rest is explained there.

### Useful tips

In the provided Unity Mod Manager there is a config file. If you update the Mod Manager, the config file will be replaced. You will lose the config file that includes Skater XL, so I included a backup file you can copy and rename.

### Building

#### Requirements

- [Visual Studio Community 2017](https://visualstudio.microsoft.com/vs/community/)(free), or any other 2017 version (others may work, but that's all I've tested)
- The .NET desktop development workload for Visual Studio (installed via the Visual Studio Installer app)
- (Optional) git to clone the repo. You can also just download the source as a zip.

Follow the installation steps for installing the Mod Loader (but not any of the mods).

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