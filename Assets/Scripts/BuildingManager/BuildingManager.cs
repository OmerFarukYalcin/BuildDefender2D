using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuilderDefender
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager instance { get; private set; }
        private Camera mainCamera;
        private BuildingTypeSO activeBuildingType;
        private BuildingTypeListSO buildingTypeList;
        [SerializeField] private Building hqBuilding;
        public event EventHandler<OnActiveBuildingTypeChangeEventHandler> onActiveBuildingTypeChange;

        public class OnActiveBuildingTypeChangeEventHandler : EventArgs
        {
            public BuildingTypeSO activeBuildingType;
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);
            buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        }

        void Start()
        {
            mainCamera = Camera.main;

            hqBuilding.GetComponent<HealthSystem>().OnDied += HandleOnHqDied;
        }

        private void HandleOnHqDied(object sender, EventArgs e)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.GameOver);
            GameOverUI.instance.Show();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (activeBuildingType != null)
                {
                    if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
                    {
                        if (ResourceManager.instance.CanAfford(activeBuildingType.constructionResouceCostArray))
                        {
                            ResourceManager.instance.SpendResources(activeBuildingType.constructionResouceCostArray);
                            SoundManager.instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                            BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                            // Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        }
                        else
                        {
                            TooltipUI.instance.Show("Cannot afford: " + activeBuildingType.GetConstructionResouceCostString(), new TooltipUI.TooltipTimer { timer = 2f });
                        }
                    }
                    else
                    {
                        TooltipUI.instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f });
                    }
                }
            }

        }

        public void SetActiveBuildingType(BuildingTypeSO buildingType)
        {
            activeBuildingType = buildingType;

            onActiveBuildingTypeChange?.Invoke(this, new OnActiveBuildingTypeChangeEventHandler { activeBuildingType = activeBuildingType });
        }

        private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
        {
            BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

            bool isAreaClear = collider2Ds.Length == 0;

            if (!isAreaClear)
            {
                errorMessage = "Area is not clear!";
                return false;
            }

            collider2Ds = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

            foreach (Collider2D collider2D in collider2Ds)
            {
                BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
                if (buildingTypeHolder != null)
                {
                    if (buildingTypeHolder.buildingType == buildingType)
                    {
                        errorMessage = "Too close to another building of the same type!";
                        return false;
                    }
                }
            }
            if (buildingType.hasResourceGeneratorData)
            {
                ResourceGeneratorData resourceGeneratorData = buildingType.resourceGeneratorData;

                int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmunt(resourceGeneratorData, position);

                if (nearbyResourceAmount == 0)
                {
                    errorMessage = "There are no nearby Resource Nodes!";
                    return false;
                }
            }

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

        public BuildingTypeSO GetActiveBuildingType => activeBuildingType;

        public Building GetHQBuilding()
        {
            return hqBuilding;
        }
    }
}
