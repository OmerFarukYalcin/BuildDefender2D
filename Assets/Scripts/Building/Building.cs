using System;
using UnityEngine;

namespace BuilderDefender
{
    public class Building : MonoBehaviour
    {
        // Fields for building's type and health system
        private BuildingTypeSO buildingType;
        private HealthSystem healthSystem;

        // UI elements for building controls (demolish and repair buttons)
        private Transform buildingDemolishButton;
        private Transform buildingRepairButton;

        void Awake()
        {
            // Find buttons in the building's hierarchy
            buildingDemolishButton = transform.Find("BuildingDemolishButton");
            buildingRepairButton = transform.Find("BuildingRepairButton");

            // Initially hide the buttons
            HideRepairButton();
            HideDemolishButton();

            // Get building type and health system from attached components
            buildingType = GetComponent<BuildingTypeHolder>().buildingType;
            healthSystem = GetComponent<HealthSystem>();

            // Set the initial health based on building type
            healthSystem.SetHealthAmountMax(buildingType.healtAmountMax, true);

            // Subscribe to health system events
            healthSystem.OnDied += HandleOnDied;
            healthSystem.OnHealed += HandleOnHealed;
            healthSystem.OnDamage += HandleOnDamage;
        }

        // Event handler when the building is fully healed
        private void HandleOnHealed(object sender, EventArgs e)
        {
            if (healthSystem.IsFullHealth())
            {
                // Hide repair button if the building is at full health
                HideRepairButton();
            }
        }

        // Event handler when the building takes damage
        private void HandleOnDamage(object sender, EventArgs e)
        {
            // Show repair button when damaged
            ShowRepairButton();

            // Play damaged sound and trigger camera shake
            SoundManager.instance.PlaySound(SoundManager.Sound.BuildingDamaged);
            CinemachineShake.instance.ShakeCamera(7f, .15f);

            // Apply visual effect
            ChromaticAberration.instance.SetWeight(1f);
        }

        // Event handler when the building is destroyed
        private void HandleOnDied(object sender, EventArgs e)
        {
            // Spawn destruction particles, play sound, and trigger camera shake
            Instantiate(Resources.Load<Transform>("pfBuildingDestroyedParticles"), transform.position, Quaternion.identity);
            SoundManager.instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
            CinemachineShake.instance.ShakeCamera(10f, .2f);

            // Apply a visual effect and destroy the building object
            ChromaticAberration.instance.SetWeight(1f);
            Destroy(gameObject);
        }

        // Mouse event handlers to show/hide demolish button
        private void OnMouseEnter()
        {
            ShowDemolishButton();
        }

        private void OnMouseExit()
        {
            HideDemolishButton();
        }

        // Method to show the repair button
        private void ShowRepairButton()
        {
            if (buildingRepairButton != null)
            {
                buildingRepairButton.gameObject.SetActive(true);
            }
        }

        // Method to hide the repair button
        private void HideRepairButton()
        {
            if (buildingRepairButton != null)
            {
                buildingRepairButton.gameObject.SetActive(false);
            }
        }

        // Method to show the demolish button
        private void ShowDemolishButton()
        {
            if (buildingDemolishButton != null)
            {
                buildingDemolishButton.gameObject.SetActive(true);
            }
        }

        // Method to hide the demolish button
        private void HideDemolishButton()
        {
            if (buildingDemolishButton != null)
            {
                buildingDemolishButton.gameObject.SetActive(false);
            }
        }
    }
}
