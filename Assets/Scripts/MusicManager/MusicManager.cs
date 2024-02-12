using UnityEngine;

namespace BuilderDefender
{
    public class MusicManager : MonoBehaviour
    {
        private AudioSource audioSource;
        private float volume = .5f;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            volume = PlayerPrefs.GetFloat("musicVolume", .5f);

            audioSource.volume = volume;
        }

        public void IncreaseVolume()
        {
            volume += .1f;
            volume = Mathf.Clamp01(volume);

            PlayerPrefs.SetFloat("musicVolume", volume);

            audioSource.volume = volume;
        }

        public void DecreaseVolume()
        {
            volume -= .1f;
            volume = Mathf.Clamp01(volume);

            PlayerPrefs.SetFloat("musicVolume", volume);

            audioSource.volume = volume;
        }

        public float GetVolume => volume;
    }
}
