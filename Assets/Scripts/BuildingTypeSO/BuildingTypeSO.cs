using UnityEngine;

namespace BuilderDefender
{
    // ScriptableObject representing a building type with its relevant data
    [CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
    public class BuildingTypeSO : ScriptableObject
    {
        // Name of the building type
        public string nameString;

        // Prefab reference for the building object
        public Transform prefab;

        // Data for resource generation, if the building generates resources
        public ResourceGeneratorData resourceGeneratorData;

        // Sprite used to represent the building in UI
        public Sprite sprite;

        // Flag indicating if the building has resource generation capabilities
        public bool hasResourceGeneratorData;

        // Minimum distance required between buildings of the same type
        public float minConstructionRadius;

        // Array that defines the resource costs for constructing this building
        public ResourceAmount[] constructionResouceCostArray;

        // Maximum health the building can have
        public int healtAmountMax;

        // Maximum time required to construct this building
        public float constructionTimerMax;

        // Method to generate a string representation of the construction resource cost
        public string GetConstructionResouceCostString()
        {
            string cost = "";

            // Iterate through the resource cost array and format the cost string with color coding
            foreach (ResourceAmount resouceAmount in constructionResouceCostArray)
            {
                cost += $"<color=#{resouceAmount.resourceType.colorHex}>" + resouceAmount.resourceType.nameShort +
                resouceAmount.amount + "</color>";
            }

            return cost;
        }
    }
}
