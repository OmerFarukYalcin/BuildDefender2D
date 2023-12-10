using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class BuildingDemolishButton : MonoBehaviour
    {
        [SerializeField] Building building;
        private void Awake()
        {
            transform.Find("button").GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingTypeSO buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;

                foreach (ResourceAmount resourceAmount in buildingType.constructionResouceCostArray)
                {
                    ResourceManager.instance.AddResources(resourceAmount.resourceType, Mathf.FloorToInt(resourceAmount.amount * 0.6f));
                }

                Destroy(building.gameObject);
            });
        }
    }
}
