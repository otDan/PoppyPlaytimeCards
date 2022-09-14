using UnityEngine;

namespace PoppyPlaytimeCards.Component
{
    internal class AnimationAutoDestroy : MonoBehaviour
    {
        private Animator _animator;

        private void Start () {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) || _animator.IsInTransition(0)) return;
            Destroy(gameObject);
        }
    }
}
