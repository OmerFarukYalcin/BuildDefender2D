using System;
using TMPro;
using UnityEngine;

namespace BuilderDefender
{
    public class EnemyWaveUI : MonoBehaviour
    {
        // Reference to the EnemyWaveManager, which handles the logic for spawning enemy waves
        [SerializeField] private EnemyWaveManager enemyWaveManager;

        // UI elements for displaying the current wave number and next wave message
        private TextMeshProUGUI waveNumberText;  // Displays the current wave number
        private TextMeshProUGUI waveMessageText; // Displays the time until the next wave spawns

        // UI indicators to show the next wave's spawn position and the closest enemy position
        private RectTransform enemyWaveSpawnPositionIndicator;  // Arrow pointing to the next wave spawn position
        private RectTransform enemyClosestPositionIndicator;    // Arrow pointing to the closest enemy's position

        // Reference to the main camera, used to calculate distances and directions
        private Camera cam;

        private void Awake()
        {
            // Get the main camera for positioning indicators relative to the player's view
            cam = Camera.main;

            // Find the spawn position indicator UI element in the scene's hierarchy
            enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();

            // Find the closest enemy position indicator UI element in the scene's hierarchy
            enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();

            // Find and set references to the text elements for wave number and message display
            waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
            waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            // Subscribe to the wave number change event in the EnemyWaveManager
            // This will update the UI whenever a new wave starts
            enemyWaveManager.OnWaveNumberChange += HandleOnWaveNumberChange;

            // Set the initial wave number in the UI when the game starts
            SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
        }

        // Event handler that is called whenever the wave number changes
        private void HandleOnWaveNumberChange(object sender, EventArgs e)
        {
            // Update the wave number text to reflect the new wave number
            SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
        }

        private void Update()
        {
            // Update the UI elements every frame

            // Show or hide the next wave message depending on how much time is left until the next wave
            HandleNextWaveMessage();

            // Update the position and visibility of the wave spawn position indicator
            HandleEnemyWaveSpawnPositionIndicator();

            // Update the position and visibility of the closest enemy indicator
            HandleEnemyClosestPositionIndicator();
        }

        // Displays the countdown for the next wave in the waveMessageText UI element
        private void HandleNextWaveMessage()
        {
            // Get the remaining time until the next wave spawns
            float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();

            // If the next wave is about to spawn, hide the message
            if (nextWaveSpawnTimer <= 0)
            {
                SetMessageText("");
            }
            else
            {
                // Otherwise, display the time until the next wave spawns, formatted to one decimal place
                SetMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F1") + "s");
            }
        }

        // Updates the arrow that points to the next enemy wave spawn position
        private void HandleEnemyWaveSpawnPositionIndicator()
        {
            // Calculate the direction from the camera to the next spawn position
            Vector3 dirToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - cam.transform.position).normalized;

            // Set the position of the indicator relative to the camera's view
            // The indicator will point in the direction of the next wave spawn
            enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition * 300f;

            // Rotate the indicator arrow to match the direction to the spawn position
            enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));

            // Calculate the distance from the camera to the next spawn position
            float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), cam.transform.position);

            // Show the spawn position indicator only if the spawn position is outside the camera's view
            enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > cam.orthographicSize * 1.5f);
        }

        // Updates the arrow that points to the closest enemy on the map
        private void HandleEnemyClosestPositionIndicator()
        {
            // Set a large radius to check for all enemies near the camera
            float targetMaxRadius = 9999f;
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(cam.transform.position, targetMaxRadius);

            // Track the closest enemy found within the search radius
            Enemy targetEnemy = null;

            // Loop through all colliders found in the search radius
            foreach (Collider2D collider2D in collider2Ds)
            {
                // Check if the collider belongs to an enemy
                Enemy enemy = collider2D.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // If this is the first enemy found, set it as the target
                    if (targetEnemy == null)
                    {
                        targetEnemy = enemy;
                    }
                    else
                    {
                        // Otherwise, check if this enemy is closer than the current target
                        if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position))
                        {
                            targetEnemy = enemy;
                        }
                    }
                }
            }

            // If a closest enemy is found, update the indicator arrow to point towards it
            if (targetEnemy != null)
            {
                Vector3 dirToClosestEnemy = (targetEnemy.transform.position - cam.transform.position).normalized;

                // Set the position of the closest enemy indicator relative to the camera's view
                enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy * 250f;

                // Rotate the indicator arrow to point in the direction of the closest enemy
                enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));

                // Calculate the distance from the camera to the closest enemy
                float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, cam.transform.position);

                // Show the closest enemy indicator only if the enemy is outside the camera's view
                enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > cam.orthographicSize * 1.5f);
            }
            else
            {
                // Hide the indicator if no enemies are found near the camera
                enemyClosestPositionIndicator.gameObject.SetActive(false);
            }
        }

        // Updates the text for the wave message UI element
        private void SetMessageText(string message)
        {
            waveMessageText.SetText(message);
        }

        // Updates the text for the wave number UI element
        private void SetWaveNumberText(string text)
        {
            waveNumberText.SetText(text);
        }
    }
}
