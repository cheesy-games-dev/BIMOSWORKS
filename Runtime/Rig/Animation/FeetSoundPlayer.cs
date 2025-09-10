using KadenZombie8.BIMOS.Audio;
using KadenZombie8.BIMOS.Rig.Tempor;
using UnityEngine;
using UnityEngine.Audio;

namespace KadenZombie8.BIMOS.Rig
{
    public class FeetSoundPlayer : SoundPlayer
    {
        [SerializeField]
        private AudioResource
            _walkSound,
            _runSound;

        private Feet _feet;
        private Tempor.Movement _smoothLocomotion;

        protected override void Awake()
        {
            base.Awake();
            _feet = BIMOSRig.Instance.AnimationRig.Feet;
            _smoothLocomotion = BIMOSRig.Instance.PhysicsRig.GetComponent<Movement>();
        }

        private void OnEnable() => _feet.OnStep += Stepped;

        private void OnDisable() => _feet.OnStep -= Stepped;

        private void Stepped() => Play(_smoothLocomotion.IsRunning ? _runSound : _walkSound);
    }
}
