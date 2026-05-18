using HarmonyLib;
using UnityEngine;

namespace CosmeticManager.Patches;

[HarmonyPatch]
public static class CosmeticSpawnPatch
{
    private static int? _originalLoopsMax;
    private static Keyframe[]? _originalSpawnCurveKeys;
    private static Keyframe[][]? _originalRarityCurveKeys;

    [HarmonyPatch(typeof(ValuableDirector), nameof(ValuableDirector.SetupHost))]
    [HarmonyPrefix]
    private static void SetupHost_Prefix(ValuableDirector __instance)
    {
        // Save originals on first call
        if (_originalLoopsMax == null)
        {
            _originalLoopsMax = __instance.cosmeticWorldObjectsLevelLoopsMax;
            _originalSpawnCurveKeys = __instance.cosmeticWorldObjectsSpawnCurve.keys;
            var setups = __instance.cosmeticWorldObjectSetups;
            _originalRarityCurveKeys = new Keyframe[setups.Count][];
            for (int i = 0; i < setups.Count; i++)
                _originalRarityCurveKeys[i] = setups[i].chanceCurve.keys;
        }
        else
        {
            __instance.cosmeticWorldObjectsLevelLoopsMax = _originalLoopsMax.Value;
        }

        // Extra spawn attempts
        if (Plugin.ExtraSpawns.Value > 0)
        {
            __instance.cosmeticWorldObjectsLevelLoopsMax += Plugin.ExtraSpawns.Value;
            Plugin.Logger.LogInfo($"CosmeticManager: loopsMax {_originalLoopsMax} -> {__instance.cosmeticWorldObjectsLevelLoopsMax}");
        }

        // Spawn chance multiplier
        if (Plugin.SpawnChanceMultiplier.Value != 100 && _originalSpawnCurveKeys != null)
        {
            float mult = Plugin.SpawnChanceMultiplier.Value / 100f;
            var scaledKeys = new Keyframe[_originalSpawnCurveKeys.Length];
            for (int i = 0; i < _originalSpawnCurveKeys.Length; i++)
            {
                scaledKeys[i] = _originalSpawnCurveKeys[i];
                scaledKeys[i].value *= mult;
            }
            __instance.cosmeticWorldObjectsSpawnCurve = new AnimationCurve(scaledKeys);
        }
        else if (_originalSpawnCurveKeys != null)
        {
            __instance.cosmeticWorldObjectsSpawnCurve = new AnimationCurve(_originalSpawnCurveKeys);
        }

        // Per-rarity weights
        if (_originalRarityCurveKeys != null)
        {
            var weights = new[]
            {
                Plugin.CommonWeight.Value,
                Plugin.UncommonWeight.Value,
                Plugin.RareWeight.Value,
                Plugin.UltraRareWeight.Value,
            };
            var setups = __instance.cosmeticWorldObjectSetups;
            for (int i = 0; i < setups.Count && i < _originalRarityCurveKeys.Length && i < weights.Length; i++)
            {
                if (weights[i] == 100)
                {
                    setups[i].chanceCurve = new AnimationCurve(_originalRarityCurveKeys[i]);
                    continue;
                }
                float mult = weights[i] / 100f;
                var origKeys = _originalRarityCurveKeys[i];
                var scaledKeys = new Keyframe[origKeys.Length];
                for (int k = 0; k < origKeys.Length; k++)
                {
                    scaledKeys[k] = origKeys[k];
                    scaledKeys[k].value *= mult;
                }
                setups[i].chanceCurve = new AnimationCurve(scaledKeys);
            }
        }
    }

    /// <summary>
    /// Override extraction limit so increased spawns can be collected.
    /// </summary>
    [HarmonyPatch(typeof(ValuableDirector), nameof(ValuableDirector.CosmeticWorldObjectLevelLoopsClampedGet))]
    [HarmonyPostfix]
    private static void ExtractionLimit_Postfix(ref int __result)
    {
        if (Plugin.ExtraSpawns.Value > 0)
        {
            int loopsMax = ValuableDirector.instance.cosmeticWorldObjectsLevelLoopsMax;
            if (__result < loopsMax)
                __result = loopsMax;
        }
    }
}
