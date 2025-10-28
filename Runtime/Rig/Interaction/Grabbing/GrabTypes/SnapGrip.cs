using UnityEngine;

namespace KadenZombie8.BIMOS.Rig {
    public class SnapGrip : Grabbable
    {
        public float RadiusOffset = 0.1f;
        public override float CalculateRank(Hand hand) => base.CalculateRank(hand) * 3f;

        public override void AlignHand(Hand hand, out Vector3 position, out Quaternion rotation) {
            base.AlignHand(hand, out position, out rotation);
            var handType = hand.Handedness;
            var isRightHanded = handType == Handedness.Right;
            position = transform.TransformPoint(hand.PalmTransform.InverseTransformPoint(hand.PhysicsHandTransform.position));
            var radiusOffset = Vector3.one * RadiusOffset;
            var posOffset = isRightHanded ? radiusOffset : -radiusOffset;
            position += posOffset;
            var rotOffset = transform.rotation * Quaternion.Inverse(hand.PalmTransform.rotation) * hand.PhysicsHandTransform.rotation;
            rotation = isRightHanded ? rotOffset : Quaternion.Inverse(rotOffset);
        }
    }
}
