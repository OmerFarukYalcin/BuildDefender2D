using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace BuilderDefender
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] List<ResourceAmount> startingResourceAmountList;
        public event EventHandler onResourceAmountChange;
        public static ResourceManager instance { get; private set; }
        private Dictionary<ResourceTypeSO, int> resourcesAmountDictionaray;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);
            resourcesAmountDictionaray = new Dictionary<ResourceTypeSO, int>();

            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

            foreach (ResourceTypeSO resourceType in resourceTypeList.list)
            {
                resourcesAmountDictionaray[resourceType] = 0;
            }

            foreach (ResourceAmount resourceAmount in startingResourceAmountList)
            {
                AddResources(resourceAmount.resourceType, resourceAmount.amount);
            }
        }


        public void AddResources(ResourceTypeSO resourceType, int amount)
        {
            resourcesAmountDictionaray[resourceType] += amount;

            onResourceAmountChange?.Invoke(this, EventArgs.Empty);

            TestLogResourceAmountDictionaray();
        }

        public int GetResourceAmount(ResourceTypeSO resourceType)
        {
            return resourcesAmountDictionaray[resourceType];
        }

        private void TestLogResourceAmountDictionaray()
        {
            foreach (ResourceTypeSO resourceType in resourcesAmountDictionaray.Keys)
            {
                Debug.Log(resourceType.nameString + ": " + resourcesAmountDictionaray[resourceType]);
            }
        }

        public bool CanAfford(ResourceAmount[] resouceAmounts)
        {
            foreach (ResourceAmount resouceAmount in resouceAmounts)
            {
                if (GetResourceAmount(resouceAmount.resourceType) >= resouceAmount.amount)
                {
                    //#
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public void SpendResources(ResourceAmount[] resouceAmounts)
        {
            foreach (ResourceAmount resouceAmount in resouceAmounts)
            {
                resourcesAmountDictionaray[resouceAmount.resourceType] -= resouceAmount.amount;
            }

        }
    }
}
