using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class BuildingConstruction : MonoBehaviour
    {
        public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingTypeSO)
        {
            Transform buildingConstructionGo = Resources.Load<Transform>("BuildingConstruction");
            Transform buildingConstructionTranform = Instantiate(buildingConstructionGo, position, Quaternion.identity);
            BuildingConstruction buildingConstruction = buildingConstructionTranform.GetComponent<BuildingConstruction>();
            buildingConstruction.SetBuildingType(buildingTypeSO);
            return buildingConstruction;
        }
        private BuildingTypeSO buildingType;
        private float constructionTimer;
        private float constructionTimerMax;
        private BoxCollider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private BuildingTypeHolder buildingTypeHolder;
        private Material constructionMaterial;

        void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
            buildingTypeHolder = GetComponent<BuildingTypeHolder>();
            constructionMaterial = spriteRenderer.material;

            Instantiate(Resources.Load<Transform>("pfBuildingPlacedParticles"), transform.position, Quaternion.identity);
        }

        void Update()
        {
            constructionTimer -= Time.deltaTime;

            constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalize());
            if (constructionTimer <= 0f)
            {
                print("ding!");
                Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
                Instantiate(Resources.Load<Transform>("pfBuildingPlacedParticles"), transform.position, Quaternion.identity);
                SoundManager.instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                Destroy(gameObject);
            }
        }

        private void SetBuildingType(BuildingTypeSO _buildingTypeSO)
        {
            constructionTimerMax = _buildingTypeSO.constructionTimerMax;

            constructionTimer = constructionTimerMax;

            buildingType = _buildingTypeSO;

            spriteRenderer.sprite = _buildingTypeSO.sprite;

            boxCollider2D.offset = _buildingTypeSO.prefab.GetComponent<BoxCollider2D>().offset;
            boxCollider2D.size = _buildingTypeSO.prefab.GetComponent<BoxCollider2D>().size;

            buildingTypeHolder.buildingType = buildingType;
        }

        public float GetConstructionTimerNormalize()
        {
            return 1 - constructionTimer / constructionTimerMax;
        }
    }
}
