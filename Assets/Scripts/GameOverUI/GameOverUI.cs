using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class GameOverUI : MonoBehaviour
    {
        // Singleton instance of GameOverUI
        public static GameOverUI instance { get; private set; }

        private void Awake()
        {
            // Ensure there's only one instance of GameOverUI (Singleton pattern)
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);  // If another instance exists, destroy this one

            // Add functionality to the retry button, which reloads the game scene when clicked
            transform.Find("retryBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameSceneManager.Load(GameSceneManager.GameScene.GameScene);  // Reloads the main game scene
            });

            // Add functionality to the main menu button, which returns to the main menu when clicked
            transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameSceneManager.Load(GameSceneManager.GameScene.MainMenuScene);  // Loads the main menu scene
            });

            // Hide the Game Over UI initially
            Hide();
        }

        // Method to show the Game Over UI and display the number of waves survived
        public void Show()
        {
            // Activate the Game Over UI
            gameObject.SetActive(true);

            // Retrieve the number of waves the player survived and display it
            int waveNumber = EnemyWaveManager.instance.GetWaveNumber();
            transform.Find("wavesSurvivedText").GetComponent<TextMeshProUGUI>().SetText($"You survived {waveNumber} Waves!");
        }

        // Method to hide the Game Over UI
        private void Hide()
        {
            // Deactivate the Game Over UI
            gameObject.SetActive(false);
        }
    }
}
