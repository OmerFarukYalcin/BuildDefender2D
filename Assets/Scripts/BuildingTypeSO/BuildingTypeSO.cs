using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    [CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
    public class BuildingTypeSO : ScriptableObject
    {
        public string nameString;
        public Transform prefab;
        public ResourceGeneratorData resourceGeneratorData;
        public Sprite sprite;
        public bool hasResourceGeneratorData;
        public float minConstructionRadius;
        public ResourceAmount[] constructionResouceCostArray;
        public int healtAmountMax;
        public float constructionTimerMax;

        public string GetConstructionResouceCostString()
        {
            string cost = "";
            foreach (ResourceAmount resouceAmount in constructionResouceCostArray)
            {
                cost += $"<color=#{resouceAmount.resourceType.colorHex}>" + resouceAmount.resourceType.nameShort +
                resouceAmount.amount + "</color>";
            }
            return cost;
        }
    }
}
