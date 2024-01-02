# CS2-AntiTeamFlash

Simple Anti Team Flash plugin.

### Requirements
[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/)

### Installation

Drag and drop from [releases](https://github.com/Franc1sco/CS2-AntiTeamFlash/releases) to game/csgo/addons/counterstrikesharp/plugins

### Configuration

Configure the file AntiTeamFlash.json generated on addons/counterstrikesharp/configs/plugins/AntiTeamFlash
```json
{
  "Enabled": true,
  "FlashOwner": true,
  "AccessFlag": "",
  "ConfigVersion": 1
}
```
* Enable - Enable or disable the plugin.
* FlashOwner - true: Whether to blind the thrower of the flash.
* AccessFlag - access required for get access to this plugin, leave blank "" for public access.
