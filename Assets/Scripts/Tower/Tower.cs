using UnityEngine;

namespace BuilderDefender
{
    public class Tower : MonoBehaviour
    {
        // Max time between each shot
        [SerializeField] private float shootTimerMax;

        // Current countdown for shooting
        private float shootTimer;

        // The enemy currently being targeted by the tower
        private Enemy targetEnemy;

        // Position from where the projectiles are spawned
        private Transform projectileSpawnPosition;

        // Timers for looking for a new target
        private float lookForTargetTimer;
        private float lookForTargetTimerMax = .2f; // How often the tower looks for a target

        private void Awake()
        {
            // Find and cache the projectile spawn position from the tower's child objects
            projectileSpawnPosition = transform.Find("projectileSpawnPosition");
        }

        private void Update()
        {
            // Handle finding and locking onto a target
            HandleTargeting();

            // Handle shooting projectiles
            HandleShooting();
        }

        // Handles the logic for finding a new target every few seconds
        private void HandleTargeting()
        {
            lookForTargetTimer -= Time.deltaTime;
            if (lookForTargetTimer <= 0f)
            {
                // Reset the target search timer
                lookForTargetTimer += lookForTargetTimerMax;

                // Search for the nearest enemy target
                LookForTarget();
            }
        }

        // Handles the logic for shooting projectiles at the targeted enemy
        private void HandleShooting()
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                // Reset the shooting timer
                shootTimer += shootTimerMax;

                // If there's a target enemy, shoot a projectile at it
                if (targetEnemy != null)
                {
                    // Create an arrow projectile that targets the enemy
                    ArrowProjectile.Create(projectileSpawnPosition.position, targetEnemy);
                }
            }
        }

        // Searches for the closest enemy within a specified radius
        private void LookForTarget()
        {
            // Define the maximum search radius for targets
            float targetMaxRadius = 20f;

            // Find all enemies within the search radius
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

            // Loop through each collider found in the radius
            foreach (Collider2D collider2D in collider2Ds)
            {
                // Check if the collider belongs to an enemy
                Enemy enemy = collider2D.GetComponent<Enemy>();

                // If an enemy is found, set it as the target
                if (enemy != null)
                {
                    // If there's no current target, set this enemy as the target
                    if (targetEnemy == null)
                    {
                        targetEnemy = enemy;
                    }
                    else
                    {
                        // If this enemy is closer than the current target, switch to this enemy
                        if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position))
                        {
                            targetEnemy = enemy;
                        }
                    }
                }
            }
        }
    }
}
