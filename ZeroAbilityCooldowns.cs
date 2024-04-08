using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;

namespace ZeroAbilityCooldowns
{
    [BepInPlugin(ModID, ModName, Version)]
    [BepInProcess("BoplBattle.exe")]
    [BepInIncompatibility("com.shadow_dev.BoplPanel")]
    public class ZeroAbilityCooldowns : BaseUnityPlugin
    {
        const string ModID = "com.Mangochicken.ZeroAbilityCooldowns";
        public const string ModName = "Zero Ability Cooldowns";
        public const string Version = "1.3.1";

        public static bool EnableAchievements { get; private set; }

        private void Awake()
        {
            EnableAchievements = false;

            Harmony harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), ModID);

            Logger.LogInfo($"Plugin {ModName} is loaded!");
        }
    }

    [HarmonyPatch]
    public class Patches
    {
        [HarmonyPatch(typeof(Ability), nameof(Ability.GetCooldown))]
        [HarmonyPostfix]
        static void AbilityPostfix(ref Fix __result)
        {
            __result = Fix.Zero;
        }

        [HarmonyPatch(typeof(InstantAbility), nameof(InstantAbility.GetCooldown))]
        [HarmonyPostfix]
        static void InstantAbilityPostfix(ref Fix __result)
        {
            __result = Fix.Zero;
        }

        [HarmonyPatch(typeof(AchievementHandler), nameof(AchievementHandler.TryAwardAchievement))]
        [HarmonyPrefix]
        static bool AchievementPrefix()
        {
            return ZeroAbilityCooldowns.EnableAchievements;
        }
    }
}
