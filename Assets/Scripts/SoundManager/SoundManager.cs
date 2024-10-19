using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class SoundManager : MonoBehaviour
    {
        // Singleton instance for SoundManager, ensuring only one instance exists
        public static SoundManager instance { get; private set; }

        // Enum to define different sound types that can be played
        public enum Sound
        {
            BuildingPlaced,
            BuildingDamaged,
            BuildingDestroyed,
            EnemyDie,
            EnemyHit,
            GameOver,
        }

        // Reference to the AudioSource component that plays the sounds
        private AudioSource audioSource;

        // Dictionary to map each Sound enum to its corresponding AudioClip
        private Dictionary<Sound, AudioClip> soundAudioClipDictionary;

        // Default volume level for sound, can be adjusted by the player
        private float volume = .5f;

        private void Awake()
        {
            // Initialize the Singleton instance
            instance = this;

            // Get the AudioSource component from the GameObject
            audioSource = GetComponent<AudioSource>();

            // Load the saved volume setting from PlayerPrefs (defaulting to 0.5 if no value is saved)
            volume = PlayerPrefs.GetFloat("soundVolume", .5f);

            // Initialize the dictionary that links sound types to AudioClips
            soundAudioClipDictionary = new();

            // Load all the AudioClips based on the Sound enum values
            foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
            {
                // Load the corresponding AudioClip from the Resources folder
                soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
            }
        }

        // Method to play a specific sound from the dictionary
        public void PlaySound(Sound sound)
        {
            // Play the AudioClip at the set volume using PlayOneShot (so it doesn't interrupt other sounds)
            audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
        }

        // Method to increase the sound volume
        public void IncreaseVolume()
        {
            // Increase the volume by 10% and clamp it between 0 and 1
            volume += .1f;
            volume = Mathf.Clamp01(volume);

            // Save the new volume setting to PlayerPrefs
            PlayerPrefs.SetFloat("soundVolume", volume);
        }

        // Method to decrease the sound volume
        public void DecreaseVolume()
        {
            // Decrease the volume by 10% and clamp it between 0 and 1
            volume -= .1f;
            volume = Mathf.Clamp01(volume);

            // Save the new volume setting to PlayerPrefs
            PlayerPrefs.SetFloat("soundVolume", volume);
        }

        // Getter to retrieve the current sound volume
        public float GetVolume => volume;
    }
}
