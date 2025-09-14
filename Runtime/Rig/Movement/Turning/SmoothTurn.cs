using UnityEngine;

namespace KadenZombie8.BIMOS.Rig.Movement
{
    /// <summary>
    /// Turn type with continuous rotation
    /// </summary>
    public class SmoothTurn : MonoBehaviour
    {
        private VirtualTurning _virtualTurning;
        private PhysicsRig _physicsRig;
        private float _turnVector;

        #region Enabling and disabling
        private void OnEnable()
        {
            _virtualTurning.TurnEvent += OnTurn;
        }

        private void OnDisable()
        {
            _virtualTurning.TurnEvent -= OnTurn;
        }
        #endregion

        private void Awake()
        {
            _physicsRig = GetComponent<PhysicsRig>();
            _virtualTurning = GetComponent<VirtualTurning>();
        }

        private void OnTurn(Vector2 vector)
        {
            _turnVector = vector.x;
        }

        private void Update()
        {
            if (Mathf.Abs(_turnVector) <= 0.75f)
                return;
            
            var turnDirection = _turnVector / Mathf.Abs(_turnVector);
            var degreesToTurn = _virtualTurning.TurnSpeed * Time.deltaTime;
            _physicsRig.Rigidbodies.Pelvis.transform.Rotate(0f, degreesToTurn * turnDirection, 0f);
        }
    }
}