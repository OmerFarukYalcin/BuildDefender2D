using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class ResourcesUI : MonoBehaviour
    {
        private ResourceTypeListSO resourceTypeList;
        private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;
        void Awake()
        {
            resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
            resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();
            Transform resourceTemplate = transform.Find("ResourceTemplate");
            resourceTemplate.gameObject.SetActive(false);
            float offsetAmount = -160f;
            int index = 0;
            foreach (ResourceTypeSO resourceType in resourceTypeList.list)
            {
                Transform resourceTransform = Instantiate(resourceTemplate, transform);
                Vector2 basePos = resourceTransform.GetComponent<RectTransform>().anchoredPosition;
                resourceTransform.gameObject.SetActive(true);

                resourceTransform.transform.Find("Image").GetComponent<Image>().sprite = resourceType.sprite;

                resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + offsetAmount * index, basePos.y);

                resourceTypeTransformDictionary[resourceType] = resourceTransform;

                index += 1;
            }
        }

        private void Start()
        {
            ResourceManager.instance.onResourceAmountChange += HandleOnResourceAmountChange;
            UpdateResourceAmount();
        }

        private void HandleOnResourceAmountChange(object sender, EventArgs e)
        {
            UpdateResourceAmount();
        }

        // Update is called once per frame
        private void UpdateResourceAmount()
        {
            foreach (ResourceTypeSO resourceType in resourceTypeList.list)
            {
                int resourceAmount = ResourceManager.instance.GetResourceAmount(resourceType);
                Transform resourceTransform = resourceTypeTransformDictionary[resourceType];
                resourceTransform.transform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
            }
        }
    }
}
