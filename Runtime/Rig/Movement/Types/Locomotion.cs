using UnityEngine;

namespace KadenZombie8.BIMOS.Rig.Movement
{
    /// <summary>
    /// Handles switching between locomotion types (smooth &
    /// teleport).
    /// </summary>
    public class Locomotion : MonoBehaviour
    {
        [SerializeField]
        private SmoothLocomotion _smoothLocomotion;

        private void OnEnable()
        {
            _smoothLocomotion.enabled = true;
        }

        private void OnDisable()
        {
            _smoothLocomotion.enabled = false;
        }
    }
}
