using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class BuildingRepairBtn : MonoBehaviour
    {
        // Reference to the health system of the building and the gold resource type
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private ResourceTypeSO goldResourceType;

        private void Awake()
        {
            // Find the repair button and add an onClick listener for the repair action
            transform.Find("button").GetComponent<Button>().onClick.AddListener(() =>
            {
                // Calculate missing health and repair cost (1 gold for every 2 health points)
                int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
                int repairCost = missingHealth / 2;

                // Create a resource cost array for the repair
                ResourceAmount[] resourceAmountCost = new ResourceAmount[] { new ResourceAmount { resourceType = goldResourceType, amount = repairCost } };

                // Check if the player can afford the repair cost
                bool canAffordRepairs = ResourceManager.instance.CanAfford(resourceAmountCost);

                if (canAffordRepairs)
                {
                    // Deduct resources and fully heal the building
                    ResourceManager.instance.SpendResources(resourceAmountCost);
                    healthSystem.HealFull();
                }
                else
                {
                    // Show tooltip if the player cannot afford the repair
                    TooltipUI.instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer { timer = 2f });
                }
            });
        }
    }
}
