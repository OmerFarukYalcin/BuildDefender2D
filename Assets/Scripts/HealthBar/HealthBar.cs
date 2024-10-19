using System;
using UnityEngine;

namespace BuilderDefender
{
    public class HealthBar : MonoBehaviour
    {
        // Reference to the HealthSystem, which tracks the health of the object this health bar belongs to
        [SerializeField] HealthSystem healthSystem;

        // Reference to the transform of the health bar itself
        private Transform barTransform;

        // Container for health bar separators (visual indicators between health units)
        private Transform separatorContainer;

        // Initialize references to components
        private void Awake()
        {
            // Find the health bar fill element (represented by a "bar" child object)
            barTransform = transform.Find("bar");

            // Find the container for health separators (child objects that show segments of health)
            separatorContainer = transform.Find("separatorContainer");
        }

        private void Start()
        {
            // Construct the separators on the health bar (showing divisions between health units)
            ConstructHealthBarSeparator();

            // Subscribe to health system events (damage, max health change, healing)
            healthSystem.OnDamage += HandleOnDamage;
            healthSystem.OnHealthAmountMaxChanged += HandleOnHealthAmountMaxChanged;
            healthSystem.OnHealed += HandleOnHealed;

            // Initialize the health bar to reflect current health state
            UpdateBar();

            // Update visibility of the health bar depending on whether the object has full health
            UpdateHealthBarVisible();
        }

        // Event handler for when the max health changes (e.g., from upgrades)
        private void HandleOnHealthAmountMaxChanged(object sender, EventArgs e)
        {
            // Rebuild the health bar separators to reflect the new max health
            ConstructHealthBarSeparator();
        }

        // Event handler for when the object is healed
        private void HandleOnHealed(object sender, EventArgs e)
        {
            // Update the health bar fill and its visibility
            UpdateBar();
            UpdateHealthBarVisible();
        }

        // Event handler for when the object takes damage
        private void HandleOnDamage(object sender, EventArgs e)
        {
            // Update the health bar fill and its visibility
            UpdateBar();
            UpdateHealthBarVisible();
        }

        // Constructs separators on the health bar to divide it into sections based on health units
        private void ConstructHealthBarSeparator()
        {
            // Find the template used for creating separators
            Transform separatorTemplate = separatorContainer.Find("separatorTemplate");

            // Hide the template itself (it is only used to spawn new separators)
            separatorTemplate.gameObject.SetActive(false);

            // Remove any existing separators from previous health configurations
            foreach (Transform separatorTransform in separatorContainer)
            {
                if (separatorTransform == separatorTemplate) continue;
                Destroy(separatorTransform.gameObject); // Remove old separators
            }

            // Define the amount of health per separator (e.g., each separator represents 10 health)
            int healthAmountPerSeparator = 10;

            // Calculate how many separators are needed based on the maximum health
            int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);

            // Define the total size of the health bar and the size per health unit
            float barSize = 3f;
            float barOneHealthAmountSize = barSize / healthSystem.GetHealthAmountMax();

            // Create new separators along the health bar based on the number of health units
            for (int i = 1; i < healthSeparatorCount; i++)
            {
                Transform separatorTransform = Instantiate(separatorTemplate, separatorContainer);
                separatorTransform.gameObject.SetActive(true);
                // Position the separator along the health bar based on its corresponding health unit
                separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeparator, 0, 0);
            }
        }

        // Updates the health bar's size (scaling) based on the object's current health
        private void UpdateBar()
        {
            // Scale the health bar's X-axis to reflect the normalized health value (0 to 1)
            barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
        }

        // Controls whether the health bar is visible based on the object's current health status
        private void UpdateHealthBarVisible()
        {
            // Hide the health bar if the object is at full health
            if (healthSystem.IsFullHealth())
            {
                gameObject.SetActive(false);
            }
            else
            {
                // Show the health bar if the object has taken damage
                gameObject.SetActive(true);
            }

            gameObject.SetActive(true);
        }
    }
}
