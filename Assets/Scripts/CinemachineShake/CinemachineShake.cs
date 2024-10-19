using UnityEngine;
using Cinemachine;

namespace BuilderDefender
{
    public class CinemachineShake : MonoBehaviour
    {
        // Singleton instance of CinemachineShake
        public static CinemachineShake instance { get; private set; }

        // Reference to the Cinemachine virtual camera and its Perlin noise component
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin cinemachineMultiChannelPerlin;

        // Variables to control shake intensity and duration
        private float timer;
        private float timerMax;
        private float startingIntensity;

        private void Awake()
        {
            // Initialize singleton instance
            instance = this;

            // Get the virtual camera and Perlin noise component
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            cinemachineMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            // If the shake effect is still active, update the timer
            if (timer < timerMax)
            {
                timer += Time.deltaTime;

                // Gradually reduce the shake intensity over time using linear interpolation
                float amplitude = Mathf.Lerp(startingIntensity, 0f, timer / timerMax);
                cinemachineMultiChannelPerlin.m_AmplitudeGain = amplitude;
            }
        }

        // Method to initiate a camera shake with a specified intensity and duration
        public void ShakeCamera(float intensity, float timerMax)
        {
            // Set the maximum shake duration and reset the timer
            this.timerMax = timerMax;
            timer = 0f;

            // Set the starting intensity of the shake
            startingIntensity = intensity;

            // Apply the initial intensity to the Perlin noise component
            cinemachineMultiChannelPerlin.m_AmplitudeGain = intensity;
        }
    }
}
