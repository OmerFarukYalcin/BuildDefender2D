using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class BuildingGhost : MonoBehaviour
    {
        private GameObject spriteGameObject;

        private ResourceNearbyOverlay resourceNearbyOverlay;

        private void Awake()
        {
            spriteGameObject = transform.Find("sprite").gameObject;

            resourceNearbyOverlay = transform.Find("ResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();

            Hide();
        }

        private void Start()
        {
            BuildingManager.instance.onActiveBuildingTypeChange += HandleOnActiveBuildingTypeChange;
        }

        private void HandleOnActiveBuildingTypeChange(object sender, BuildingManager.OnActiveBuildingTypeChangeEventHandler e)
        {
            if (e.activeBuildingType == null)
            {
                Hide();
                resourceNearbyOverlay.Hide();
            }
            else
            {
                Show(e.activeBuildingType.sprite);
                if (e.activeBuildingType.hasResourceGeneratorData)
                    resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
                else
                    resourceNearbyOverlay.Hide();
            }

        }

        private void Update()
        {
            transform.position = UtilsClass.GetMouseWorldPosition();
        }

        private void Show(Sprite ghostSprite)
        {
            spriteGameObject.SetActive(true);
            spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
        }

        private void Hide()
        {
            spriteGameObject.SetActive(false);
        }
    }
}
