using BepInEx;
using BoplFixedMath;
using System;

namespace ZeroAbilityCooldowns
{
    //[BepInPlugin(GUID, Name, Version)]
    [BepInPlugin("Mangochicken.ZeroAbilityCooldowns", "ZeroAbilityCooldowns", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            On.Ability.GetCooldown += Ability_InstantCooldown;
            On.InstantAbility.GetCooldown += InstantAbility_InstantCooldown;

            Logger.LogInfo($"Plugin \"ZeroAbilityCooldowns\" is loaded!");
        }

        private Fix Ability_InstantCooldown(On.Ability.orig_GetCooldown orig, Ability self)
        {
            return Fix.Zero;
        }

        private Fix InstantAbility_InstantCooldown(On.InstantAbility.orig_GetCooldown orig, InstantAbility self)
        {
            return (self.name == "Smoke(Clone)") ? (Fix)1 / (Fix)4 : Fix.Zero;
        }
    }
}
