using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
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

        private int _rigidbodyId;

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
            _rigidbodyId = _rigidbody.GetInstanceID();
            MaxSlopeAngle = _maxSlopeAngle;
        }

        private void OnEnable()
        {
            Physics.ContactModifyEvent += OnContactModify;
            Physics.ContactEvent += OnContact;
        }

        private void OnDisable()
        {
            Physics.ContactModifyEvent -= OnContactModify;
            Physics.ContactEvent -= OnContact;
        }

        private void OnContactModify(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
        {
            IsGrounded = false;
            var gravity = Physics.gravity;

            if (gravity.sqrMagnitude == 0f) return;

            var upDirection = -gravity.normalized;

            foreach (var pair in pairs)
            {
                if (pair.colliderInstanceID != _rigidbodyId && pair.otherColliderInstanceID != _rigidbodyId) continue;

                var groundNormal = pair.GetNormal(0);
                if (pair.colliderInstanceID != _rigidbodyId)
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

        //private void OnCollisionStay(Collision collision)
        //{
        //    if (!enabled) return;

        //    IsSlipping = collision.collider.material.staticFriction < _minFriction;
        //    if (IsSlipping) return;

        //    var gravity = Physics.gravity;
        //    if (gravity.sqrMagnitude == 0f) return;

        //    var upDirection = -gravity.normalized;
        //    var otherBody = collision.body;

        //    for (int i = 0; i < collision.contactCount; i++)
        //    {
        //        var contactPoint = collision.GetContact(i);
        //        var groundNormal = contactPoint.normal;
        //        var slopeDot = Vector3.Dot(groundNormal, upDirection);

        //        if (slopeDot < _minSlopeDot) continue;

        //        Vector3 alongPlaneVector = Vector3.Cross(groundNormal, upDirection);
        //        Vector3 upPlaneVector = Vector3.Cross(alongPlaneVector, groundNormal);

        //        var impulse = contactPoint.impulse;
        //        var counterImpulse = impulse.magnitude / slopeDot * upPlaneVector;
        //        _rigidbody.AddForce(counterImpulse, ForceMode.Impulse);

        //        switch (otherBody)
        //        {
        //            case Rigidbody otherRigidbody:
        //                otherRigidbody.AddForceAtPosition(-counterImpulse, contactPoint.point, ForceMode.Impulse);
        //                break;
        //            case ArticulationBody otherArticulationBody:
        //                otherArticulationBody.AddForceAtPosition(-counterImpulse, contactPoint.point, ForceMode.Impulse);
        //                break;
        //        }

        //        return;
        //    }
        //}

        private void FixedUpdate()
        {
            _jobHandle.Complete();

            var otherBodyInstanceId = _result.OtherBodyInstanceId;
            //var otherBody = instance
            var point = _result.Point;
            var counterImpulse = _result.CounterImpulse;

            print(counterImpulse);
            _rigidbody.AddForce(counterImpulse, ForceMode.Impulse);

            //switch (otherBodyInstanceId)
            //{
            //    case Rigidbody otherRigidbody:
            //        otherRigidbody.AddForceAtPosition(-counterImpulse, point, ForceMode.Impulse);
            //        break;
            //    case ArticulationBody otherArticulationBody:
            //        otherArticulationBody.AddForceAtPosition(-counterImpulse, point, ForceMode.Impulse);
            //        break;
            //}
        }

        private void OnContact(PhysicsScene scene, NativeArray<ContactPairHeader>.ReadOnly pairHeaders)
        {
            var gravity = Physics.gravity;
            if (gravity.sqrMagnitude == 0f) return;

            var upDirection = -gravity.normalized;

            int n = pairHeaders.Length;

            _data.RigidbodyId = _rigidbodyId;
            _data.UpDirection = upDirection;
            _data.MinSlopeDot = _minSlopeDot;

            AddCounterImpulseJob job = new()
            {
                PairHeaders = pairHeaders,
                Data = _data,
                Result = _result
            };

            _jobHandle = job.Schedule(n, 256);
        }

        private struct JobDataStruct
        {
            public int RigidbodyId;
            public Vector3 UpDirection;
            public float MinSlopeDot;
        }

        private struct JobResultStruct
        {
            public int OtherBodyInstanceId;
            public Vector3 Point;
            public Vector3 CounterImpulse;
        }

        private JobDataStruct _data;
        private JobResultStruct _result;
        private JobHandle _jobHandle;

        private struct AddCounterImpulseJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<ContactPairHeader>.ReadOnly PairHeaders;

            [ReadOnly]
            public JobDataStruct Data;

            public JobResultStruct Result;

            public void Execute(int index)
            {
                var counterImpulse = Vector3.zero;
                var point = Vector3.zero;

                for (int j = 0; j < PairHeaders[index].pairCount; j++)
                {
                    ref readonly var pair = ref PairHeaders[index].GetContactPair(j);

                    if (pair.isCollisionExit) continue;

                    var impulse = pair.impulseSum;
                    var groundNormal = impulse.normalized;

                    var slopeDot = Vector3.Dot(groundNormal, Data.UpDirection);

                    if (slopeDot < Data.MinSlopeDot) continue;

                    Vector3 alongPlaneVector = Vector3.Cross(groundNormal, Data.UpDirection);
                    Vector3 upPlaneVector = Vector3.Cross(alongPlaneVector, groundNormal);

                    counterImpulse = impulse.magnitude / slopeDot * upPlaneVector;
                    point = pair.GetContactPoint(0).position;

                    break;
                }

                Result = new()
                {
                    OtherBodyInstanceId = PairHeaders[index].otherBodyInstanceID,
                    Point = point,
                    CounterImpulse = counterImpulse
                };
            }
        }
    }
}
