using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BuilderDefender
{
    public class ResourceNearbyOverlay : MonoBehaviour
    {
        private ResourceGeneratorData resourceGeneratorData;

        private void Awake()
        {
            Hide();
        }

        private void Update()
        {
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmunt(resourceGeneratorData, transform.position);

            float percent = Mathf.Round((float)nearbyResourceAmount / resourceGeneratorData.maxResouceAmount * 100f);

            transform.Find("text").GetComponent<TextMeshPro>().SetText($"{percent}%");
        }

        public void Show(ResourceGeneratorData _resourceGeneratorData)
        {
            resourceGeneratorData = _resourceGeneratorData;

            gameObject.SetActive(true);

            transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;

        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
