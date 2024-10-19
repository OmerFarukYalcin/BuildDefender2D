using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class MainMenuUI : MonoBehaviour
    {
        private void Awake()
        {
            // Find and set up the "Play" button to start the game when clicked
            transform.Find("playBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                // Load the game scene using the GameSceneManager
                GameSceneManager.Load(GameSceneManager.GameScene.GameScene);
            });

            // Find and set up the "Quit" button to exit the application when clicked
            transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                // Quit the application
                Application.Quit();
            });
        }
    }
}
