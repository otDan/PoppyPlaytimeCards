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
            var webSize = AssetManager.MommyLongLegsEffect.transform.localScale;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var randomPosition = new Vector2(Random.Range(-webSize.x * 2, webSize.x * 2), Random.Range(-webSize.y * 2, webSize.y * 2));
            var spawnPosition = mousePosition + randomPosition;
            SendCobweb(player.playerID, spawnPosition);
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
            PoppyPlaytimeCards.Cobwebs.Add(mommyLongLegsEffect);
        }

        protected override string GetTitle()
        {
            return "Mommy Long Legs";
        }

        protected override string GetDescription()
        {
            return "Spawn a web trap close to your cursor";
        }

        protected override GameObject GetCardArt()
        {
            return AssetManager.MommyLongLegsCard;
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
                    amount = "Spawn Cobweb",
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
