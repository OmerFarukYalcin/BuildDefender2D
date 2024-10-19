using UnityEngine;

namespace BuilderDefender
{
    // This class represents a resource node in the game world.
    // A resource node is an object that provides a specific type of resource when interacted with.
    public class ResourceNode : MonoBehaviour
    {
        // The type of resource this node provides (e.g., wood, stone, etc.)
        public ResourceTypeSO resourceType;
    }
}
