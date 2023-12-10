using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class CostructionTimerUI : MonoBehaviour
    {
        [SerializeField] private BuildingConstruction buildingConstruction;
        private Image constructionProgressImage;
        void Awake()
        {
            constructionProgressImage = transform.Find("mask").Find("image").GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            constructionProgressImage.fillAmount = buildingConstruction.GetConstructionTimerNormalize();
        }
    }
}
