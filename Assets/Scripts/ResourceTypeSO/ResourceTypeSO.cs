using UnityEngine;

namespace BuilderDefender
{
    // This ScriptableObject defines a specific resource type (e.g., wood, stone, etc.).
    // It stores data related to the resource, such as its name, sprite, and color.
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]// Allows creating this ScriptableObject in the Unity editor.
    public class ResourceTypeSO : ScriptableObject
    {
        // The full name of the resource (e.g., "Wood", "Stone")
        public string nameString;

        // The short name of the resource (e.g., "W" for wood)
        public string nameShort;

        // The sprite used to visually represent this resource in the UI
        public Sprite sprite;

        // The color of the resource in hex format (e.g., "#FFFFFF" for white)
        public string colorHex;
    }
}
