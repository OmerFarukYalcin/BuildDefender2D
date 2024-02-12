using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BuilderDefender
{
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField] SoundManager soundManager;
        [SerializeField] MusicManager musicManager;
        private TextMeshProUGUI soundVolumeText;
        private TextMeshProUGUI musicVolumeText;
        void Awake()
        {
            soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();

            musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();

            transform.Find("soundIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                soundManager.IncreaseVolume();
                UpdateText();
            });

            transform.Find("soundDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                soundManager.DecreaseVolume();
                UpdateText();
            });

            transform.Find("musicIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                musicManager.IncreaseVolume();
                UpdateText();
            });

            transform.Find("musicDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                musicManager.DecreaseVolume();
                UpdateText();
            });

            transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                GameSceneManager.Load(GameSceneManager.GameScene.MainMenuScene);
            });

            transform.Find("edgeScrollingToggle").GetComponent<Toggle>().onValueChanged.AddListener((bool set) =>
           {
               CameraHandler.instance.SetEdgeScrolling(set);
           });

        }

        private void Start()
        {
            UpdateText();
            gameObject.SetActive(false);

            transform.Find("edgeScrollingToggle").GetComponent<Toggle>().SetIsOnWithoutNotify(CameraHandler.instance.GetEdgeScrolling());
        }

        private void UpdateText()
        {
            soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume * 10).ToString());
            musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume * 10).ToString());
        }

        public void ToggleVisible()
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }




        // Update is called once per frame

    }
}
