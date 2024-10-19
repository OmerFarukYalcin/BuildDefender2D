using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class ConstructionTimerUI : MonoBehaviour
    {
        // Reference to the BuildingConstruction script to get the construction progress
        [SerializeField] private BuildingConstruction buildingConstruction;

        // Image UI element used to display the construction progress visually
        private Image constructionProgressImage;

        private void Awake()
        {
            // Find the progress image within the UI hierarchy
            constructionProgressImage = transform.Find("mask").Find("image").GetComponent<Image>();
        }

        private void Update()
        {
            // Update the fill amount of the progress image based on the normalized construction timer
            constructionProgressImage.fillAmount = buildingConstruction.GetConstructionTimerNormalize();
        }
    }
}
