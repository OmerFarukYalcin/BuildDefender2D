using UnityEngine;
using UnityEngine.Rendering;

namespace BuilderDefender
{
    public class ChromaticAberration : MonoBehaviour
    {
        // Singleton instance of ChromaticAberration
        public static ChromaticAberration instance { get; private set; }

        // Reference to the Volume component which controls the post-processing effect
        private Volume volume;

        private void Awake()
        {
            // Initialize the singleton instance
            instance = this;

            // Get the Volume component attached to this GameObject
            volume = GetComponent<Volume>();
        }

        private void Update()
        {
            // Gradually decrease the chromatic aberration effect over time if it's active
            if (volume.weight > 0)
            {
                float decreaseSpeed = 1f;
                volume.weight -= Time.deltaTime * decreaseSpeed;
            }
        }

        // Method to manually set the strength (weight) of the chromatic aberration effect
        public void SetWeight(float weight)
        {
            volume.weight = weight;
        }
    }
}
