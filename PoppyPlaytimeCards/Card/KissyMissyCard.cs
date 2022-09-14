using ClassesManagerReborn.Util;
using PoppyPlaytimeCards.Asset;
using PoppyPlaytimeCards.Class;
using PoppyPlaytimeCards.Component;
using PoppyPlaytimeCards.Component.Mono;
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
            var jumpScareMono = PoppyPlaytimeCards.Instance.gameObject.GetOrAddComponent<JumpScareMono>();
            jumpScareMono.AddJumpScare(JumpScareMono.JumpScare.KissyMissy);

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
            return "Hitting a player will trigger a jumpscare..." +
                   "\nBlocking will heal you 25hp";
        }

        protected override GameObject GetCardArt()
        {
            return AssetManager.KissyMissyCard;
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
