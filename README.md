![GitHub Releases](https://img.shields.io/github/downloads/Hanbin-GW/LobbyMusic/LobbyMusic.dll)

#LobbyMusic (Free Version)

A **lobby background music plugin** for **SCP: Secret Laboratory**, allowing you to play custom songs while players wait in the lobby.

> [!WARNING]
> ⚠️ **Note:** This plugin requires **CedMod’s [SCPSLAudioApi](https://github.com/CedModV2/SCPSLAudioApi/releases/tag/0.0.8)** to function properly.

---

## Installation

1. Download and install **[SCPSLAudioApi](https://github.com/CedModV2/SCPSLAudioApi/releases/tag/0.0.8)**.
2. Place your **`.ogg`** audio files inside the following directory:

   ```
   C:\Users\<YourUserName>\AppData\Roaming\EXILED\Plugins\audio
   ```
3. Configure the plugin settings in your **config file**:

    * Enable `LoopSingleSong` to play a single song infinitely.
    * Set the song name in `SingleSongName`.
    * If you disable `LoopSingleSong`, you can create a song queue using `QueueSongs`.

---

## Audio Requirements

* The music file **must be mono**.
* The audio **must be 48,000 Hz**.
* You can easily convert your `.mp3` files to `.ogg` (mono + 48,000 Hz) using this tool:
   [**Music .ogg Mono Converter (requires ffmpeg)**](https://github.com/Hanbin-GW/Music-.ogg-mono-Converter/releases)

---

## Tech Support
 
**Made by [Ghost Server](https://discord.gg/aYyNucAfqE)**

---

## Future Notice

This plugin will receive a **major rework** in the future.

> The reworked version will be **paid ($1)**.

---
