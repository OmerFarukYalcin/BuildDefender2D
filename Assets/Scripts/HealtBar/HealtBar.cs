using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class HealtBar : MonoBehaviour
    {
        [SerializeField] HealthSystem healthSystem;

        private Transform barTransform;

        private void Awake()
        {
            barTransform = transform.Find("bar");
        }

        private void Start()
        {
            healthSystem.OnDamage += HandleOnDamage;
            UpdateBar();
            UpdateHealtBarVisible();
        }

        private void HandleOnDamage(object sender, EventArgs e)
        {
            UpdateBar();
            UpdateHealtBarVisible();
        }

        private void UpdateBar()
        {
            barTransform.localScale = new Vector3(healthSystem.GetHealtAmountNormalized(), 1, 1);
        }

        private void UpdateHealtBarVisible()
        {
            if (healthSystem.IsFullHealt())
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}
