using UnityEngine;

namespace KadenZombie8.BIMOS.Rig
{
    [AddComponentMenu("BIMOS/Grabbables/Grabbable (Snap)")]
    public class SnapGrabbable : Grabbable
    {
        public float Range = 0.01f; 

        public override float CalculateRank(Hand hand) => base.CalculateRank(hand) * 3f;

        public override void AlignHand(Hand hand, out Vector3 position, out Quaternion rotation)
        {
            var range = Vector3.one * Range;
            position = transform.TransformPoint(hand.PalmTransform.InverseTransformPoint(hand.PhysicsHandTransform.position)) + range;
            var rawRotation = transform.rotation * Quaternion.Inverse(hand.PalmTransform.rotation) * hand.PhysicsHandTransform.rotation;
            rotation = hand.Handedness == Handedness.Right ? rawRotation : Quaternion.Inverse(rawRotation);
        }
    }
}