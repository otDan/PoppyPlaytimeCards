using ClassesManagerReborn.Util;
using PoppyPlaytimeCards.Asset;
using PoppyPlaytimeCards.Class;
using PoppyPlaytimeCards.Component;
using PoppyPlaytimeCards.Util;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace PoppyPlaytimeCards.Card
{
    internal class KissyMissyCard : CustomCard
    {
        internal static CardInfo Card = null;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gameObject.GetOrAddComponent<ClassNameMono>().className = PoppyPlaytimeClass.name;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.BlockAction += _ => KissyMissyBlock(player);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.BlockAction -= _ => KissyMissyBlock(player);
        }

        private static void KissyMissyBlock(Player player)
        {
            player.data.healthHandler.Heal(25);
            var kissyMissyEffect = Instantiate(AssetManager.KissyMissyEffect, player.transform);
            kissyMissyEffect.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            kissyMissyEffect.GetOrAddComponent<AnimationAutoDestroy>();
            AudioController.Play(AssetManager.KissyMissySound, kissyMissyEffect.transform);
        }

        protected override string GetTitle()
        {
            return "Kissy Missy";
        }

        protected override string GetDescription()
        {
            return "Heal yourself on block";
        }

        protected override GameObject GetCardArt()
        {
            return AssetManager.KissyMissyCard;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Block Action",
                    amount = "+25hp",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return ThemeManager._poppyRed;
        }

        public override string GetModName()
        {
            return PoppyPlaytimeCards.ModInitials;
        }
    }
}
