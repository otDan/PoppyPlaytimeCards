using System.Collections.Generic;
using System.Linq;
using PoppyPlaytimeCards.Asset;
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
                    var bunzoBunnyEffect = Instantiate(AssetManager.BunzoBunnyEffect, Player.gameObject.transform);
                    var lineEffect = bunzoBunnyEffect.GetComponent<LineEffect>();
                    var removeAfterSeconds = bunzoBunnyEffect.GetOrAddComponent<RemoveAfterSeconds>();
                    removeAfterSeconds.seconds = 5;
                    _lineEffects.Add(lineEffect);
                }

                if (_times >= 20)
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
            foreach (var lineEffect in new List<LineEffect>(_lineEffects))
            {
                if (lineEffect.counter >= 1)
                {
                    _lineEffects.Remove(lineEffect);
                    Destroy(lineEffect.gameObject);
                }

                var radiusOverTime = lineEffect.radiusOverTime.Evaluate(lineEffect.counter);
                float radius = 2.5f + lineEffect.radius * lineEffect.transform.root.localScale.x * radiusOverTime;
                var enemiesInRange = PlayerManager.instance.players.Where(player => player.teamID != Player.teamID && 
                        !player.data.dead && 
                        Vector3.Distance(player.transform.position, transform.position) <= radius)
                    .ToArray();

                foreach (Player player in enemiesInRange)
                {
                    if (!PlayerManager.instance.CanSeePlayer(transform.position, player).canSee) continue;
                    var playerTransform = player.transform;
                    var position = playerTransform.position;
                    Vector3 dir = (position - transform.position).normalized;

                    player.data.healthHandler.TakeDamage(Damage * TimeHandler.deltaTime * dir, position, null, Player);
                }
            }
        }
    }
}
