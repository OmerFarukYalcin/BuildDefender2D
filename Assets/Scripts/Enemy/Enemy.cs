using UnityEngine;

namespace BuilderDefender
{
    public class Enemy : MonoBehaviour
    {
        // Static method to create an enemy at a specified position
        public static Enemy Create(Vector3 position)
        {
            Transform enemyGo = Resources.Load<Transform>("Enemy");
            Transform enemyTransform = Instantiate(enemyGo, position, Quaternion.identity);
            Enemy enemy = enemyTransform.GetComponent<Enemy>();
            return enemy;
        }

        // Rigidbody2D for handling physics-based movement
        private Rigidbody2D rb2d;

        // Reference to the current target transform (building or HQ)
        private Transform targetTransform;

        // Movement speed for the enemy
        private float moveSpeed = 6f;

        // Timer for periodically looking for a new target
        private float lookForTargetTimer;
        private float lookForTargetTimerMax = 0.2f;

        // Health system for managing the enemy's health
        private HealthSystem healthSystem;

        private void Start()
        {
            // Set initial target to HQ building if it exists
            if (BuildingManager.instance.GetHQBuilding() != null)
                targetTransform = BuildingManager.instance.GetHQBuilding().transform;

            // Get Rigidbody2D and HealthSystem components
            rb2d = GetComponent<Rigidbody2D>();
            healthSystem = GetComponent<HealthSystem>();

            // Subscribe to health system events (OnDied and OnDamage)
            healthSystem.OnDied += HandleOnDied;
            healthSystem.OnDamage += HandleOnDamage;

            // Initialize the target search timer with a random value
            lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
        }

        // Handle the event when the enemy takes damage
        private void HandleOnDamage(object sender, System.EventArgs e)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.EnemyHit);
            CinemachineShake.instance.ShakeCamera(2f, 0.1f);
            ChromaticAberration.instance.SetWeight(0.5f);
        }

        private void Update()
        {
            // Handle movement and targeting logic every frame
            HandleMovement();
            HandleTargeting();
        }

        // Handle the event when the enemy dies
        private void HandleOnDied(object sender, System.EventArgs e)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.EnemyDie);
            CinemachineShake.instance.ShakeCamera(4f, 0.15f);

            // Play death particles
            Instantiate(Resources.Load<Transform>("pfEnemyDieParticles"), transform.position, Quaternion.identity);

            ChromaticAberration.instance.SetWeight(0.5f);

            // Destroy the enemy GameObject
            Destroy(gameObject);
        }

        // Handle enemy movement towards the target
        private void HandleMovement()
        {
            if (targetTransform != null)
            {
                // Move towards the target position
                Vector3 moveDir = (targetTransform.position - transform.position).normalized;
                rb2d.velocity = moveDir * moveSpeed;
            }
        }

        // Handle logic for finding and switching targets
        private void HandleTargeting()
        {
            lookForTargetTimer -= Time.deltaTime;
            if (lookForTargetTimer <= 0f)
            {
                lookForTargetTimer += lookForTargetTimerMax;
                LookForTarget();
            }
        }

        // Look for a new target (closest building or HQ)
        private void LookForTarget()
        {
            float targetMaxRadius = 10f;
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

            foreach (Collider2D collider2D in collider2Ds)
            {
                Building building = collider2D.GetComponent<Building>();
                if (building != null)
                {
                    // If no target, assign the first building found as the target
                    if (targetTransform == null)
                    {
                        targetTransform = building.transform;
                    }
                    else
                    {
                        // If the building is closer than the current target, switch to it
                        if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTransform.position))
                        {
                            targetTransform = building.transform;
                        }
                    }
                }
            }

            // If no building was found, set target to HQ building if it exists
            if (targetTransform == null && BuildingManager.instance.GetHQBuilding() != null)
            {
                targetTransform = BuildingManager.instance.GetHQBuilding().transform;
            }
        }

        // Handle collisions with buildings
        private void OnCollisionEnter2D(Collision2D other)
        {
            Building building = other.gameObject.GetComponent<Building>();
            if (building != null)
            {
                // Damage the building and destroy the enemy
                HealthSystem healthSystem = building.GetComponent<HealthSystem>();
                healthSystem.Damage(10);
                this.healthSystem.Damage(999); // Destroy the enemy
            }
        }
    }
}
