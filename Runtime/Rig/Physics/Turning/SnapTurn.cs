using System.Collections;
using UnityEngine;

namespace KadenZombie8.BIMOS.Rig.Tempor
{
    /// <summary>
    /// Turn type with stepped rotation
    /// </summary>
    public class SnapTurn : MonoBehaviour
    {
        [Tooltip("The angle (in degrees) the player turns when they move the turn stick horizontally")]
        public float TurnIncrement = 45;

        private VirtualTurning _virtualTurning;
        private PhysicsRig _physicsRig;
        private bool _isTurning;

        #region Enabling and disabling
        private void OnEnable()
        {
            _virtualTurning.TurnEvent += Turn;
        }

        private void OnDisable()
        {
            _virtualTurning.TurnEvent -= Turn;
        }
        #endregion

        private void Awake()
        {
            _physicsRig = GetComponent<PhysicsRig>();
            _virtualTurning = GetComponent<VirtualTurning>();
        }

        private void Turn(Vector2 vector)
        {
            var turnVector = vector.x;
            var wasTurning = _isTurning;
            _isTurning = Mathf.Abs(turnVector) > 0.75f;

            if (wasTurning || !_isTurning)
                return;
            
            var turnDirection = turnVector / Mathf.Abs(turnVector);
            StartCoroutine(Snap(turnDirection));
        }

        private IEnumerator Snap(float turnDirection)
        {
            var degreesLeftToTurn = TurnIncrement;
            while (degreesLeftToTurn > 0f)
            {
                var degreesToTurn = Mathf.Min(degreesLeftToTurn, _virtualTurning.TurnSpeed * Time.deltaTime);
                degreesLeftToTurn -= degreesToTurn;
                _physicsRig.Rigidbodies.Pelvis.transform.Rotate(0f, degreesToTurn * turnDirection, 0f);
                yield return null;
            }
        }
    }
}