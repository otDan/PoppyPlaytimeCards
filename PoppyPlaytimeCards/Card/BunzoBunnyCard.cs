using ClassesManagerReborn.Util;
using PoppyPlaytimeCards.Asset;
using PoppyPlaytimeCards.Class;
using PoppyPlaytimeCards.Component.Mono;
using PoppyPlaytimeCards.Util;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace PoppyPlaytimeCards.Card
{
    internal class BunzoBunnyCard : CustomCard
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
            jumpScareMono.AddJumpScare(JumpScareMono.JumpScare.BunzoBunny);

            var bunzoBunnyMono = player.gameObject.GetOrAddComponent<BunzoBunnyMono>();
            bunzoBunnyMono.Player = player;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var bunzoBunnyMono = player.gameObject.GetComponent<BunzoBunnyMono>();
            if (bunzoBunnyMono != null) Destroy(bunzoBunnyMono);
        }

        protected override string GetTitle()
        {
            return "Bunzo Bunny";
        }

        protected override string GetDescription()
        {
            return "Hitting a player will trigger a jumpscare..." + 
                   "\nReloading will cause a radiance attack";
        }

        protected override GameObject GetCardArt()
        {
            return AssetManager.BunzoBunnyCard;
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
