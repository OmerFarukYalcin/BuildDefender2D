using System;
using UnityEngine;

namespace BuilderDefender
{
    public class HealtBar : MonoBehaviour
    {
        [SerializeField] HealthSystem healthSystem;

        private Transform barTransform;
        private Transform seperatorContainer;

        private void Awake()
        {
            barTransform = transform.Find("bar");

            seperatorContainer = transform.Find("seperatorContainer");
        }

        private void Start()
        {
            ConstructHealtBarSeperator();

            healthSystem.OnDamage += HandleOnDamage;
            healthSystem.OnHealtAmountMaxChanged += HandleOnHealtAmountMaxChanged;
            healthSystem.OnHealed += HandleOnHealed;

            UpdateBar();

            UpdateHealtBarVisible();
        }

        private void HandleOnHealtAmountMaxChanged(object sender, EventArgs e)
        {
            ConstructHealtBarSeperator();
        }

        private void HandleOnHealed(object sender, EventArgs e)
        {
            UpdateBar();
            UpdateHealtBarVisible();
        }

        private void HandleOnDamage(object sender, EventArgs e)
        {
            UpdateBar();
            UpdateHealtBarVisible();
        }

        private void ConstructHealtBarSeperator()
        {
            Transform seperatorTemplate = seperatorContainer.Find("seperatorTemplate");

            seperatorTemplate.gameObject.SetActive(false);

            foreach (Transform seperatorTransform in seperatorContainer)
            {
                if (seperatorTransform == seperatorTemplate) continue;
                Destroy(seperatorTransform.gameObject);
            }

            int healtAmounPerSeperator = 10;

            int healtSeperatorCount = Mathf.FloorToInt(healthSystem.GetHealtAmountMax() / healtAmounPerSeperator);

            float barSize = 3f;

            float barOneHealtAmountSize = barSize / healthSystem.GetHealtAmountMax();

            for (int i = 1; i < healtSeperatorCount; i++)
            {
                Transform seperatorTransform = Instantiate(seperatorTemplate, seperatorContainer);
                seperatorTransform.gameObject.SetActive(true);
                seperatorTransform.localPosition = new Vector3(barOneHealtAmountSize * i * healtAmounPerSeperator, 0, 0);
            }
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
            gameObject.SetActive(true);
        }
    }
}
