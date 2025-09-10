namespace KadenZombie8.BIMOS
{
    // Created by SparkMint.

    /// <summary>
    /// A simple one dimensional PD controller. Used to build more complex PD controllers.
    /// </summary>
    public class PDScalar
    {
        private float _proportionalGain;
        private float _derivativeGain;

        private float _previousError;

        public PDScalar(float pGain, float dGain)
        {
            _proportionalGain = pGain;
            _derivativeGain = dGain;
        }

        /// <summary>
        /// Updates the Proportional Gain of this PD Scalar to the specified value.
        /// </summary>
        /// <param name="pGain">The Proportional Gain to provide this Scalar.</param>
        public void UpdateProportionalGain(float pGain)
            => _proportionalGain = pGain;

        /// <summary>
        /// Updates the Derivative Gain of this PD Scalar to the specified value.
        /// </summary>
        /// <param name="dGain">The Derivative Gain to provide this Scalar.</param>
        public void UpdateDerivativeGain(float dGain)
            => _derivativeGain = dGain;

        /// <summary>
        /// Outputs a PD Scalar value based on the values provided.
        /// </summary>
        /// <param name="target">The value we wish to reach with the PD Scalar.</param>
        /// <param name="current">The current value we are at.</param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public float CalculatePD(float current, float target, float deltaTime)
        {
            float error = target - current;
            float derivative = (error - _previousError) / deltaTime;
            _previousError = error;
            return error * _proportionalGain + derivative * _derivativeGain;
        }
    }
}
