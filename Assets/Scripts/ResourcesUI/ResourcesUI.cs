using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class ResourcesUI : MonoBehaviour
    {
        // List of all available resource types
        private ResourceTypeListSO resourceTypeList;

        // Dictionary to map resource types to their corresponding UI elements (transforms)
        private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;

        // Awake is called when the script instance is being loaded
        void Awake()
        {
            // Load the list of resource types from the Resources folder
            resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

            // Initialize the dictionary that links resource types to UI elements
            resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

            // Find the resource template UI element (used to create instances of the resource display)
            Transform resourceTemplate = transform.Find("ResourceTemplate");
            resourceTemplate.gameObject.SetActive(false);  // Hide the template itself

            // Offset to position the resource UI elements horizontally
            float offsetAmount = -160f;
            int index = 0;

            // Loop through each resource type and create a UI element for it
            foreach (ResourceTypeSO resourceType in resourceTypeList.list)
            {
                // Instantiate a new UI element for the resource using the template
                Transform resourceTransform = Instantiate(resourceTemplate, transform);

                // Set the base position of the UI element and offset it based on the index
                Vector2 basePos = resourceTransform.GetComponent<RectTransform>().anchoredPosition;
                resourceTransform.gameObject.SetActive(true);

                // Set the sprite of the resource image based on the resource type
                resourceTransform.transform.Find("Image").GetComponent<Image>().sprite = resourceType.sprite;

                // Position the resource UI element with a horizontal offset
                resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + offsetAmount * index, basePos.y);

                // Store the resource type and its corresponding UI element in the dictionary
                resourceTypeTransformDictionary[resourceType] = resourceTransform;

                // Increment the index for the next resource's position
                index += 1;
            }
        }

        private void Start()
        {
            // Subscribe to the ResourceManager's event for when resources change
            ResourceManager.instance.onResourceAmountChange += HandleOnResourceAmountChange;

            // Update the UI initially to reflect the current resource amounts
            UpdateResourceAmount();
        }

        // Event handler called when resources change in the ResourceManager
        private void HandleOnResourceAmountChange(object sender, EventArgs e)
        {
            // Update the UI when the resource amounts change
            UpdateResourceAmount();
        }

        // Updates the resource amount text for each resource type in the UI
        private void UpdateResourceAmount()
        {
            // Loop through each resource type and update the displayed amount in the UI
            foreach (ResourceTypeSO resourceType in resourceTypeList.list)
            {
                // Get the current amount of the resource from the ResourceManager
                int resourceAmount = ResourceManager.instance.GetResourceAmount(resourceType);

                // Find the corresponding UI element for the resource
                Transform resourceTransform = resourceTypeTransformDictionary[resourceType];

                // Update the resource amount text in the UI
                resourceTransform.transform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
            }
        }
    }
}
