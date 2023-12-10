using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public static class UtilsClass
    {
        private static Camera mainCamera;
        public static Vector3 GetMouseWorldPosition()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            Vector3 _worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _worldPos.z = 0f;
            return _worldPos;
        }

        public static Vector3 GetRandomDir()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        public static float GetAngleFromVector(Vector3 vector)
        {
            float radians = Mathf.Atan2(vector.y, vector.x);
            float degrees = radians * Mathf.Rad2Deg;
            return degrees;
        }
    }
}
