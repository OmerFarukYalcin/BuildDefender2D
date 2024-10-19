using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class BuildingDemolishButton : MonoBehaviour
    {
        // Reference to the Building this button will demolish
        [SerializeField] Building building;

        private void Awake()
        {
            // Find the button and add an onClick listener to trigger the demolish action
            transform.Find("button").GetComponent<Button>().onClick.AddListener(() =>
            {
                // Get the building type from the Building component
                BuildingTypeSO buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;

                // Refund 60% of the resource cost for building construction
                foreach (ResourceAmount resourceAmount in buildingType.constructionResouceCostArray)
                {
                    ResourceManager.instance.AddResources(
                        resourceAmount.resourceType,
                        Mathf.FloorToInt(resourceAmount.amount * 0.6f)  // Calculate 60% of the original cost
                    );
                }

                // Destroy the building GameObject
                Destroy(building.gameObject);
            });
        }
    }
}
