using System.Collections.Generic;
using System;
using UnityEngine;

namespace BuilderDefender
{
    public class ResourceManager : MonoBehaviour
    {
        // List of starting resources (defined in Unity Inspector)
        [SerializeField] List<ResourceAmount> startingResourceAmountList;

        // Event triggered when the amount of any resource changes
        public event EventHandler onResourceAmountChange;

        // Singleton instance of ResourceManager
        public static ResourceManager instance { get; private set; }

        // Dictionary to store the current amount of each resource type
        private Dictionary<ResourceTypeSO, int> resourcesAmountDictionary;

        private void Awake()
        {
            // Set up singleton instance
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject); // Destroy any duplicate instances

            // Initialize the dictionary to track resource amounts
            resourcesAmountDictionary = new Dictionary<ResourceTypeSO, int>();

            // Load the list of all available resource types
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

            // Set initial amount of each resource type to 0 in the dictionary
            foreach (ResourceTypeSO resourceType in resourceTypeList.list)
            {
                resourcesAmountDictionary[resourceType] = 0;
            }

            // Add the starting resources defined in the Unity Inspector to the dictionary
            foreach (ResourceAmount resourceAmount in startingResourceAmountList)
            {
                AddResources(resourceAmount.resourceType, resourceAmount.amount);
            }
        }

        // Adds a specified amount of a resource to the dictionary
        public void AddResources(ResourceTypeSO resourceType, int amount)
        {
            resourcesAmountDictionary[resourceType] += amount;

            // Trigger the resource amount change event
            onResourceAmountChange?.Invoke(this, EventArgs.Empty);

            // Log the current resource amounts for testing
            TestLogResourceAmountDictionary();
        }

        // Returns the current amount of a specific resource type
        public int GetResourceAmount(ResourceTypeSO resourceType)
        {
            return resourcesAmountDictionary[resourceType];
        }

        // Logs the current amount of all resource types for debugging purposes
        private void TestLogResourceAmountDictionary()
        {
            foreach (ResourceTypeSO resourceType in resourcesAmountDictionary.Keys)
            {
                Debug.Log(resourceType.nameString + ": " + resourcesAmountDictionary[resourceType]);
            }
        }

        // Checks if the player can afford a list of resource amounts
        public bool CanAfford(ResourceAmount[] resourceAmounts)
        {
            foreach (ResourceAmount resourceAmount in resourceAmounts)
            {
                // If the current amount of a resource is less than the required amount, return false
                if (GetResourceAmount(resourceAmount.resourceType) < resourceAmount.amount)
                {
                    return false;
                }
            }

            return true; // Player can afford the resources
        }

        // Subtracts a list of resource amounts from the player's total resources
        public void SpendResources(ResourceAmount[] resourceAmounts)
        {
            foreach (ResourceAmount resourceAmount in resourceAmounts)
            {
                // Reduce the amount of each resource in the dictionary
                resourcesAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
            }
        }
    }
}
