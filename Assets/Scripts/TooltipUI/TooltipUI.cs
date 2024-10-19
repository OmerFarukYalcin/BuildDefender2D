using TMPro;
using UnityEngine;

namespace BuilderDefender
{
    public class TooltipUI : MonoBehaviour
    {
        // Singleton instance to allow easy access to the TooltipUI
        public static TooltipUI instance;

        // Reference to the Canvas's RectTransform (to manage tooltip positioning within the UI bounds)
        [SerializeField] private RectTransform canvasRectTransform;

        // References to this object's RectTransform, the text, and the background
        private RectTransform rectTransform;
        private TextMeshProUGUI text;
        private RectTransform backgroundRectTransform;

        // Timer to control how long the tooltip remains visible
        private TooltipTimer tooltipTimer;

        private void Awake()
        {
            // Singleton pattern implementation
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject); // Destroy duplicate instance if one already exists

            // Cache references to RectTransform and UI components
            rectTransform = GetComponent<RectTransform>();
            text = transform.Find("text").GetComponent<TextMeshProUGUI>();
            backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

            // Initially hide the tooltip
            Hide();
        }

        private void Update()
        {
            // Move the tooltip to follow the mouse position
            HandleFollowMouse();

            // Check if a tooltip timer is active and count down until it expires
            if (tooltipTimer != null)
            {
                tooltipTimer.timer -= Time.deltaTime;
                if (tooltipTimer.timer <= 0)
                {
                    Hide(); // Hide the tooltip when the timer runs out
                }
            }
        }

        // Moves the tooltip to follow the mouse cursor and keeps it within the screen bounds
        private void HandleFollowMouse()
        {
            // Get the mouse position relative to the canvas
            Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

            // Check if the tooltip exceeds the canvas width, and adjust position if needed
            if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
            {
                anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
            }

            // Check if the tooltip exceeds the canvas height, and adjust position if needed
            if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
            {
                anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
            }

            // Apply the adjusted position to the tooltip
            rectTransform.anchoredPosition = anchoredPosition;
        }

        // Sets the tooltip's text and adjusts the background size to fit the text
        private void SetText(string tooltipText)
        {
            // Set the text in the TextMeshPro component
            text.SetText(tooltipText);

            // Force the text to update immediately
            text.ForceMeshUpdate();

            // Get the actual size of the rendered text
            Vector2 textSize = text.GetRenderedValues(false);

            // Add padding around the text and adjust the background size
            Vector2 padding = new Vector2(8, 8);
            backgroundRectTransform.sizeDelta = textSize + padding;
        }

        // Shows the tooltip with the provided text and an optional timer
        public void Show(string tooltipText, TooltipTimer _tooltipTimer = null)
        {
            // Set the tooltip timer (if provided)
            this.tooltipTimer = _tooltipTimer;

            // Make the tooltip visible
            gameObject.SetActive(true);

            // Set the tooltip text
            SetText(tooltipText);

            // Update the position of the tooltip to follow the mouse
            HandleFollowMouse();
        }

        // Hides the tooltip by deactivating the GameObject
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        // Class to manage tooltip display duration
        public class TooltipTimer
        {
            public float timer;
        }
    }
}
