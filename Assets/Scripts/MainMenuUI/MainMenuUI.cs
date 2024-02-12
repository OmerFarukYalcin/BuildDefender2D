using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class MainMenuUI : MonoBehaviour
    {
        void Awake()
        {
            transform.Find("playBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameSceneManager.Load(GameSceneManager.GameScene.GameScene);
            });

            transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}
