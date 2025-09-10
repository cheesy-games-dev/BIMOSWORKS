using UnityEngine;

namespace KadenZombie8.BIMOS.Rig.Tempor
{
    /// <summary>
    /// Handles switching between locomotion types (smooth &
    /// teleport).
    /// </summary>
    public class SmoothLocomotion : MonoBehaviour
    {
        [SerializeField] private Movement _movement;

        private void OnEnable()
        {
            _movement.enabled = true;
        }

        private void OnDisable()
        {
            _movement.enabled = false;
        }
    }
}
