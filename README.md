## Overview

This plugin is designed to enhance gameplay by providing a set of powerful features that give you an edge in multiplayer matches. Whether you're participating in HvH scenarios or just looking to optimize your gameplay, this plugin delivers the tools you need to stay ahead.

**Note:** This plugin is currently in beta. All issues will be addressed in the full release, and functionality will be expanded.

### Keybinds (Temporary)

- Z - FakeLag (not working right now)
- E - Fly Up
- Q - Fly Down
- X - RageAim + TriggerBot
- C - LagFly

### Features

- **RageAim + TriggerBot:** Automatically targets and fires at the nearest enemy, making it ideal for HvH situations where precision and speed are critical.

- **ESP (Extrasensory Perception):** Displays boxes around enemies and teammates, showing their names and health status, providing enhanced awareness of your surroundings.

- **LagFly:** Enables client-side flight, particularly useful in HvH. This feature allows you to pass through walls and eliminate opponents, provided you remain within 4 blocks of your original position. When moving slowly from the activation point, it relocates you on the server side.

- **FastReload/AlwaysPerfectReload:** Automatically enabled to allow for rapid reloading, keeping you in the fight without delay.

### Current Limitations

- **RageAim + TriggerBot** can occasionally misfire or aim inaccurately (although this happens rarely). It also does not display the current ammo count, so you'll need to reload manually.
- Occasionally, the plugin may incorrectly identify dead enemies as alive due to plugin's creator stupidity.
- In Infection mode, teammates may sometimes be mistakenly identified as enemies by RageAim and ESP.
- New players joining the game after you may not be detected by the plugin.
- Currently, there is no GUI or advanced customization options available.
- The plugin was developed quickly, similar to the game itself.

## Installation

Install latest Bepinex IL2Cpp 6.0.0 x86 build from this site: https://builds.bepinex.dev/projects/bepinex_be
You can read instruction how to install but I'll make short guide:

- Download build I mentioned before.
- Insert all from zip archive into root game's folder.
- Put BepInEx config file into BepInEx folder by it path BepInEx/config/BepInEx.cfg (you may need to create /config/ folder, config for BepInEx are inside project)
- Launch game to init BepInEx.
- Insert .dll plugin into BepInEx/plugins/
