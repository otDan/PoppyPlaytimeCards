using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace PoppyPlaytimeCards.Card.Base.Extension
{
    public class CharacterStatModifiersAdditionalData
    {
        public Dictionary<(int,int), int> minionIDstoCardIndxMap;
        public Dictionary<(int, int), int> oldMinionIDstoCardIndxMap;

        public CharacterStatModifiersAdditionalData()
        {
            minionIDstoCardIndxMap = new Dictionary<(int, int), int>();
            oldMinionIDstoCardIndxMap = new Dictionary<(int, int), int>();
        }
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> Data = new();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers characterstats)
        {
            return Data.GetOrCreateValue(characterstats);
        }

        public static void AddData(this CharacterStatModifiers characterstats, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                Data.Add(characterstats, value);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        // reset additional CharacterStatModifiers when ResetStats is called
        [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
        private class CharacterStatModifiersPatchResetStats
        {
            private static void Prefix(CharacterStatModifiers __instance)
            {
                __instance.GetAdditionalData().oldMinionIDstoCardIndxMap = new Dictionary<(int, int), int>(__instance.GetAdditionalData().minionIDstoCardIndxMap);
                __instance.GetAdditionalData().minionIDstoCardIndxMap = new Dictionary<(int, int), int>();
            }
        }

    }
}
