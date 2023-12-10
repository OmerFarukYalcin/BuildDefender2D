using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace BuilderDefender
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
        private float moveSpeed = 30f;
        private float zoomAmount = 2f;
        private float zoomSpeed = 5f;
        private float orthographicSize;
        private float TargetOrthographicSize;
        private float minOrthographicSize = 10;
        private float maxOrthographicSize = 30;

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
    }
}
