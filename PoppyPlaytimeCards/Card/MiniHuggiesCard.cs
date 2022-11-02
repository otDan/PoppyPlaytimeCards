using System;
using System.Collections.Generic;
using System.Linq;
using ClassesManagerReborn.Util;
using ModdingUtils.AIMinion;
using ModdingUtils.Extensions;
using PoppyPlaytimeCards.Asset;
using PoppyPlaytimeCards.Card.Base;
using PoppyPlaytimeCards.Class;
using PoppyPlaytimeCards.Util;
using UnboundLib;
using UnityEngine;

namespace PoppyPlaytimeCards.Card
{
    internal class MiniHuggiesCard : MinionBaseCard
    {
        internal static CardInfo Card = null;

        public override void OnSetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gameObject.GetOrAddComponent<ClassNameMono>().className = PoppyPlaytimeClass.name;
        }

        public override void OnOnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override GameObject GetCardArt()
        {
            return AssetManager.MiniHuggiesCard;
        }

        protected override string GetDescription()
        {
            return "Spawn huggies that chase others all over the map";
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return ThemeManager._poppyRed;
        }

        protected override string GetTitle()
        {
            return "Mini Huggies";
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Minions",
                    amount = "+3",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        public override string GetModName()
        {
            return PoppyPlaytimeCards.ModInitials;
        }

        public override Color GetDetailColor(Player player)
        {
            return new Color(1, 1, 1, 0.65f);
            // return new Color(UnityEngine.Random.Range(0,2), UnityEngine.Random.Range(0, 2), UnityEngine.Random.Range(0, 2), 1f);
        }

        public override AIMinionHandler.SpawnLocation GetAiSpawnLocation(Player player)
        {
            return AIMinionHandler.SpawnLocation.Random;
        }
        public override AIMinionHandler.AISkill GetAiSkill(Player player)
        {
            return AIMinionHandler.AISkill.Normal;
        }
        public override float? GetMaxHealth(Player player)
        {
            return 35f;
        }
        public override int GetNumberOfMinions(Player player)
        {
            return 3;
        }
        
        public override CharacterStatModifiersModifier GetCharacterStats(Player player)
        {
            CharacterStatModifiersModifier charStats = new CharacterStatModifiersModifier
            {
                movementSpeed_mult = 1.35f
            };

            return charStats;
        }

        public override GunAmmoStatModifier GetGunAmmoStats(Player player)
        {
            GunAmmoStatModifier gunAmmoStats = new GunAmmoStatModifier
            {
                maxAmmo_add = -2,
                reloadTimeMultiplier_mult = 1f / 3f
            };
            return gunAmmoStats;
        }

        public override GunStatModifier GetGunStats(Player player)
        {
            GunStatModifier gunStats = new GunStatModifier
            {
                damage_mult = 0.35f
            };
            return gunStats;
        }
        
        public override List<Type> GetEffects(Player player)
        {
            return new List<Type>
            {
                // typeof(AntSquishEffect)
            };
        }

        public override List<CardInfo> GetCards(Player player)
        {
            var floatCards = new List<string> { "chase", "chase", "lifestealer" };

            return ModdingUtils.Utils.Cards.all.Where(card => floatCards.Contains(card.cardName.ToLower())).ToList();
        }
    }

    // public class AntSquishEffect : MonoBehaviour
    // {
    //     private Player _playerToModify;
    //     private CharacterStatModifiers _charStatsToModify;
    //
    //     private void Awake()
    //     {
    //
    //     }
    //
    //     private void Start()
    //     {
    //         _playerToModify = gameObject.GetComponent<Player>();
    //         _charStatsToModify = gameObject.GetComponent<CharacterStatModifiers>();
    //     }
    //
    //     private void Update()
    //     {
    //         // if the Ant player hasn't been squished in the minimum amount of time, and is currently alive, check for squish
    //         if (!PlayerStatus.PlayerAliveAndSimulated(_playerToModify) ||
    //             !(Time.time >= _timeOfLastSquish + _minTimeBetweenSquishes)) return;
    //         var enemyPlayers = PlayerManager.instance.players.Where(player => PlayerStatus.PlayerAliveAndSimulated(player) && (player.teamID != _playerToModify.teamID)).ToList();
    //
    //         foreach (Player enemyPlayer in enemyPlayers)
    //         {
    //             // get the displacement vector from the Ant player to the enemy player, only if the enemy is at least 1.1x the mass of the Ant player
    //             float mass = (float)Traverse.Create(_playerToModify.data.playerVel).Field("mass").GetValue();
    //             float enemyMass = (float)Traverse.Create(enemyPlayer.data.playerVel).Field("mass").GetValue();
    //
    //             if (!(enemyMass >= _minMassFactor * mass)) continue;
    //             Vector2 displacement = enemyPlayer.transform.position - _playerToModify.transform.position;
    //             if (!(displacement.magnitude <= _range) ||
    //                 !(Vector2.Angle(Vector2.up, displacement) <= Math.Abs(_angleThreshold / 2))) continue;
    //             // if the enemy player is both within range and within the specified angle above the player, then squish the Ant player
    //             //float damage = this.damagePerc * (enemy_mass / mass) * this.playerToModify.data.maxHealth;
    //             float damage = _playerToModify.data.maxHealth * 2f; // instakill player
    //
    //             _playerToModify.data.healthHandler.TakeDamage(new Vector2(0, damage * -1), _playerToModify.transform.position, Color.red, null, enemyPlayer, true, false);
    //             // reset the time since last squish and return
    //             ResetTimer();
    //             return;
    //         }
    //     }
    //
    //     public void OnDestroy()
    //     {
    //     }
    //
    //     public void ResetTimer()
    //     {
    //         _timeOfLastSquish = Time.time;
    //     }
    //
    //     public void Destroy()
    //     {
    //         Destroy(this);
    //     }
    //
    //     public void SetDamagePercentage(float percentage)
    //     {
    //         _damagePercentage = percentage;
    //     }
    //
    //     public void IncreaseDamagePercentage(float inc)
    //     {
    //         _damagePercentage += inc;
    //         _damagePercentage = Math.Min(_damagePercentage, 0.75f);
    //     }
    //
    //     private float
    //       _damagePercentage = 0.0f,
    //       _timeOfLastSquish = -1f;
    //     private readonly float 
    //         _range = 1.5f,
    //         _angleThreshold = 30f,
    //         _minTimeBetweenSquishes = 0.5f,
    //         _minMassFactor = 1.1f;
    // }
}