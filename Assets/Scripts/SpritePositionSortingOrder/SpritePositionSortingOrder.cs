using UnityEngine;

namespace BuilderDefender
{
    public class SpritePositionSortingOrder : MonoBehaviour
    {
        // Whether the script should run only once or continuously
        [SerializeField] private bool runOnce;

        // Offset for the Y position to adjust sorting order
        [SerializeField] private float positionOffsetY;

        // Reference to the SpriteRenderer component
        private SpriteRenderer spriteRenderer;

        // Multiplier to increase sorting precision
        float precisionMultiplier = 5f;

        private void Awake()
        {
            // Get the SpriteRenderer component attached to this GameObject
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            // Calculate and set the sorting order based on the Y position of the object
            // Objects with lower Y positions (further down) will appear in front of objects with higher Y positions
            spriteRenderer.sortingOrder = (int)(-(transform.position.y + positionOffsetY) * precisionMultiplier);

            // If runOnce is true, the script will destroy itself after the sorting order is set once
            if (runOnce)
            {
                Destroy(this);
            }
        }
    }
}
