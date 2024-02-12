using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class BuildingRepairBtn : MonoBehaviour
    {
        [SerializeField] HealthSystem healthSystem;
        [SerializeField] ResourceTypeSO goldResourceType;
        private void Awake()
        {
            transform.Find("button").GetComponent<Button>().onClick.AddListener(() =>
            {
                int missingHealt = healthSystem.GetHealtAmountMax() - healthSystem.GetHealtAmount();
                int repairCost = missingHealt / 2;

                ResourceAmount[] resourceAmountCost = new ResourceAmount[] { new ResourceAmount { resourceType = goldResourceType, amount = repairCost } };

                bool canAffordRepairs = ResourceManager.instance.CanAfford(resourceAmountCost);

                if (canAffordRepairs)
                {
                    ResourceManager.instance.SpendResources(resourceAmountCost);
                    healthSystem.HealFull();
                }
                else
                {
                    TooltipUI.instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer { timer = 2f });
                }
            });
        }
    }
}
