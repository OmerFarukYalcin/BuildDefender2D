using System;
using UnityEngine;

namespace BuilderDefender
{
    public class HealthSystem : MonoBehaviour
    {
        // Events for various health-related actions
        public event EventHandler OnHealthAmountMaxChanged;  // Fired when max health changes
        public event EventHandler OnDamage;  // Fired when damage is taken
        public event EventHandler OnDied;    // Fired when the entity dies
        public event EventHandler OnHealed;  // Fired when healing occurs

        // Serialized field to set max health in the Unity editor
        [SerializeField] private int healthAmountMax;

        // Current health amount
        private int healthAmount;

        private void Awake()
        {
            // Initialize current health to the maximum value at the start
            healthAmount = healthAmountMax;
        }

        // Method to apply damage to the health system
        public void Damage(int damageAmount)
        {
            // Reduce health by the damage amount, ensuring it doesn't go below 0
            healthAmount -= damageAmount;
            healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

            // Trigger the OnDamage event
            OnDamage?.Invoke(this, EventArgs.Empty);

            // Check if the entity is dead, and trigger the OnDied event if necessary
            if (IsDead())
            {
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }

        // Method to heal the entity by a specified amount
        public void Heal(int healAmount)
        {
            // Increase health by the healing amount, ensuring it doesn't exceed max health
            healthAmount += healAmount;
            healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

            // Trigger the OnHealed event
            OnHealed?.Invoke(this, EventArgs.Empty);
        }

        // Method to fully heal the entity
        public void HealFull()
        {
            // Set current health to the maximum health
            healthAmount = healthAmountMax;

            // Trigger the OnHealed event
            OnHealed?.Invoke(this, EventArgs.Empty);
        }

        // Method to check if the entity is dead (health reaches 0)
        public bool IsDead()
        {
            return healthAmount == 0;
        }

        // Method to check if the entity is at full health
        public bool IsFullHealth()
        {
            return healthAmount == healthAmountMax;
        }

        // Getter for the current health amount
        public int GetHealthAmount()
        {
            return healthAmount;
        }

        // Getter for the maximum health amount
        public int GetHealthAmountMax()
        {
            return healthAmountMax;
        }

        // Returns the current health as a normalized value (0 to 1 range)
        public float GetHealthAmountNormalized()
        {
            return (float)healthAmount / healthAmountMax;
        }

        // Method to set the maximum health and optionally update the current health to match
        public void SetHealthAmountMax(int newHealthAmountMax, bool updateHealthAmount)
        {
            healthAmountMax = newHealthAmountMax;

            // If true, also set current health to the new maximum
            if (updateHealthAmount)
            {
                healthAmount = healthAmountMax;
            }

            // Trigger the OnHealthAmountMaxChanged event
            OnHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
