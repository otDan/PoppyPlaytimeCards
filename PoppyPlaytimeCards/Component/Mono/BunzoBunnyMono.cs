using System.Collections.Generic;
using System.Linq;
using PoppyPlaytimeCards.Asset;
using PoppyPlaytimeCards.Util;
using UnboundLib;
using UnityEngine;
using UnityEngine.Events;

namespace PoppyPlaytimeCards.Component.Mono
{
    internal class BunzoBunnyMono : MonoBehaviour
    {
        public Player Player;
        private readonly List<LineEffect> _lineEffects = new();
        private const float Damage = 25f;
        private int _times;

        private void Start()
        {
            var duringReloadTrigger = Player.gameObject.GetOrAddComponent<DuringReloadTrigger>();
            duringReloadTrigger.triggerStartEvent = new UnityEvent();
            duringReloadTrigger.triggerEndEvent = new UnityEvent();
            var triggerEvent = duringReloadTrigger.triggerEvent = new UnityEvent();
            triggerEvent.AddListener(() =>
            {
                if (_times == 0)
                {
                    var bunzoBunnyEffect = Instantiate(AssetManager.BunzoBunnyEffect, Player.gameObject.transform.position, Quaternion.identity);
                    var lineEffect = bunzoBunnyEffect.GetComponent<LineEffect>();
                    AudioController.Play(AssetManager.BunzoBunnySound, bunzoBunnyEffect.transform);
                    var removeAfterSeconds = bunzoBunnyEffect.GetOrAddComponent<RemoveAfterSeconds>();
                    removeAfterSeconds.seconds = 5;
                    _lineEffects.Add(lineEffect);
                }

                if (_times >= 35)
                {
                    _times = 0;
                }
                else
                {
                    _times++;
                }
            });
        }

        private void Update()
        {
            if (!Player.data.view.IsMine) return;
            foreach (var lineEffect in new List<LineEffect>(_lineEffects))
            {
                if (lineEffect.counter >= 1)
                {
                    _lineEffects.Remove(lineEffect);
                    Destroy(lineEffect.gameObject);
                    continue;
                }

                var currentRadius = lineEffect.radiusOverTime.Evaluate(lineEffect.counter);
                var effectTransform = lineEffect.transform;
                float radius = (lineEffect.radius + currentRadius) * effectTransform.root.localScale.x * effectTransform.localScale.x;//lineEffect.radius * lineEffect.transform.root.localScale.x * radiusOverTime;
                var enemiesInRange = PlayerManager.instance.players.Where(player => 
                        player.teamID != Player.teamID && 
                        !player.data.dead && 
                        Vector3.Distance(lineEffect.transform.position, player.transform.position) <= radius)
                    .ToArray();

                foreach (Player enemyPlayer in enemiesInRange)
                {
                    if (!PlayerManager.instance.CanSeePlayer(lineEffect.transform.position, enemyPlayer).canSee) continue;
                    var playerTransform = enemyPlayer.transform;
                    var position = playerTransform.position;
                    var lineEffectTransform = lineEffect.transform;
                    var lineEffectPosition = lineEffectTransform.position;
                    Vector3 dir = (position - transform.position).normalized;

                    enemyPlayer.data.healthHandler.CallTakeDamage(Damage * TimeHandler.deltaTime * dir, lineEffectPosition, null, Player);
                    enemyPlayer.data.healthHandler.CallTakeForce(1.45f * lineEffectTransform.localScale.x * (enemyPlayer.transform.position - lineEffectPosition).normalized);
                }
            }
        }
    }
}
