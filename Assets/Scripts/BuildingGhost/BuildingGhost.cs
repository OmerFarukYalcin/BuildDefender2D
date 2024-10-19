using UnityEngine;

namespace BuilderDefender
{
    public class BuildingGhost : MonoBehaviour
    {
        // References to the sprite GameObject and the resource nearby overlay
        private GameObject spriteGameObject;
        private ResourceNearbyOverlay resourceNearbyOverlay;

        private void Awake()
        {
            // Find the sprite GameObject and resource nearby overlay in the hierarchy
            spriteGameObject = transform.Find("sprite").gameObject;
            resourceNearbyOverlay = transform.Find("ResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();

            // Initially hide the ghost and the overlay
            Hide();
        }

        private void Start()
        {
            // Subscribe to the event when the active building type changes
            BuildingManager.instance.onActiveBuildingTypeChange += HandleOnActiveBuildingTypeChange;
        }

        // Event handler to manage building type changes
        private void HandleOnActiveBuildingTypeChange(object sender, BuildingManager.OnActiveBuildingTypeChangeEventHandler e)
        {
            if (e.activeBuildingType == null)
            {
                // Hide ghost and resource overlay if no building type is selected
                Hide();
                resourceNearbyOverlay.Hide();
            }
            else
            {
                // Show ghost with the selected building type sprite
                Show(e.activeBuildingType.sprite);

                // Show or hide resource overlay based on whether the building type has resource generator data
                if (e.activeBuildingType.hasResourceGeneratorData)
                    resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
                else
                    resourceNearbyOverlay.Hide();
            }
        }

        // Update the ghost's position to follow the mouse
        private void Update()
        {
            transform.position = UtilsClass.GetMouseWorldPosition();
        }

        // Method to show the ghost with the given sprite
        private void Show(Sprite ghostSprite)
        {
            spriteGameObject.SetActive(true);
            spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
        }

        // Method to hide the ghost
        private void Hide()
        {
            spriteGameObject.SetActive(false);
        }
    }
}
