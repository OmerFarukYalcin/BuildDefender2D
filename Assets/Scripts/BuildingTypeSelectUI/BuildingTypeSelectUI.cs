using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class BuildingTypeSelectUI : MonoBehaviour
    {
        [SerializeField] Sprite arrowSprite;
        [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;
        Transform btnTemplate;
        Transform arrowBtn;
        private BuildingTypeListSO buildingTypeList;
        private Dictionary<BuildingTypeSO, Transform> btntransformDictionary;
        void Awake()
        {

            btnTemplate = transform.Find("btnTemplate");
            btnTemplate.gameObject.SetActive(false);
            buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
            float offsetAmount = +130f;

            btntransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

            int index = 0;

            arrowBtn = Instantiate(btnTemplate, transform);
            arrowBtn.gameObject.SetActive(true);
            arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
            arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);
            Vector2 basePos = arrowBtn.GetComponent<RectTransform>().anchoredPosition;

            arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + offsetAmount * index, basePos.y);

            arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.instance.SetActiveBuildingType(null);
            });

            MouseEnterExitEvent mouseEnterExitEvent = arrowBtn.GetComponent<MouseEnterExitEvent>();

            mouseEnterExitEvent.OnMouseEnterEvent += (object sender, EventArgs e) =>
            {
                TooltipUI.instance.Show("Arrow");
            };

            mouseEnterExitEvent.OnMouseExitEvent += (object sender, EventArgs e) =>
            {
                TooltipUI.instance.Hide();
            };

            index++;

            foreach (BuildingTypeSO buildingType in buildingTypeList.list)
            {

                if (ignoreBuildingTypeList.Contains(buildingType))
                {
                    continue;
                }

                Transform btnTransform = Instantiate(btnTemplate, transform);
                btnTransform.gameObject.SetActive(true);
                btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

                btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + offsetAmount * index, basePos.y);

                btnTransform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    BuildingManager.instance.SetActiveBuildingType(buildingType);
                });

                mouseEnterExitEvent = btnTransform.GetComponent<MouseEnterExitEvent>();

                mouseEnterExitEvent.OnMouseEnterEvent += (object sender, EventArgs e) =>
                {
                    TooltipUI.instance.Show(buildingType.nameString + "\n" + buildingType.GetConstructionResouceCostString()
                    , new TooltipUI.TooltipTimer { timer = 2f });
                };

                mouseEnterExitEvent.OnMouseExitEvent += (object sender, EventArgs e) =>
                {
                    TooltipUI.instance.Hide();
                };

                btntransformDictionary[buildingType] = btnTransform;

                index++;
            }
        }

        private void Start()
        {
            BuildingManager.instance.onActiveBuildingTypeChange += HandleOnActiveBuildingTypeChange;
            UpdateActiveBuildingTypeButton();
        }

        private void HandleOnActiveBuildingTypeChange(object sender, BuildingManager.OnActiveBuildingTypeChangeEventHandler e)
        {
            UpdateActiveBuildingTypeButton();
        }

        private void UpdateActiveBuildingTypeButton()
        {
            arrowBtn.Find("selected").gameObject.SetActive(false);
            foreach (BuildingTypeSO buildingType in btntransformDictionary.Keys)
            {
                Transform btnTransform = btntransformDictionary[buildingType];
                btnTransform.Find("selected").gameObject.SetActive(false);
            }
            BuildingTypeSO activeBuildingType = BuildingManager.instance.GetActiveBuildingType;
            if (activeBuildingType == null)
            {
                arrowBtn.Find("selected").gameObject.SetActive(true);
            }
            else
            {
                btntransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
            }
        }
    }
}
