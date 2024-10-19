using UnityEngine;

namespace BuilderDefender
{
    public class ResourceGenerator : MonoBehaviour
    {
        // This static method calculates the number of nearby resource nodes within the specified radius
        // It is used to determine how many resources are available near the generator.
        public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
        {
            // Find all colliders within the resource detection radius
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

            int nearbyResourceAmount = 0;

            // Loop through each collider found in the detection radius
            foreach (Collider2D collider2D in collider2Ds)
            {
                // Check if the collider belongs to a resource node
                ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();

                // If a resource node is found and it matches the required resource type, increment the resource count
                if (resourceNode != null)
                {
                    if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                    {
                        nearbyResourceAmount++;
                    }
                }
            }

            // Clamp the number of nearby resources to the maximum allowed by the generator data
            nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResouceAmount);
            return nearbyResourceAmount;
        }

        // Data related to how the resource generator works (e.g., resource type, detection radius)
        private ResourceGeneratorData resourceGeneratorData;

        // Timer to control resource generation over time
        private float timer;

        // The maximum timer value (the time it takes to generate one resource)
        private float timerMax;

        private void Awake()
        {
            // Get the ResourceGeneratorData from the BuildingType attached to this GameObject
            resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
            timerMax = resourceGeneratorData.timerMax; // Set the maximum timer from the generator data
        }

        private void Start()
        {
            // Calculate how many resources are nearby at the generator's position
            int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);

            // If no resources are nearby, disable the resource generator
            if (nearbyResourceAmount == 0)
            {
                enabled = false;
            }
            else
            {
                // Adjust the timer based on the number of nearby resources, improving efficiency
                timerMax = (resourceGeneratorData.timerMax / 2f) + resourceGeneratorData.timerMax * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResouceAmount);
            }
        }

        private void Update()
        {
            // Reduce the timer by the time passed in this frame
            timer -= Time.deltaTime;

            // When the timer reaches zero or less, generate a resource and reset the timer
            if (timer <= 0f)
            {
                timer += timerMax; // Reset the timer
                ResourceManager.instance.AddResources(resourceGeneratorData.resourceType, 1); // Add 1 resource to the ResourceManager
            }
        }

        // Getter to retrieve the ResourceGeneratorData associated with this generator
        public ResourceGeneratorData GetResourceGeneratorData => resourceGeneratorData;

        // Returns the current timer value as a normalized value between 0 and 1, used for UI progress bars
        public float GetTimerNormalized()
        {
            return timer / timerMax;
        }

        // Returns how many resources are generated per second
        public float GetAmountGeneratedPerSecond()
        {
            return 1 / timerMax;
        }
    }
}
