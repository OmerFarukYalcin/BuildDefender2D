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
        private Transform buildingRepairButton;
        void Awake()
        {
            buildingDemolishButton = transform.Find("BuildingDemolishButton");

            buildingRepairButton = transform.Find("BuildingRepairButton");

            HideRepairButton();

            HideDemolishButton();

            buildingType = GetComponent<BuildingTypeHolder>().buildingType;

            healthSystem = GetComponent<HealthSystem>();

            healthSystem.SetHealtAmountMax(buildingType.healtAmountMax, true);

            healthSystem.OnDied += HandleOnDied;

            healthSystem.OnHealed += HandleOnHealed;

            healthSystem.OnDamage += HandleOnDamage;
        }

        private void HandleOnHealed(object sender, EventArgs e)
        {
            if (healthSystem.IsFullHealt())
            {
                HideRepairButton();
            }
        }

        private void HandleOnDamage(object sender, EventArgs e)
        {
            ShowRepairButton();

            SoundManager.instance.PlaySound(SoundManager.Sound.BuildingDamaged);

            CinemachineShake.instance.ShakeCamera(7f, .15f);

            ChromaticAberration.instance.SetWeight(1f);
        }

        private void HandleOnDied(object sender, EventArgs e)
        {
            Instantiate(Resources.Load<Transform>("pfBuildingDestroyedParticles"), transform.position, Quaternion.identity);
            SoundManager.instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
            CinemachineShake.instance.ShakeCamera(10f, .2f);
            ChromaticAberration.instance.SetWeight(1f);
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

        private void ShowRepairButton()
        {
            if (buildingRepairButton != null)
            {
                buildingRepairButton.gameObject.SetActive(true);
            }
        }

        private void HideRepairButton()
        {
            if (buildingRepairButton != null)
            {
                buildingRepairButton.gameObject.SetActive(false);
            }
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
