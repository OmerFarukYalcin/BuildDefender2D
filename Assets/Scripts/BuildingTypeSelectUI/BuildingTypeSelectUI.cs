using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class BuildingTypeSelectUI : MonoBehaviour
    {
        // Sprite for the arrow button that resets building selection
        [SerializeField] private Sprite arrowSprite;

        // List of building types to ignore when creating buttons
        [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;

        // References to button template and arrow button
        private Transform btnTemplate;
        private Transform arrowBtn;

        // Reference to the list of all building types
        private BuildingTypeListSO buildingTypeList;

        // Dictionary to store button transforms for each building type
        private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;

        void Awake()
        {
            // Find the button template and hide it (used to clone for each building type)
            btnTemplate = transform.Find("btnTemplate");
            btnTemplate.gameObject.SetActive(false);

            // Load the building type list from resources
            buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

            // Offset for button placement
            float offsetAmount = 130f;

            // Initialize the button transform dictionary
            btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

            int index = 0;

            // Create the arrow button (used to deselect active building)
            arrowBtn = Instantiate(btnTemplate, transform);
            arrowBtn.gameObject.SetActive(true);
            arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
            arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);
            Vector2 basePos = arrowBtn.GetComponent<RectTransform>().anchoredPosition;

            arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + offsetAmount * index, basePos.y);

            // Add listener to arrow button to clear active building type
            arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.instance.SetActiveBuildingType(null);
            });

            // Add tooltip for arrow button
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

            // Create a button for each building type
            foreach (BuildingTypeSO buildingType in buildingTypeList.list)
            {
                // Skip ignored building types
                if (ignoreBuildingTypeList.Contains(buildingType))
                {
                    continue;
                }

                // Instantiate a new button for the building type
                Transform btnTransform = Instantiate(btnTemplate, transform);
                btnTransform.gameObject.SetActive(true);
                btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

                btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + offsetAmount * index, basePos.y);

                // Add listener to set active building type when clicked
                btnTransform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    BuildingManager.instance.SetActiveBuildingType(buildingType);
                });

                // Add tooltip for each building type button
                mouseEnterExitEvent = btnTransform.GetComponent<MouseEnterExitEvent>();

                mouseEnterExitEvent.OnMouseEnterEvent += (object sender, EventArgs e) =>
                {
                    TooltipUI.instance.Show(buildingType.nameString + "\n" + buildingType.GetConstructionResouceCostString(),
                        new TooltipUI.TooltipTimer { timer = 2f });
                };

                mouseEnterExitEvent.OnMouseExitEvent += (object sender, EventArgs e) =>
                {
                    TooltipUI.instance.Hide();
                };

                // Store the button in the dictionary for later reference
                btnTransformDictionary[buildingType] = btnTransform;

                index++;
            }
        }

        private void Start()
        {
            // Subscribe to the event when the active building type changes
            BuildingManager.instance.onActiveBuildingTypeChange += HandleOnActiveBuildingTypeChange;

            // Update the UI to reflect the current active building type
            UpdateActiveBuildingTypeButton();
        }

        // Event handler for when the active building type changes
        private void HandleOnActiveBuildingTypeChange(object sender, BuildingManager.OnActiveBuildingTypeChangeEventHandler e)
        {
            UpdateActiveBuildingTypeButton();
        }

        // Update the UI to show which building type is currently selected
        private void UpdateActiveBuildingTypeButton()
        {
            // Deselect all buttons
            arrowBtn.Find("selected").gameObject.SetActive(false);
            foreach (BuildingTypeSO buildingType in btnTransformDictionary.Keys)
            {
                Transform btnTransform = btnTransformDictionary[buildingType];
                btnTransform.Find("selected").gameObject.SetActive(false);
            }

            // Highlight the currently selected building type button
            BuildingTypeSO activeBuildingType = BuildingManager.instance.GetActiveBuildingType;
            if (activeBuildingType == null)
            {
                arrowBtn.Find("selected").gameObject.SetActive(true);
            }
            else
            {
                btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
            }
        }
    }
}
