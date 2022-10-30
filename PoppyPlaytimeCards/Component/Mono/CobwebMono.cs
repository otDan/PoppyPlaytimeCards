using Photon.Pun;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace PoppyPlaytimeCards.Component.Mono
{
    internal class CobwebMono : MonoBehaviour
    {
        public Player player;
        private SpriteRenderer _cobwebSprite;
        private bool _caught;

        private void Start()
        {
            _cobwebSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            if (_caught)
            {
                var alpha = _cobwebSprite.color.a - 0.0055f;
                _cobwebSprite.color = new Color(255, 255, 255, alpha);
                return;
            }

            float radius = 2.5f + _cobwebSprite.transform.localScale.x / _cobwebSprite.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            var enemiesInRange = PlayerManager.instance.players.Where(p =>
                p.teamID != player.teamID && !p.data.dead &&
                Vector3.Distance(p.transform.position, transform.position) < radius).ToArray();

            if (enemiesInRange.Length <= 0) return;
            foreach (Player caughtPlayer in enemiesInRange)
            {
                if (!PlayerManager.instance.CanSeePlayer(transform.position, caughtPlayer).canSee) continue;

                caughtPlayer.data.view.RPC("RPCA_AddSilence", RpcTarget.All, 3f);
                caughtPlayer.data.stunHandler.AddStun(3);
                _caught = true;
            }

            if (!_caught) return;
            PoppyPlaytimeCards.Instance.ExecuteAfterSeconds(3, () => Destroy(this));
        }
    }
}
