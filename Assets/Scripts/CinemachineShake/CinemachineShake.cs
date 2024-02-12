using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace BuilderDefender
{
    public class CinemachineShake : MonoBehaviour
    {
        public static CinemachineShake instance { get; private set; }
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin cinemachineMultiChannnelPerling;
        private float timer;
        private float timerMax;
        private float startingIntensity;
        void Awake()
        {
            instance = this;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            cinemachineMultiChannnelPerling = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            if (timer < timerMax)
            {
                timer += Time.deltaTime;
                float amplitude = Mathf.Lerp(startingIntensity, 0f, timer / timerMax);
                cinemachineMultiChannnelPerling.m_AmplitudeGain = amplitude;
            }
        }

        public void ShakeCamera(float intensity, float timerMax)
        {
            this.timerMax = timerMax;
            timer = 0f;
            startingIntensity = intensity;
            cinemachineMultiChannnelPerling.m_AmplitudeGain = intensity;


        }
    }
}
