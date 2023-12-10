using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BuilderDefender
{
    public class TooltipUI : MonoBehaviour
    {
        public static TooltipUI instance;
        [SerializeField] private RectTransform canvasRectTransform;
        private RectTransform rectTransform;
        private TextMeshProUGUI text;
        private RectTransform backgroundRectTransform;
        private TooltipTimer tooltipTimer;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);

            rectTransform = GetComponent<RectTransform>();

            text = transform.Find("text").GetComponent<TextMeshProUGUI>();

            backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

            Hide();
        }

        private void Update()
        {
            HandleFollowMouse();

            if (tooltipTimer != null)
            {
                tooltipTimer.timer -= Time.deltaTime;
                if (tooltipTimer.timer <= 0)
                {
                    Hide();
                }
            }
        }

        private void HandleFollowMouse()
        {
            Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

            if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
            {
                anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
            }

            if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
            {
                anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
            }

            rectTransform.anchoredPosition = anchoredPosition;
        }

        private void SetText(string tooltipText)
        {
            text.SetText(tooltipText);
            text.ForceMeshUpdate();

            Vector2 textSize = text.GetRenderedValues(false);
            Vector2 padding = new Vector2(8, 8);
            backgroundRectTransform.sizeDelta = textSize + padding;
        }

        public void Show(string tooltipText, TooltipTimer _tooltipTimer = null)
        {
            this.tooltipTimer = _tooltipTimer;

            gameObject.SetActive(true);

            SetText(tooltipText);

            HandleFollowMouse();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public class TooltipTimer
        {
            public float timer;
        }

    }
}
