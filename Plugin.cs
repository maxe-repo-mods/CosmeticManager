using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace CosmeticManager;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "maxenterme.CosmeticManager";
    private const string PluginName = "CosmeticManager";
    private const string PluginVersion = "1.0.0";

    internal static Plugin Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;

    // Spawn settings
    internal static ConfigEntry<int> SpawnChanceMultiplier = null!;
    internal static ConfigEntry<int> ExtraSpawns = null!;
    internal static ConfigEntry<int> CommonWeight = null!;
    internal static ConfigEntry<int> UncommonWeight = null!;
    internal static ConfigEntry<int> RareWeight = null!;
    internal static ConfigEntry<int> UltraRareWeight = null!;

    // Unlock
    internal static ConfigEntry<bool> UnlockAll = null!;

    private void Awake()
    {
        Instance = this;

        SpawnChanceMultiplier = Config.Bind("Spawn", "SpawnChanceMultiplier", 100,
            new ConfigDescription("Multiplier for cosmetic box spawn chance (100 = 100%, 200 = double, 1000 = guaranteed)", new AcceptableValueRange<int>(0, 1000)));
        ExtraSpawns = Config.Bind("Spawn", "ExtraSpawns", 0,
            new ConfigDescription("Extra cosmetic box spawn attempts per level", new AcceptableValueRange<int>(0, 10)));
        CommonWeight = Config.Bind("Spawn", "CommonWeight", 100,
            new ConfigDescription("Weight multiplier for Common cosmetic rarity (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        UncommonWeight = Config.Bind("Spawn", "UncommonWeight", 100,
            new ConfigDescription("Weight multiplier for Uncommon cosmetic rarity (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        RareWeight = Config.Bind("Spawn", "RareWeight", 100,
            new ConfigDescription("Weight multiplier for Rare cosmetic rarity (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        UltraRareWeight = Config.Bind("Spawn", "UltraRareWeight", 100,
            new ConfigDescription("Weight multiplier for UltraRare cosmetic rarity (100 = 100%)", new AcceptableValueRange<int>(0, 500)));

        UnlockAll = Config.Bind("Unlock", "UnlockAll", false,
            "Unlock all cosmetics on game start. Applies to local save only.");

        new Harmony(PluginGuid).PatchAll(typeof(Plugin).Assembly);
        Logger.LogInfo($"{PluginName} v{PluginVersion} loaded!");
    }
}
