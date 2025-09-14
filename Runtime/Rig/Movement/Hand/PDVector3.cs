using UnityEngine;

namespace KadenZombie8.BIMOS
{
    // Created by SparkMint.

    /// <summary>
    /// A PD Controller that outputs a Vector3 value.
    /// </summary>
    public class PDVector3
    {
        private readonly PDScalar _xPD, _yPD, _zPD;

        public PDVector3(float pGain, float dGain)
        {
            _xPD = new PDScalar(pGain, dGain);
            _yPD = new PDScalar(pGain, dGain);
            _zPD = new PDScalar(pGain, dGain);
        }

        /// <summary>
        /// Updates the Proportional Gain of this PD Controller to the specified value.
        /// </summary>
        /// <param name="pGain">The Proportional Gain to provide this Controller.</param>
        public void UpdateProportionalGain(float pGain)
        {
            _xPD.UpdateProportionalGain(pGain);
            _yPD.UpdateProportionalGain(pGain);
            _zPD.UpdateProportionalGain(pGain);
        }

        /// <summary>
        /// Updates the Derivative Gain of this PD Controller to the specified value.
        /// </summary>
        /// <param name="dGain">The Derivative Gain to provide this Controller.</param>
        public void UpdateDerivativeGain(float dGain)
        {
            _xPD.UpdateDerivativeGain(dGain);
            _yPD.UpdateDerivativeGain(dGain);
            _zPD.UpdateDerivativeGain(dGain);
        }

        /// <summary>
        /// Outputs a PD value based on the values provided.
        /// </summary>
        /// <param name="current">The current position we are at.</param>
        /// <param name="target">The target position we wish to reach.</param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public Vector3 CalculatePD(Vector3 current, Vector3 target, float deltaTime)
        {
            var x = _xPD.CalculatePD(current.x, target.x, deltaTime);
            var y = _yPD.CalculatePD(current.y, target.y, deltaTime);
            var z = _zPD.CalculatePD(current.z, target.z, deltaTime);

            return new Vector3(x, y, z);
        }
    }
}
