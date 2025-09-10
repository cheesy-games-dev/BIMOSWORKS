using System;
using UnityEngine;

namespace KadenZombie8.BIMOS.Rig.Tempor
{
    /// <summary>
    /// Handles turning input and turn types
    /// </summary>
    public class VirtualTurning : MonoBehaviour
    {
        public event Action<Vector2> TurnEvent;
        
        [Tooltip("The speed (in degrees/second) turning occurs at")]
        public float TurnSpeed = 450f;

        private SnapTurn _snapTurn;
        private SmoothTurn _smoothTurn;
        private VirtualTurningMode _turningMode;
        private enum VirtualTurningMode
        {
            NoTurn,
            SnapTurn,
            SmoothTurn
        }
        private VirtualTurningMode TurningMode
        {
            get => _turningMode;
            set
            {
                _turningMode = value;
                switch (value)
                {
                    case VirtualTurningMode.NoTurn:
                        _snapTurn.enabled = false;
                        _smoothTurn.enabled = false;
                        break;
                    case VirtualTurningMode.SnapTurn:
                        _snapTurn.enabled = true;
                        _smoothTurn.enabled = false;
                        break;
                    case VirtualTurningMode.SmoothTurn:
                        _snapTurn.enabled = false;
                        _smoothTurn.enabled = true;
                        break;
                }
            }
        }

        private void Awake()
        {
            //_inputReader.TurnEvent += OnTurn;
            _snapTurn = GetComponent<SnapTurn>();
            _smoothTurn = GetComponent<SmoothTurn>();
        }

        private void OnTurn(Vector2 direction)
        {
            TurnEvent?.Invoke(direction);
        }
    }
}