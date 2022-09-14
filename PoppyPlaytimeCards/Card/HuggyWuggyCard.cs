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
    internal class HuggyWuggyCard : CustomCard
    {
        internal static CardInfo Card = null;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.movementSpeed = 2.5f;

            cardInfo.allowMultiple = false;
            gameObject.GetOrAddComponent<ClassNameMono>().className = PoppyPlaytimeClass.name;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var jumpScareMono = PoppyPlaytimeCards.Instance.gameObject.GetOrAddComponent<JumpScareMono>();
            jumpScareMono.AddJumpScare(JumpScareMono.JumpScare.HuggyWuggy);

            var tallPlayerEffect = player.gameObject.transform.GetChild(0).gameObject.GetOrAddComponent<TallPlayerEffect>();
            tallPlayerEffect.ratio *= 0.35f;
            tallPlayerEffect.yOffset += 0.35f;
            tallPlayerEffect.ResetScale();
            tallPlayerEffect.MakeTall();
            player.gameObject.transform.GetChild(3).gameObject.GetComponentInChildren<LegRaycasters>().force *= 2f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle()
        {
            return "Huggy Wuggy";
        }

        protected override string GetDescription()
        {
            return "Hitting a player will trigger a jumpscare..." +
                   "\nBecome tall and skinny";
        }

        protected override GameObject GetCardArt()
        {
            return AssetManager.HuggyWuggyCard;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Move Speed",
                    amount = "+125%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return ThemeManager.GetRedTheme();
        }

        public override string GetModName()
        {
            return PoppyPlaytimeCards.ModInitials;
        }

        internal class TallPlayerEffect : MonoBehaviour
		{
			private void Start()
			{
				orig_scale = gameObject.transform.localScale;
				foreach (var obj in gameObject.transform.parent.GetChild(4))
				{
					((Transform)obj).localPosition += new Vector3(0f, yOffset, 0f);
				}
				MakeTall();
				ResetTimer();
			}
			
			private void Update()
            {
                if (!(Time.time >= _startTime + 0.5f)) return;
                ResetTimer();
                if (gameObject.transform.localScale.x != gameObject.transform.localScale.y) return;
                orig_scale = gameObject.transform.localScale;
                MakeTall();
            }
            
			internal void MakeTall()
            {
                if (!(Mathf.Abs(gameObject.transform.localScale.x / gameObject.transform.localScale.y -
                                ratio) >= 0.0001f)) return;
                var playerObject = gameObject;
                var localScale = playerObject.transform.localScale;
                localScale = 2.35f * new Vector3(localScale.x * ratio, localScale.x, localScale.z);
                playerObject.transform.localScale = localScale;
                playerObject.transform.localPosition = new Vector3(0f, yOffset, 0f);
            }
            
			internal void ResetScale()
			{
				if (orig_scale != Vector3.zero)
				{
					gameObject.transform.localScale = orig_scale;
				}
			}
            
			private void ResetTimer()
			{
				_startTime = Time.time;
			}
            
			private void OnDestroy()
			{
				ResetScale();
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				foreach (var obj in gameObject.transform.parent.GetChild(4))
				{
					((Transform)obj).localPosition -= new Vector3(0f, yOffset, 0f);
				}
			}
            
            private const float FlatScale = 1.25f;
			private const float UpdateDelay = 0.5f;
            private float _startTime = -1f;
			internal float ratio = 1f;
            internal float yOffset;
            internal Vector3 orig_scale = Vector3.zero;
		}
    }
}
