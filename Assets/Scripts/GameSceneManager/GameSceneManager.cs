using UnityEngine.SceneManagement;

namespace BuilderDefender
{
    public static class GameSceneManager
    {
        // Enum for different game scenes
        public enum GameScene
        {
            GameScene,      // Represents the main game scene
            MainMenuScene,  // Represents the main menu scene
        }

        // Method to load a specific scene based on the provided enum value
        public static void Load(GameScene scene)
        {
            // Convert the enum to a string and load the corresponding scene
            SceneManager.LoadScene(scene.ToString());
        }
    }
}
