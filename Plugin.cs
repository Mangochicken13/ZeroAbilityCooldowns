using BepInEx;
using BoplFixedMath;

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

            //TODO: Work on making this a slider or at least some option in the settings

            Logger.LogInfo($"Plugin \"ZeroAbilityCooldowns\" is loaded!");
        }

        private Fix Ability_InstantCooldown(On.Ability.orig_GetCooldown orig, Ability self)
        {
            return Fix.Zero;
        }

        private Fix InstantAbility_InstantCooldown(On.InstantAbility.orig_GetCooldown orig, InstantAbility self)
        {
            return self.IsRope ? orig.Invoke(self) : Fix.One / (Fix)6;
            // Smoke is so fast that it absorbs all other input, a little bit of delay is ideal
            // Unfortunately i don't know how to differentiate between smoke and everything else
        }
    }
}
