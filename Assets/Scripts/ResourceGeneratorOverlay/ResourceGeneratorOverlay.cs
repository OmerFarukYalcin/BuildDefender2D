using TMPro;
using UnityEngine;

namespace BuilderDefender
{
    public class ResourceGeneratorOverlay : MonoBehaviour
    {
        // Reference to the ResourceGenerator component, which controls the resource generation logic
        [SerializeField] ResourceGenerator resourceGenerator;

        // Reference to the transform of the bar that visually represents resource generation progress
        private Transform barTransform;

        private void Start()
        {
            // Get the data associated with the ResourceGenerator
            ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData;

            // Find and cache the transform of the progress bar
            barTransform = transform.Find("bar");

            // Set the sprite of the resource type icon based on the resource type in ResourceGeneratorData
            transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;

            // Set the text to display the amount of resources generated per second, formatted to 1 decimal place
            transform.Find("text").GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
        }

        private void Update()
        {
            // Update the progress bar scale based on the generator's normalized timer
            // This shows how much time is left before the next resource is generated
            barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(), 1, 1);
        }
    }
}
