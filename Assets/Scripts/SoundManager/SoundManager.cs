using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance { get; private set; }

        public enum Sound
        {
            BuildingPlaced,
            BuildingDamaged,
            BuildingDestroyed,
            EnemyDie,
            EnemyHit,
            GameOver,
        }

        private AudioSource audioSource;

        private Dictionary<Sound, AudioClip> soundAudioClipDictionary;

        private float volume = .5f;

        private void Awake()
        {
            instance = this;

            audioSource = GetComponent<AudioSource>();

            volume = PlayerPrefs.GetFloat("soundVolume", .5f);

            soundAudioClipDictionary = new();

            foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
            {
                soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
            }
        }

        public void PlaySound(Sound sound)
        {
            audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
        }

        public void IncreaseVolume()
        {
            volume += .1f;
            volume = Mathf.Clamp01(volume);

            PlayerPrefs.SetFloat("soundVolume", volume);
        }

        public void DecreaseVolume()
        {
            volume -= .1f;
            volume = Mathf.Clamp01(volume);

            PlayerPrefs.SetFloat("soundVolume", volume);
        }

        public float GetVolume => volume;
    }
}
