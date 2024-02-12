using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BuilderDefender
{
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] Gradient gradient;
        [SerializeField] float secondsPerDay = 10f;
        private Light2D light2D;
        private float dayTime;
        private float dayTimeSpeed;
        void Awake()
        {
            light2D = GetComponent<Light2D>();

            dayTimeSpeed = 1 / secondsPerDay;
        }

        void Update()
        {
            dayTime += Time.deltaTime * dayTimeSpeed;
            light2D.color = gradient.Evaluate(dayTime % 1f);
        }
    }
}
