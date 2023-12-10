using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class Building : MonoBehaviour
    {
        private BuildingTypeSO buildingType;
        private HealthSystem healthSystem;
        private Transform buildingDemolishButton;
        void Awake()
        {
            buildingDemolishButton = transform.Find("BuildingDemolishButton");

            HideDemolishButton();

            buildingType = GetComponent<BuildingTypeHolder>().buildingType;

            healthSystem = GetComponent<HealthSystem>();

            healthSystem.SetHealtAmountMax(buildingType.healtAmountMax, true);

            healthSystem.OnDied += HandleOnDied;
        }

        private void HandleOnDied(object sender, EventArgs e)
        {
            Destroy(gameObject);
        }

        private void OnMouseEnter()
        {
            ShowDemolishButton();
        }

        private void OnMouseExit()
        {
            HideDemolishButton();
        }

        private void ShowDemolishButton()
        {
            if (buildingDemolishButton != null)
            {
                buildingDemolishButton.gameObject.SetActive(true);
            }
        }

        private void HideDemolishButton()
        {
            if (buildingDemolishButton != null)
            {
                buildingDemolishButton.gameObject.SetActive(false);
            }
        }
    }
}
