using TMPro;
using UnityEngine;

namespace BuilderDefender
{
    public class ResourceNearbyOverlay : MonoBehaviour
    {
        // Holds the data for the associated Resource Generator, including resource type, radius, and max amount
        private ResourceGeneratorData resourceGeneratorData;

        // Called when the script instance is being loaded
        private void Awake()
        {
            // Hide the overlay initially, as it's not visible by default
            Hide();
        }

        // Called once per frame
        private void Update()
        {
            // Calculate how many resources are nearby based on the generator's data and position
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position - transform.localPosition);

            // Calculate the percentage of nearby resources compared to the generator's maximum capacity
            float percent = Mathf.Round((float)nearbyResourceAmount / resourceGeneratorData.maxResouceAmount * 100f);

            // Update the text to show the percentage of available resources
            transform.Find("text").GetComponent<TextMeshPro>().SetText($"{percent}%");
        }

        // Method to show the overlay with the provided ResourceGeneratorData
        public void Show(ResourceGeneratorData _resourceGeneratorData)
        {
            // Set the resource generator data
            resourceGeneratorData = _resourceGeneratorData;

            // Make the overlay visible
            gameObject.SetActive(true);

            // Update the icon to represent the resource type being generated
            transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
        }

        // Method to hide the overlay
        public void Hide()
        {
            // Deactivate the game object, effectively hiding the overlay
            gameObject.SetActive(false);
        }
    }
}
