using Unity.Collections;
using UnityEngine;

namespace KadenZombie8.BIMOS
{
    public class HandWallSlipping : MonoBehaviour
    {
        [SerializeField]
        private float _maxSlopeAngle = 50f;

        private float _minSlopeDot;
        private Collider _collider;

        private int _colliderId;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.hasModifiableContacts = true;
            _colliderId = _collider.GetInstanceID();

            _minSlopeDot = Mathf.Cos((_maxSlopeAngle + 0.001f) * Mathf.Deg2Rad);
        }

        private void OnEnable() => Physics.ContactModifyEvent += OnContactModify;

        private void OnDisable() => Physics.ContactModifyEvent -= OnContactModify;

        private void OnContactModify(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
        {
            if (Physics.gravity.sqrMagnitude == 0f) return;

            var upDirection = -Physics.gravity.normalized;
            
            foreach (var pair in pairs)
            {
                if (pair.colliderInstanceID != _colliderId && pair.otherColliderInstanceID != _colliderId) continue;

                var slopeNormal = pair.GetNormal(0);
                if (pair.colliderInstanceID != _colliderId)
                    slopeNormal *= -1f;

                var slopeDot = Vector3.Dot(slopeNormal, upDirection);

                if (slopeDot > _minSlopeDot) continue;

                pair.SetDynamicFriction(0, 0f);
                pair.SetStaticFriction(0, 0f);
            }
        }
    }
}
