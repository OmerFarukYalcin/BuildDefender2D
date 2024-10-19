using UnityEngine;

namespace BuilderDefender
{
    public class MusicManager : MonoBehaviour
    {
        // Reference to the AudioSource component, which plays the background music
        private AudioSource audioSource;

        // Current volume level of the music, initialized to 50%
        private float volume = 0.5f;

        private void Awake()
        {
            // Get the AudioSource component attached to this GameObject
            audioSource = GetComponent<AudioSource>();

            // Load the saved volume setting from PlayerPrefs, or use default value 0.5 if none exists
            volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);

            // Set the AudioSource's volume to the loaded volume value
            audioSource.volume = volume;
        }

        // Method to increase the music volume by 10%, up to a maximum of 100%
        public void IncreaseVolume()
        {
            volume += 0.1f;               // Increase volume
            volume = Mathf.Clamp01(volume); // Ensure volume stays within the 0-1 range

            // Save the new volume value to PlayerPrefs
            PlayerPrefs.SetFloat("musicVolume", volume);

            // Update the AudioSource's volume to reflect the new value
            audioSource.volume = volume;
        }

        // Method to decrease the music volume by 10%, down to a minimum of 0%
        public void DecreaseVolume()
        {
            volume -= 0.1f;               // Decrease volume
            volume = Mathf.Clamp01(volume); // Ensure volume stays within the 0-1 range

            // Save the new volume value to PlayerPrefs
            PlayerPrefs.SetFloat("musicVolume", volume);

            // Update the AudioSource's volume to reflect the new value
            audioSource.volume = volume;
        }

        // Getter to retrieve the current music volume
        public float GetVolume => volume;
    }
}
