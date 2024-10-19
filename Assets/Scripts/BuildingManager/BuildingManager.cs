using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuilderDefender
{
    public class BuildingManager : MonoBehaviour
    {
        // Singleton instance of BuildingManager
        public static BuildingManager instance { get; private set; }

        // Camera and building references
        private Camera mainCamera;
        private BuildingTypeSO activeBuildingType;
        private BuildingTypeListSO buildingTypeList;

        // Reference to the HQ building
        [SerializeField] private Building hqBuilding;

        // Event triggered when the active building type changes
        public event EventHandler<OnActiveBuildingTypeChangeEventHandler> onActiveBuildingTypeChange;

        // Custom event handler class for active building type change
        public class OnActiveBuildingTypeChangeEventHandler : EventArgs
        {
            public BuildingTypeSO activeBuildingType;
        }

        private void Awake()
        {
            // Set up the singleton instance
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);

            // Load the list of building types
            buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        }

        private void Start()
        {
            // Get the main camera
            mainCamera = Camera.main;

            // Subscribe to the HQ building's death event
            hqBuilding.GetComponent<HealthSystem>().OnDied += HandleOnHqDied;
        }

        // Event handler for HQ building's destruction
        private void HandleOnHqDied(object sender, EventArgs e)
        {
            // Play game over sound and show game over UI
            SoundManager.instance.PlaySound(SoundManager.Sound.GameOver);
            GameOverUI.instance.Show();
        }

        private void Update()
        {
            // Handle mouse click to place a building
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (activeBuildingType != null)
                {
                    // Check if the building can be placed at the mouse position
                    if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
                    {
                        // Check if the player can afford the building
                        if (ResourceManager.instance.CanAfford(activeBuildingType.constructionResouceCostArray))
                        {
                            // Deduct resources and place the building
                            ResourceManager.instance.SpendResources(activeBuildingType.constructionResouceCostArray);
                            SoundManager.instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                            BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                        }
                        else
                        {
                            // Show tooltip if player cannot afford the building
                            TooltipUI.instance.Show("Cannot afford: " + activeBuildingType.GetConstructionResouceCostString(), new TooltipUI.TooltipTimer { timer = 2f });
                        }
                    }
                    else
                    {
                        // Show tooltip with error message
                        TooltipUI.instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f });
                    }
                }
            }
        }

        // Method to set the active building type
        public void SetActiveBuildingType(BuildingTypeSO buildingType)
        {
            activeBuildingType = buildingType;

            // Trigger the event for active building type change
            onActiveBuildingTypeChange?.Invoke(this, new OnActiveBuildingTypeChangeEventHandler { activeBuildingType = activeBuildingType });
        }

        // Method to check if a building can be placed at the given position
        private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
        {
            // Check for collisions in the area where the building will be placed
            BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
            bool isAreaClear = collider2Ds.Length == 0;

            if (!isAreaClear)
            {
                errorMessage = "Area is not clear!";
                return false;
            }

            // Check if the building is too close to another of the same type
            collider2Ds = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
            foreach (Collider2D collider2D in collider2Ds)
            {
                BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
                if (buildingTypeHolder != null && buildingTypeHolder.buildingType == buildingType)
                {
                    errorMessage = "Too close to another building of the same type!";
                    return false;
                }
            }

            // If the building has resource generator data, ensure it's near a resource node
            if (buildingType.hasResourceGeneratorData)
            {
                ResourceGeneratorData resourceGeneratorData = buildingType.resourceGeneratorData;
                int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, position);

                if (nearbyResourceAmount == 0)
                {
                    errorMessage = "There are no nearby Resource Nodes!";
                    return false;
                }
            }

            // Ensure the building is within a reasonable distance from another building
            float maxConstructionRadius = 25f;
            collider2Ds = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
            foreach (Collider2D collider2D in collider2Ds)
            {
                BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
                if (buildingTypeHolder != null)
                {
                    errorMessage = "";
                    return true;
                }
            }
            errorMessage = "Too far from any other building!";
            return false;
        }

        // Getter for the currently active building type
        public BuildingTypeSO GetActiveBuildingType => activeBuildingType;

        // Getter for the HQ building
        public Building GetHQBuilding()
        {
            return hqBuilding;
        }
    }
}
