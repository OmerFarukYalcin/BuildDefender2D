using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BuilderDefender
{
    public class DayNightCycle : MonoBehaviour
    {
        // Gradient used to define color changes during the day-night cycle
        [SerializeField] private Gradient gradient;

        // The number of seconds it takes for a full day to pass
        [SerializeField] private float secondsPerDay = 10f;

        // Reference to the 2D light component that will be affected by the cycle
        private Light2D light2D;

        // Tracks the current time of the day (normalized between 0 and 1)
        private float dayTime;

        // Speed at which the day progresses (1 full cycle per secondsPerDay)
        private float dayTimeSpeed;

        private void Awake()
        {
            // Get the Light2D component attached to the same GameObject
            light2D = GetComponent<Light2D>();

            // Calculate how fast the day progresses based on the secondsPerDay
            dayTimeSpeed = 1 / secondsPerDay;
        }

        private void Update()
        {
            // Increment dayTime based on the time passed since the last frame
            dayTime += Time.deltaTime * dayTimeSpeed;

            // Use the gradient to evaluate and set the light color based on the normalized dayTime (0 to 1)
            light2D.color = gradient.Evaluate(dayTime % 1f);
        }
    }
}
