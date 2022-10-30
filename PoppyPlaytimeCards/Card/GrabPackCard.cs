using ClassesManagerReborn.Util;
using PoppyPlaytimeCards.Asset;
using PoppyPlaytimeCards.Util;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace PoppyPlaytimeCards.Card
{
    internal class GrabPackCard : CustomCard
    {
        internal static CardInfo Card = null;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gameObject.GetOrAddComponent<ClassNameMono>();
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle()
        {
            return "Grab Pack";
        }

        protected override string GetDescription()
        {
            return "Unlock the poppy playtime experience...";
        }

        protected override GameObject GetCardArt()
        {
            return AssetManager.GrabPackCard;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[] { };
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return ThemeManager.GetRedTheme();
        }

        public override string GetModName()
        {
            return PoppyPlaytimeCards.ModInitials;
        }
    }
}
