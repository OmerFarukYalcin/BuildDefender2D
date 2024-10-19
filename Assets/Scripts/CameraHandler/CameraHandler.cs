using Cinemachine;
using UnityEngine;

namespace BuilderDefender
{
    public class CameraHandler : MonoBehaviour
    {
        // Singleton instance of the CameraHandler
        public static CameraHandler instance { get; private set; }

        // Cinemachine virtual camera used for controlling the camera
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        // Camera movement and zoom settings
        private float moveSpeed = 30f;
        private float zoomAmount = 2f;
        private float zoomSpeed = 5f;
        private float orthographicSize;
        private float targetOrthographicSize;
        private float minOrthographicSize = 10;
        private float maxOrthographicSize = 30;

        // Enable or disable edge scrolling feature
        private bool edgeScrolling;

        private void Awake()
        {
            // Initialize the singleton instance
            instance = this;

            // Load edge scrolling setting from player preferences (default is enabled)
            edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
        }

        private void Start()
        {
            // Initialize the orthographic size of the camera
            orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
            targetOrthographicSize = orthographicSize;
        }

        private void Update()
        {
            // Handle camera movement and zoom every frame
            HandleMovement();
            HandleZoom();
        }

        // Handles camera movement based on keyboard input or edge scrolling
        void HandleMovement()
        {
            // Get keyboard input for horizontal (A/D or arrow keys) and vertical (W/S or arrow keys) movement
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            // Edge scrolling: if the mouse is near the screen edge, move the camera
            if (edgeScrolling)
            {
                float edgeScrollingSize = 30; // Threshold for detecting edge scrolling

                // Check if the mouse is near the right edge of the screen
                if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
                {
                    x = 1;
                }
                // Check if the mouse is near the left edge of the screen
                if (Input.mousePosition.x < edgeScrollingSize)
                {
                    x = -1;
                }
                // Check if the mouse is near the top edge of the screen
                if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
                {
                    y = 1;
                }
                // Check if the mouse is near the bottom edge of the screen
                if (Input.mousePosition.y < edgeScrollingSize)
                {
                    y = -1;
                }
            }

            // Calculate movement direction and move the camera
            Vector3 moveDir = new Vector3(x, y).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        // Handles camera zoom based on mouse scroll input
        void HandleZoom()
        {
            // Adjust target zoom level based on mouse scroll wheel input
            targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

            // Clamp the zoom level within the specified range
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

            // Smoothly interpolate the current zoom level to the target zoom level
            orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
            cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
        }

        // Method to enable or disable edge scrolling
        public void SetEdgeScrolling(bool _edgeScrolling)
        {
            edgeScrolling = _edgeScrolling;

            // Save edge scrolling setting to player preferences
            PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
        }

        // Method to get the current edge scrolling setting
        public bool GetEdgeScrolling()
        {
            return edgeScrolling;
        }
    }
}
