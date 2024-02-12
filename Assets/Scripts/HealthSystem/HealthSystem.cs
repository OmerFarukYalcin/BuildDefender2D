using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class HealthSystem : MonoBehaviour
    {
        public event EventHandler OnHealtAmountMaxChanged;
        public event EventHandler OnDamage;
        public event EventHandler OnDied;
        public event EventHandler OnHealed;

        [SerializeField] private int healtAmountMax;
        private int healtAmount;
        void Awake()
        {
            healtAmount = healtAmountMax;
        }

        public void Damage(int damageAmount)
        {
            healtAmount -= damageAmount;
            healtAmount = Mathf.Clamp(healtAmount, 0, healtAmountMax);

            OnDamage?.Invoke(this, EventArgs.Empty);

            if (IsDead())
            {
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Heal(int healAmount)
        {
            healtAmount += healAmount;

            healtAmount = Mathf.Clamp(healtAmount, 0, healtAmountMax);

            OnHealed?.Invoke(this, EventArgs.Empty);
        }

        public void HealFull()
        {
            healtAmount = healtAmountMax;

            OnHealed?.Invoke(this, EventArgs.Empty);
        }

        public bool IsDead()
        {
            return healtAmount == 0;
        }

        public bool IsFullHealt()
        {
            return healtAmount == healtAmountMax;
        }

        public int GetHealtAmount()
        {
            return healtAmount;
        }

        public int GetHealtAmountMax()
        {
            return healtAmountMax;
        }

        public float GetHealtAmountNormalized()
        {
            return (float)healtAmount / healtAmountMax;
        }

        public void SetHealtAmountMax(int _healtAmountMax, bool updateHealtAmount)
        {
            healtAmountMax = _healtAmountMax;

            if (updateHealtAmount)
            {
                healtAmount = healtAmountMax;
            }

            OnHealtAmountMaxChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
