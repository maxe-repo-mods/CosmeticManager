using HarmonyLib;

namespace CosmeticManager.Patches;

[HarmonyPatch]
public static class CosmeticUnlockPatch
{
    private static bool _unlocked;

    /// <summary>
    /// Unlock all cosmetics when MetaManager initializes (once per game launch).
    /// Only acts if UnlockAll config is enabled.
    /// </summary>
    [HarmonyPatch(typeof(MetaManager), "Start")]
    [HarmonyPostfix]
    private static void MetaManager_Start_Postfix(MetaManager __instance)
    {
        if (!Plugin.UnlockAll.Value) return;
        if (_unlocked) return;

        _unlocked = true;
        __instance.CosmeticUnlockAll();
        Plugin.Logger.LogInfo("CosmeticManager: all cosmetics unlocked.");
    }
}
