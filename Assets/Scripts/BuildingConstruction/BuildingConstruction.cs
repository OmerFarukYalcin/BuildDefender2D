using UnityEngine;

namespace BuilderDefender
{
    public class BuildingConstruction : MonoBehaviour
    {
        // Static method to create a new BuildingConstruction instance at a specified position
        public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingTypeSO)
        {
            // Load the building construction prefab and instantiate it at the given position
            Transform buildingConstructionGo = Resources.Load<Transform>("BuildingConstruction");
            Transform buildingConstructionTransform = Instantiate(buildingConstructionGo, position, Quaternion.identity);

            // Get the BuildingConstruction component and set its building type
            BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
            buildingConstruction.SetBuildingType(buildingTypeSO);

            return buildingConstruction;
        }

        // Fields for the building's construction process and related components
        private BuildingTypeSO buildingType;
        private float constructionTimer;
        private float constructionTimerMax;
        private BoxCollider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private BuildingTypeHolder buildingTypeHolder;
        private Material constructionMaterial;

        // Initialize components and set up the building construction
        void Awake()
        {
            // Get required components for the building
            boxCollider2D = GetComponent<BoxCollider2D>();
            spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
            buildingTypeHolder = GetComponent<BuildingTypeHolder>();
            constructionMaterial = spriteRenderer.material;

            // Instantiate building placed particles for visual feedback
            Instantiate(Resources.Load<Transform>("pfBuildingPlacedParticles"), transform.position, Quaternion.identity);
        }

        // Update method for tracking construction progress
        void Update()
        {
            // Decrease the construction timer
            constructionTimer -= Time.deltaTime;

            // Update the material's construction progress for visual feedback
            constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalize());

            // Check if construction is complete
            if (constructionTimer <= 0f)
            {
                // Instantiate the completed building and play feedback effects
                print("ding!");
                Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
                Instantiate(Resources.Load<Transform>("pfBuildingPlacedParticles"), transform.position, Quaternion.identity);
                SoundManager.instance.PlaySound(SoundManager.Sound.BuildingPlaced);

                // Destroy the temporary construction object
                Destroy(gameObject);
            }
        }

        // Method to set the building type and initialize its properties
        private void SetBuildingType(BuildingTypeSO _buildingTypeSO)
        {
            // Set the construction time and start the timer
            constructionTimerMax = _buildingTypeSO.constructionTimerMax;
            constructionTimer = constructionTimerMax;

            // Store the building type
            buildingType = _buildingTypeSO;

            // Update the sprite and collider to match the building type
            spriteRenderer.sprite = _buildingTypeSO.sprite;
            boxCollider2D.offset = _buildingTypeSO.prefab.GetComponent<BoxCollider2D>().offset;
            boxCollider2D.size = _buildingTypeSO.prefab.GetComponent<BoxCollider2D>().size;

            // Set the building type in the BuildingTypeHolder component
            buildingTypeHolder.buildingType = buildingType;
        }

        // Returns the normalized construction progress (0 to 1)
        public float GetConstructionTimerNormalize()
        {
            return 1 - constructionTimer / constructionTimerMax;
        }
    }
}
