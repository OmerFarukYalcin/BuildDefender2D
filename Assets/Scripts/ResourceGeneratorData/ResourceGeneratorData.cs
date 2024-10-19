namespace BuilderDefender
{
    // This class holds the configuration data for a resource generator
    // It defines how the generator works, what resources it generates, and its efficiency range
    [System.Serializable] // This attribute allows the class to be serialized and shown in the Unity Inspector
    public class ResourceGeneratorData
    {
        // Maximum time (in seconds) required to generate one unit of the resource
        public float timerMax;

        // The type of resource that this generator produces (e.g., wood, stone)
        public ResourceTypeSO resourceType;

        // The detection radius used to find nearby resource nodes
        public float resourceDetectionRadius;

        // The maximum number of resources that can be detected and counted within the detection radius
        public int maxResouceAmount;
    }
}
