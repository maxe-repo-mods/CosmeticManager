# CosmeticManager

Configure cosmetic box spawn rates, rarity weights, and unlock cosmetics in R.E.P.O.

## Features

- Adjust cosmetic box spawn chance multiplier across all levels
- Add extra cosmetic box spawn attempts per level
- Configure weight multipliers for each rarity tier (Common, Uncommon, Rare, UltraRare)
- Unlock all cosmetics on game start
- Cumulative spawn rate adjustments

## Installation

1. Install [BepInEx 5.x](https://github.com/BepInEx/BepInEx/releases) for R.E.P.O.
2. Download the latest `CosmeticManager.dll` from releases
3. Place `CosmeticManager.dll` in `BepInEx/plugins/`
4. Launch the game to generate config file

## Configuration

Configuration file: `BepInEx/config/maxenterme.CosmeticManager.cfg`

| Section | Key | Type | Default | Description |
|---------|-----|------|---------|-------------|
| Spawn | SpawnChanceMultiplier | int | 100 | Multiplier for cosmetic box spawn chance (100 = 100%, 200 = double, 1000 = guaranteed). Range: 0-1000 |
| Spawn | ExtraSpawns | int | 0 | Extra cosmetic box spawn attempts per level. Range: 0-10 |
| Spawn | CommonWeight | int | 100 | Weight multiplier for Common cosmetic rarity (100 = 100%). Range: 0-500 |
| Spawn | UncommonWeight | int | 100 | Weight multiplier for Uncommon cosmetic rarity (100 = 100%). Range: 0-500 |
| Spawn | RareWeight | int | 100 | Weight multiplier for Rare cosmetic rarity (100 = 100%). Range: 0-500 |
| Spawn | UltraRareWeight | int | 100 | Weight multiplier for UltraRare cosmetic rarity (100 = 100%). Range: 0-500 |
| Unlock | UnlockAll | bool | false | Unlock all cosmetics on game start. Applies to local save only. |

## Build

```bash
dotnet build -c Release
```

Output: `bin/Release/netstandard2.1/CosmeticManager.dll`


## AI Disclosure

This mod was developed with the assistance of AI (Claude by Anthropic). All code has been reviewed and tested by the developer.

## License

MIT
