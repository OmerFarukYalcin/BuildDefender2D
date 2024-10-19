using UnityEngine;

namespace BuilderDefender
{
    public static class UtilsClass
    {
        // A reference to the main camera in the scene
        private static Camera mainCamera;

        // Returns the mouse position in world coordinates
        public static Vector3 GetMouseWorldPosition()
        {
            // Lazy initialization: If the main camera hasn't been cached, get it from Camera.main
            if (mainCamera == null) mainCamera = Camera.main;

            // Convert the mouse screen position to world position
            Vector3 _worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Set the Z-axis value to 0, ensuring we work in a 2D plane
            _worldPos.z = 0f;

            return _worldPos;
        }

        // Returns a random normalized direction vector
        public static Vector3 GetRandomDir()
        {
            // Generate random values for x and y between -1 and 1, then normalize the vector
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        // Returns the angle (in degrees) from a given vector
        public static float GetAngleFromVector(Vector3 vector)
        {
            // Calculate the angle in radians from the vector's y and x components
            float radians = Mathf.Atan2(vector.y, vector.x);

            // Convert the angle from radians to degrees
            float degrees = radians * Mathf.Rad2Deg;

            return degrees;
        }
    }
}
