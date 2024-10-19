namespace BuilderDefender
{
    // Serializable class to represent a specific amount of a resource type
    [System.Serializable] // This attribute allows the class to be serialized and shown in the Unity Inspector
    public class ResourceAmount
    {
        // The type of resource (e.g., wood, stone, etc.)
        public ResourceTypeSO resourceType;

        // The amount of the specified resource type
        public int amount;
    }
}
