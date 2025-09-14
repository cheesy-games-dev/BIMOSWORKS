using System;
using Unity.Collections;
using UnityEngine;

namespace KadenZombie8.BIMOS
{
    public class Grounding : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 89f)]
        [Tooltip("The maximum slope angle (in degrees) that can be stood on")]
        private float _maxSlopeAngle = 50f;

        private float _minSlopeDot;
        private readonly float _minFriction = 0.1f;

        private Rigidbody _rigidbody;
        private SphereCollider _collider;

        private int _colliderId;

        public float MaxSlopeAngle
        {
            get => _maxSlopeAngle;
            set
            {
                _maxSlopeAngle = Mathf.Clamp(value, 0f, 89f);
                _minSlopeDot = Mathf.Cos((_maxSlopeAngle + 0.001f) * Mathf.Deg2Rad);
            }
        }

        public bool IsGrounded { get; private set; }

        public Vector3 GroundNormal { get; private set; }

        public bool IsSlipping { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
            _collider.hasModifiableContacts = true;
            _colliderId = _collider.GetInstanceID();
            MaxSlopeAngle = _maxSlopeAngle;
        }

        private void OnEnable() => Physics.ContactModifyEvent += OnContactModify;

        private void OnDisable() => Physics.ContactModifyEvent -= OnContactModify;

        private void OnContactModify(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
        {
            if (Physics.gravity.sqrMagnitude == 0f) return;

            IsGrounded = false;
            var upDirection = -Physics.gravity.normalized;

            foreach (var pair in pairs)
            {
                if (pair.colliderInstanceID != _colliderId && pair.otherColliderInstanceID != _colliderId) continue;

                var groundNormal = pair.GetNormal(0);
                if (pair.colliderInstanceID != _colliderId)
                    groundNormal *= -1f;

                var slopeDot = Vector3.Dot(groundNormal, upDirection);

                if (slopeDot < _minSlopeDot)
                {
                    pair.SetDynamicFriction(0, 0f);
                    pair.SetStaticFriction(0, 0f);
                    continue;
                }

                IsGrounded = true;
                GroundNormal = groundNormal;
            }
        }

        private void OnCollisionExit() => IsGrounded = false;

        private void OnCollisionStay(Collision collision)
        {
            if (!enabled) return;

            IsSlipping = collision.collider.material.staticFriction < _minFriction;
            if (IsSlipping) return;

            var gravity = Physics.gravity;
            if (gravity.sqrMagnitude == 0f) return;

            var upDirection = -gravity.normalized;
            var otherBody = collision.body;

            for (int i = 0; i < collision.contactCount; i++)
            {
                var contactPoint = collision.GetContact(i);
                var groundNormal = contactPoint.normal;
                var slopeDot = Vector3.Dot(groundNormal, upDirection);

                if (slopeDot < _minSlopeDot) continue;

                Vector3 alongPlaneVector = Vector3.Cross(groundNormal, upDirection);
                Vector3 upPlaneVector = Vector3.Cross(alongPlaneVector, groundNormal);

                var impulse = contactPoint.impulse;
                var counterImpulse = impulse.magnitude / slopeDot * upPlaneVector;
                _rigidbody.AddForce(counterImpulse, ForceMode.Impulse);

                switch (otherBody)
                {
                    case Rigidbody otherRigidbody:
                        otherRigidbody.AddForceAtPosition(-counterImpulse, contactPoint.point, ForceMode.Impulse);
                        break;
                    case ArticulationBody otherArticulationBody:
                        otherArticulationBody.AddForceAtPosition(-counterImpulse, contactPoint.point, ForceMode.Impulse);
                        break;
                }

                return;
            }
        }
    }
}
