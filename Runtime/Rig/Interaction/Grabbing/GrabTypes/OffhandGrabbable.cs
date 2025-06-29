using System.Collections;
using UnityEngine;

namespace KadenZombie8.BIMOS.Rig
{
    [AddComponentMenu("BIMOS/Grabbables/Grabbable (Offhand)")]
    public class OffhandGrabbable : SnapGrabbable
    {
        public override void Grab(Hand hand)
        {
            base.Grab(hand);
            AlignHand(hand, out var position, out var rotation);
            StartCoroutine(TargetOffsetCoroutine(hand));
        }

        private IEnumerator TargetOffsetCoroutine(Hand hand)
        {
            Transform otherPhysicsHand = hand.OtherHand.PhysicsHandTransform;
            hand.PhysicsHand.Target = hand.OtherHand.PhysicsHand.Target;

            if (!hand.OtherHand.CurrentGrab)
                yield return null;

            Vector3 targetPosition = transform.TransformPoint(hand.PalmTransform.InverseTransformPoint(hand.PhysicsHandTransform.position));
            Quaternion targetRotation = transform.rotation * Quaternion.Inverse(hand.PalmTransform.rotation) * hand.PhysicsHandTransform.rotation;

            var initialOffsetPosition = otherPhysicsHand.InverseTransformPoint(hand.PhysicsHandTransform.position);
            var initialOffsetRotation = Quaternion.Inverse(otherPhysicsHand.rotation) * hand.PhysicsHandTransform.rotation;

            var finalOffsetPosition = otherPhysicsHand.InverseTransformPoint(targetPosition);
            var finalOffsetRotation = Quaternion.Inverse(otherPhysicsHand.rotation) * targetRotation;

            AlignHand(hand, out var position, out var rotation);

            hand.transform.GetPositionAndRotation(out var initialPosition, out var initialRotation);

            hand.PhysicsHandTransform.SetPositionAndRotation(initialPosition, initialRotation);

            var elapsedTime = 0f;
            var positionDifference = Mathf.Min(
                Vector3.Distance(initialPosition, position), MaxPositionDifference)
                / MaxPositionDifference;
            var rotationDifference = Quaternion.Angle(initialRotation, rotation) / 180f;
            var averageDifference = Mathf.Min(positionDifference + rotationDifference, 1f);
            var grabTime = MaxGrabTime * averageDifference;
            while (elapsedTime < grabTime)
            {
                if (!hand.GrabJoint)
                    yield break;

                var lerpedTargetPosition = Vector3.Lerp(initialOffsetPosition, finalOffsetPosition, elapsedTime / grabTime);
                var lerpedTargetRotation = Quaternion.Lerp(initialOffsetRotation, finalOffsetRotation, elapsedTime / grabTime);

                hand.PhysicsHand.TargetOffsetPosition = lerpedTargetPosition;
                hand.PhysicsHand.TargetOffsetRotation = lerpedTargetRotation;

                elapsedTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            if (hand.GrabJoint)
            {
                hand.PhysicsHand.TargetOffsetPosition = finalOffsetPosition;
                hand.PhysicsHand.TargetOffsetRotation = finalOffsetRotation;
            }
            else
            {
                hand.PhysicsHand.TargetOffsetPosition = Vector3.zero;
                hand.PhysicsHand.TargetOffsetRotation = Quaternion.identity;
            }
        }

        public override void DestroyGrabJoint(Hand hand)
        {
            base.DestroyGrabJoint(hand);
            hand.PhysicsHand.Target = hand.PhysicsHand.Controller;
            hand.PhysicsHand.TargetOffsetPosition = Vector3.zero;
            hand.PhysicsHand.TargetOffsetRotation = Quaternion.identity;
        }
    }
}
