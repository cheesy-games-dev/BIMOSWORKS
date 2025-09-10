using UnityEngine;

namespace KadenZombie8.BIMOS
{
    public class SpeedPrinter : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        private void FixedUpdate() => print(name + " Speed: " + _rigidbody.linearVelocity.magnitude);
    }
}
