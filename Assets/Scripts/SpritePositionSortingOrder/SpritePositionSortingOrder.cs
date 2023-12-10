using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class SpritePositionSortingOrder : MonoBehaviour
    {
        [SerializeField] private bool runOnce;
        [SerializeField] private float positionOffsetY;
        private SpriteRenderer spriteRenderer;
        float precisionMultiplier = 5f;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            spriteRenderer.sortingOrder = (int)(-(transform.position.y + positionOffsetY) * precisionMultiplier);

            if (runOnce)
            {
                Destroy(this);
            }
        }
    }
}
