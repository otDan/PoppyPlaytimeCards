using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ModdingUtils.AIMinion;
using ModdingUtils.Extensions;
using Photon.Pun;
using PoppyPlaytimeCards.Asset;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnboundLib.Networking;
using UnityEngine;
using CharacterStatModifiersExtension = PoppyPlaytimeCards.Card.Extension.CharacterStatModifiersExtension;

namespace PoppyPlaytimeCards.Card.Base
{
    public abstract class MinionBaseCard : CustomCard
    {
        public static CardCategory minionCategory = CustomCardCategories.instance.CardCategory("AIMinion");
        public static CardCategory[] categories = { 
            minionCategory, 
            CustomCardCategories.instance.CardCategory("NoRemove"), 
            CustomCardCategories.instance.CardCategory("NoRandom")
        };

        internal static bool AIsDoneSpawning = true;

        private const float DelayBetweenSpawns = 0.25f;

        public virtual BlockModifier GetBlockStats(Player player)
        { return null; }

        public virtual GunAmmoStatModifier GetGunAmmoStats(Player player)
        { return null; }

        public virtual GunStatModifier GetGunStats(Player player)
        { return null; }

        public virtual CharacterStatModifiersModifier GetCharacterStats(Player player)
        { return null; }

        public virtual GravityModifier GetGravityModifier(Player player)
        { return null; }

        public virtual float? GetMaxHealth(Player player)
        { return null; }

        public virtual List<CardInfo> GetCards(Player player)
        { return null; }

        public virtual bool CardsAreReassigned(Player player)
        { return false; }

        public virtual AIMinionHandler.SpawnLocation GetAiSpawnLocation(Player player)
        { return AIMinionHandler.SpawnLocation.Owner_Random; }

        public virtual AIMinionHandler.AISkill GetAiSkill(Player player)
        { return AIMinionHandler.AISkill.None; }

        public virtual AIMinionHandler.AIAggression GetAiAggression(Player player)
        { return AIMinionHandler.AIAggression.None; }

        public virtual AIMinionHandler.AI GetAi(Player player)
        { return AIMinionHandler.AI.None; }

        public virtual Color GetDetailColor(Player player)
        {
            return Color.white;
        }

        public virtual int GetNumberOfMinions(Player player)
        {
            return 1;
        }

        public virtual List<Type> GetEffects(Player player)
        { return new List<Type>(); }

        private protected List<CardInfo> GetValidCards(Player player)
        {
            // Certain AI cards can cause infinite recursion (e.g. Mirror & Doppleganger) and so are not valid for AIs to have

            if (GetCards(player) == null || GetCards(player).All(card => card.categories.Contains(minionCategory)))  
                return new List<CardInfo>();
            return GetCards(player).Where(card => !card.categories.Contains(minionCategory)).ToList();
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.categories = categories;
            cardInfo.GetAdditionalData().canBeReassigned = false;
            OnSetupCard(cardInfo, gun, cardStats, statModifiers, block);
        }

        public virtual void OnSetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // AIs can't have AIs
            if (ModdingUtils.AIMinion.Extensions.CharacterDataExtension.GetAdditionalData(player.data).isAIMinion)
            {
                return;
            }
            int idx = player.data.currentCards.Count;

            AIsDoneSpawning = false;

            Unbound.Instance.StartCoroutine(SpawnAIs(idx, GetNumberOfMinions(player), DelayBetweenSpawns, player, gun, gunAmmo, data, health, gravity, block, characterStats));
            Unbound.Instance.ExecuteAfterSeconds(GetNumberOfMinions(player) * DelayBetweenSpawns + 1f, () => { AIsDoneSpawning = true; });
            OnOnAddCard(player, gun, gunAmmo, data, health, gravity, block, characterStats);
        }

        public virtual void OnOnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) { }

        private IEnumerator SpawnAIs(int idx, int n, float delay, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            for (int i = 0; i < n; i++)
            {
                int nextId = (int) typeof(AIMinionHandler).GetProperty("NextAvailablePlayerID", BindingFlags.NonPublic | BindingFlags.Static)
                    ?.GetValue(null, null)!;
                CharacterStatModifiersExtension.GetAdditionalData(characterStats).minionIDstoCardIndxMap[(nextId, player.data.view.ControllerActorNr)] = idx;


                AIMinionHandler.CreateAIWithStats(player.data.view.IsMine, 
                    player.playerID,
                    player.teamID, 
                    player.data.view.ControllerActorNr, 
                    GetAiSkill(player),
                    GetAiAggression(player), 
                    GetAi(player),
                    GetMaxHealth(player), 
                    GetBlockStats(player), 
                    GetGunAmmoStats(player), 
                    GetGunStats(player),
                    GetCharacterStats(player), 
                    GetGravityModifier(player),
                    GetEffects(player), 
                    GetValidCards(player),
                    CardsAreReassigned(player), 
                    GetAiSpawnLocation(player), 
                    0, 
                    new Vector2(0f, -0.5f), 
                    0, 
                    new Vector2(0f, -0.5f), 
                    14, 
                    new Vector2(0f, 1f), 
                    0, 
                    new Vector2(0f, 0f), 
                    AIMinionHandler.sandbox, 
                    Finalizer: (mId, aId) => SetDetailColor(mId, aId, GetDetailColor(player)));
                yield return new WaitForSecondsRealtime(delay);
            }
        }

        private static IEnumerator SetDetailColor(int minionId, int actorId, Color detailColor)
        {
            Player minion = ModdingUtils.Utils.FindPlayer.GetPlayerWithActorAndPlayerIDs(actorId, minionId);
            yield return new WaitUntil(() => minion.GetComponentsInChildren<SpriteRenderer>().Any(renderer => renderer.gameObject.name.Contains("P_A_X6")));
            yield return new WaitForSecondsRealtime(0.15f);
            NetworkingManager.RPC(typeof(MinionBaseCard), nameof(RPCA_SetDetailColor), minion.data.view.ViewID, detailColor.r, detailColor.g, detailColor.b, detailColor.a);
        }

        [UnboundRPC]
        private static void RPCA_SetDetailColor(int viewId, float r, float g, float b, float a)
        {
            GameObject minion = PhotonView.Find(viewId).gameObject;
            var spriteRenderer = minion.GetComponentsInChildren<SpriteRenderer>().First(renderer => renderer.gameObject.name.Contains("P_A_X6"));
            spriteRenderer.sprite = AssetManager.MiniHuggyFace;
            spriteRenderer.transform.localPosition = Vector3.zero;
            spriteRenderer.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            spriteRenderer.color = new Color(r,g,b,a);
        }

        public override void OnRemoveCard() { }

        private static Player GetMinionWithPlayerAndActorId(IEnumerable<Player> minions, int playerId, int actorId)
        {
            return minions.FirstOrDefault(m => m.playerID == playerId && m.data.view.ControllerActorNr == actorId);
        }

        internal static void OnRemoveCallback(Player player, CardInfo card, int indx)
        {
            Unbound.Instance.ExecuteAfterSeconds(0.15f, () =>
            {
                // restore all minions, disable any that were removed
                foreach ((int minionId, int actorId) in CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).oldMinionIDstoCardIndxMap.Keys.ToList())
                {
                    int idx = CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).oldMinionIDstoCardIndxMap[(minionId, actorId)];

                    if (idx == indx)
                    {
                        ModdingUtils.AIMinion.Extensions.CharacterDataExtension.GetAdditionalData(GetMinionWithPlayerAndActorId(ModdingUtils.AIMinion.Extensions.CharacterDataExtension.GetAdditionalData(player.data).oldMinions.ToArray(), minionId, actorId).data).isEnabled = false;
                    }

                    ModdingUtils.AIMinion.Extensions.CharacterDataExtension.GetAdditionalData(player.data).minions.Add(GetMinionWithPlayerAndActorId(ModdingUtils.AIMinion.Extensions.CharacterDataExtension.GetAdditionalData(player.data).oldMinions.ToArray(), minionId, actorId));
                    CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).minionIDstoCardIndxMap[(minionId, actorId)] = idx == indx ? -1 : idx > indx ? idx - 1 : idx;
                }
            });
        }

        public override string GetModName()
        {
            return PoppyPlaytimeCards.ModInitials;
        }
        
        internal static IEnumerator WaitForAIs(IGameModeHandler gm)
        {
            yield return new WaitUntil(() => AIsDoneSpawning);
        }
    }
}
