using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BuilderDefender
{
    public class OptionsUI : MonoBehaviour
    {
        // References to SoundManager and MusicManager to control sound and music volumes
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private MusicManager musicManager;

        // Text components to display the current sound and music volume levels
        private TextMeshProUGUI soundVolumeText;
        private TextMeshProUGUI musicVolumeText;

        private void Awake()
        {
            // Find and cache the UI elements for displaying sound and music volume
            soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
            musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();

            // Set up buttons to control the sound volume
            transform.Find("soundIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                soundManager.IncreaseVolume(); // Increase sound volume
                UpdateText(); // Update the UI to reflect the new volume
            });

            transform.Find("soundDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                soundManager.DecreaseVolume(); // Decrease sound volume
                UpdateText(); // Update the UI to reflect the new volume
            });

            // Set up buttons to control the music volume
            transform.Find("musicIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                musicManager.IncreaseVolume(); // Increase music volume
                UpdateText(); // Update the UI to reflect the new volume
            });

            transform.Find("musicDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                musicManager.DecreaseVolume(); // Decrease music volume
                UpdateText(); // Update the UI to reflect the new volume
            });

            // Set up button to return to the main menu
            transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Time.timeScale = 1; // Resume the game if paused
                GameSceneManager.Load(GameSceneManager.GameScene.MainMenuScene); // Load main menu scene
            });

            // Set up toggle for edge scrolling feature
            transform.Find("edgeScrollingToggle").GetComponent<Toggle>().onValueChanged.AddListener((bool set) =>
            {
                CameraHandler.instance.SetEdgeScrolling(set); // Enable or disable edge scrolling based on toggle value
            });
        }

        private void Start()
        {
            // Update the volume display texts when the options menu is opened
            UpdateText();

            // Hide the options menu by default
            gameObject.SetActive(false);

            // Set the edge scrolling toggle state without triggering the event listener
            transform.Find("edgeScrollingToggle").GetComponent<Toggle>().SetIsOnWithoutNotify(CameraHandler.instance.GetEdgeScrolling());
        }

        // Updates the displayed sound and music volume text values
        private void UpdateText()
        {
            soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume * 10).ToString()); // Convert sound volume to a value between 0-10
            musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume * 10).ToString()); // Convert music volume to a value between 0-10
        }

        // Toggles the visibility of the options menu and pauses/unpauses the game
        public void ToggleVisible()
        {
            gameObject.SetActive(!gameObject.activeSelf); // Toggle visibility

            if (gameObject.activeSelf)
            {
                Time.timeScale = 0; // Pause the game if options menu is open
            }
            else
            {
                Time.timeScale = 1; // Resume the game if options menu is closed
            }
        }
    }
}
