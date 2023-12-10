using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class ResourceGenerator : MonoBehaviour
    {
        public static int GetNearbyResourceAmunt(ResourceGeneratorData resourceGeneratorData, Vector3 position)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

            int nearbyResourceAmount = 0;

            foreach (Collider2D collider2D in collider2Ds)
            {
                ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();

                if (resourceNode != null)
                {
                    if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                    {
                        nearbyResourceAmount++;
                    }
                }
            }

            nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResouceAmount);
            return nearbyResourceAmount;
        }
        private ResourceGeneratorData resourceGeneratorData;
        private float timer;
        private float timerMax;
        void Awake()
        {
            resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
            timerMax = resourceGeneratorData.timerMax;
        }


        private void Start()
        {
            int nearbyResourceAmount = GetNearbyResourceAmunt(resourceGeneratorData, transform.position);

            if (nearbyResourceAmount == 0)
            {
                enabled = false;
            }
            else
            {
                timerMax = (resourceGeneratorData.timerMax / 2f) + resourceGeneratorData.timerMax * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResouceAmount);
            }

        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer += timerMax;
                ResourceManager.instance.AddResources(resourceGeneratorData.resourceType, 1);
            }
        }

        public ResourceGeneratorData GetResourceGeneratorData => resourceGeneratorData;

        public float GetTimerNormalize()
        {
            return timer / timerMax;
        }

        public float GetAmountGeneratedPerSecond()
        {
            return 1 / timerMax;
        }
    }
}
