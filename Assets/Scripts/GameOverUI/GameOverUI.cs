using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderDefender
{
    public class GameOverUI : MonoBehaviour
    {
        public static GameOverUI instance { get; private set; }
        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            transform.Find("retryBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameSceneManager.Load(GameSceneManager.GameScene.GameScene);
            });

            transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameSceneManager.Load(GameSceneManager.GameScene.MainMenuScene);
            });

            Hide();
        }


        public void Show()
        {
            gameObject.SetActive(true);
            int waveNumber = EnemyWaveManager.instance.GetWaveNumber();
            transform.Find("wavesSurvivedText").GetComponent<TextMeshProUGUI>().SetText($"You survived {waveNumber} Waves!");
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
