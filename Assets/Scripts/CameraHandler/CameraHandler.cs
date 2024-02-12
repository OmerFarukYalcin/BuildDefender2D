using Cinemachine;
using UnityEngine;

namespace BuilderDefender
{
    public class CameraHandler : MonoBehaviour
    {
        public static CameraHandler instance { get; private set; }
        [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
        private float moveSpeed = 30f;
        private float zoomAmount = 2f;
        private float zoomSpeed = 5f;
        private float orthographicSize;
        private float TargetOrthographicSize;
        private float minOrthographicSize = 10;
        private float maxOrthographicSize = 30;
        private bool edgeScrolling;

        private void Awake()
        {
            instance = this;

            edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
        }

        private void Start()
        {
            orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
            TargetOrthographicSize = orthographicSize;
        }

        void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        void HandleMovement()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");


            if (edgeScrolling)
            {
                float edgeScrollingSize = 30;
                if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
                {
                    x = 1;
                }

                if (Input.mousePosition.x < edgeScrollingSize)
                {
                    x = -1;
                }

                if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
                {
                    y = 1;
                }

                if (Input.mousePosition.y < edgeScrollingSize)
                {
                    y = -1;
                }
            }


            Vector3 moveDir = new Vector3(x, y).normalized;

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        void HandleZoom()
        {
            TargetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

            TargetOrthographicSize = Mathf.Clamp(TargetOrthographicSize, minOrthographicSize, maxOrthographicSize);

            orthographicSize = Mathf.Lerp(orthographicSize, TargetOrthographicSize, Time.deltaTime * zoomSpeed);
            cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
        }

        public void SetEdgeScrolling(bool _edgeScrolling)
        {
            edgeScrolling = _edgeScrolling;

            PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
        }

        public bool GetEdgeScrolling()
        {
            return edgeScrolling;
        }
    }
}
