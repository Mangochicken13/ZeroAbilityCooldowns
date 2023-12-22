using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;

namespace ZeroAbilityCooldowns
{
    [BepInPlugin(ModID, ModName, Version)]
    [BepInProcess("BoplBattle.exe")]
    [BepInIncompatibility("com.shadow_dev.BoplPanel")]
    public class Plugin : BaseUnityPlugin
    {
        const string ModID = "com.Mangochicken.ZeroAbilityCooldowns";
        public const string ModName = "Zero Ability Cooldowns";
        public const string Version = "1.2.0";

        public static bool EnableAchievements { get; private set; }

        private void Awake()
        {
            EnableAchievements = false;

            Harmony harmony = new Harmony(ModID);

            MethodInfo orig;
            MethodInfo patch;

            orig = AccessTools.Method(typeof(AchievementHandler), nameof(AchievementHandler.TryAwardAchievement));
            patch = AccessTools.Method(typeof(Patches.NoAchievements), "Prefix");
            harmony.Patch(orig, new HarmonyMethod(patch));

            orig = AccessTools.Method(typeof(Ability), nameof(Ability.GetCooldown));
            patch = AccessTools.Method(typeof(Patches.Ability_Zero), "Postfix");
            harmony.Patch(orig, null, new HarmonyMethod(patch));

            orig = AccessTools.Method(typeof(InstantAbility), nameof(InstantAbility.GetCooldown));
            patch = AccessTools.Method(typeof(Patches.InstantAbility_Zero), "Postfix");
            harmony.Patch(orig, null, new HarmonyMethod(patch));

            Logger.LogInfo($"Plugin {ModName} is loaded!");
        }
    }

    public class Patches
    {
        [HarmonyPatch(typeof(Ability))]
        [HarmonyPatch("GetCooldown")]
        public class Ability_Zero
        {
            static void Postfix(ref Fix __result)
            {
                __result = Fix.Zero;
            }
        }

        [HarmonyPatch(typeof(InstantAbility))]
        [HarmonyPatch("GetCooldown")]
        public class InstantAbility_Zero
        {
            static void Postfix(InstantAbility __instance, ref Fix __result)
            {
                if (__instance.name == "Smoke(Clone)")
                {
                    __result = Fix.One / (Fix)4;
                    return;
                }

                __result = Fix.Zero;
            }

        }

        [HarmonyPatch(typeof(AchievementHandler), nameof(AchievementHandler.TryAwardAchievement))]
        public class NoAchievements
        {
            static bool Prefix()
            {
                return Plugin.EnableAchievements;
            }

        }
    }
}
