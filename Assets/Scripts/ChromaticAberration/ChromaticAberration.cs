using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace BuilderDefender
{
    public class ChromaticAberration : MonoBehaviour
    {
        public static ChromaticAberration instance { get; private set; }
        private Volume volume;
        void Awake()
        {
            instance = this;
            volume = GetComponent<Volume>();
        }

        void Update()
        {
            if (volume.weight > 0)
            {
                float decreaseSpeed = 1f;
                volume.weight -= Time.deltaTime * decreaseSpeed;
            }
        }

        public void SetWeight(float weight)
        {
            volume.weight = weight;
        }
    }
}
