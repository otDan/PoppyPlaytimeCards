using PoppyPlaytimeCards.Component.Mono;
using UnityEngine;

namespace PoppyPlaytimeCards.Component
{
    internal class DamagedPlayer : DealtDamageEffect
    {
        public Player Player;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null)
        {
            Player = GetComponent<Player>();
            JumpScareMono.Instance.CallDamage(Player, damagedPlayer);
        }
    }
}
