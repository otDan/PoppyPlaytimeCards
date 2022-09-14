using ClassesManagerReborn.Util;
using PoppyPlaytimeCards.Asset;
using PoppyPlaytimeCards.Class;
using PoppyPlaytimeCards.Component.Mono;
using PoppyPlaytimeCards.Util;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Networking;
using UnityEngine;

namespace PoppyPlaytimeCards.Card
{
    internal class MommyLongLegsCard : CustomCard
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
            jumpScareMono.AddJumpScare(JumpScareMono.JumpScare.MommyLongLegs);
            block.BlockAction += _ => MommyLongLegsBlock(player);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.BlockAction -= _ => MommyLongLegsBlock(player);
        }

        private static void MommyLongLegsBlock(Player player)
        {
            if (!player.data.view.IsMine) return;
            if (Camera.main == null) return;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SendCobweb(player.playerID, mousePosition);
        }

        public static void SendCobweb(int player, Vector2 mousePosition)
        {
            NetworkingManager.RPC(typeof(MommyLongLegsCard), nameof(SyncCobweb), player, mousePosition);
        }

        [UnboundRPC]
        private static void SyncCobweb(int playerId, Vector2 mousePosition)
        {
            var mommyLongLegsEffect = Instantiate(AssetManager.MommyLongLegsEffect, mousePosition, Quaternion.identity);
            mommyLongLegsEffect.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            var cobwebMono = mommyLongLegsEffect.AddComponent<CobwebMono>();
            Player player = PlayerManager.instance.players[playerId];
            cobwebMono.player = player;
            PoppyPlaytimeCards.cobwebs.Add(mommyLongLegsEffect);
        }

        protected override string GetTitle()
        {
            return "Mommy Long Legs";
        }

        protected override string GetDescription()
        {
            return "Hitting a player will trigger a jumpscare..." +
                   "\nBlocking will spawn a web trap on the cursor";
        }

        protected override GameObject GetCardArt()
        {
            return AssetManager.MommyLongLegsCard;
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
