# LobbyMusic Plugin for SCP: Secret Laboratory

>[!WARNING]
> ⚠️ **Note:** This plugin has been **merged into [GhostPlugin](https://github.com/Hanbin-GW/GhostPlugin)** and is no longer maintained as a standalone project.
> All LobbyMusic features are now included and actively developed within **GhostPlugin**.

---

## Overview

The **LobbyMusic Plugin** was designed to enhance the **SCP: Secret Laboratory** lobby experience by playing background music before rounds start.
It allowed server administrators to control, customize, and play music tracks dynamically for all connected players.

Now, all of its functionality has been integrated into **GhostPlugin**, providing improved stability, extended features, and centralized control across Ghost World server plugins.

---

## Key Features (Legacy)

* **Lobby Background Music** – Plays music tracks while players wait in the lobby.
* **Music Controls** – Start, stop, and change songs with in-game commands.
* **Volume Management** – Adjust playback volume dynamically via `/setvolume`.
* **Song Lists** – Supports multiple tracks and random playback.
* **Custom Configuration** – Configure song paths and playback settings in `Config.cs`.

---

## Technical Details

| Category        | Details                                                 |
| --------------- | ------------------------------------------------------- |
| **Language**    | C#                                                      |
| **Framework**   | Exiled API                                              |
| **Game**        | SCP: Secret Laboratory                                  |
| **Merged Into** | [GhostPlugin](https://github.com/Hanbin-GW/GhostPlugin) |

---

## Installation (Legacy)

1. Build the project using **Visual Studio**.
2. Copy the generated `LobbyMusic.dll` to the **Exiled/Plugins** directory.
3. Edit configuration options in your server’s config file.
4. Restart the SCP:SL server.

---

## Project Status

* **Merged into:** [GhostPlugin](https://github.com/Hanbin-GW/GhostPlugin)
* **Maintenance:** Discontinued as standalone
* **Last Active Version:** Before GhostPlugin integration
