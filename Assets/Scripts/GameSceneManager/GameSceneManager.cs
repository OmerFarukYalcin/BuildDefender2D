using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuilderDefender
{
    public static class GameSceneManager
    {
        public enum GameScene
        {
            GameScene,
            MainMenuScene,
        }


        public static void Load(GameScene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }
    }
}
