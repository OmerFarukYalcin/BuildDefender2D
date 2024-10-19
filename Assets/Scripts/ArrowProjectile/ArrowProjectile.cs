using UnityEngine;

namespace BuilderDefender
{
    public class ArrowProjectile : MonoBehaviour
    {
        // Speed at which the arrow moves.
        private float moveSpeed = 20f;

        // The enemy that the arrow is targeting.
        private Enemy enemyTarget;

        // The last direction the arrow moved in, used if the target is lost.
        private Vector3 lastMoveDir;

        // Time in seconds before the arrow is destroyed automatically.
        private float timeToDie = 2f;

        // Amount of damage the arrow deals to the enemy.
        private int damageAmount = 10;

        // Static method to create an ArrowProjectile instance at a specific position with an enemy target.
        public static ArrowProjectile Create(Vector3 position, Enemy enemy)
        {
            // Load the ArrowProjectile prefab and instantiate it at the given position.
            Transform arrowGo = Resources.Load<Transform>("ArrowProjectile");
            Transform arrowTransform = Instantiate(arrowGo, position, Quaternion.identity);

            // Get the ArrowProjectile component and set its target.
            ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
            arrowProjectile.SetTarget(enemy);

            return arrowProjectile;
        }

        private void Update()
        {
            Vector3 moveDir;

            // If the arrow has a target, calculate the direction towards the enemy.
            if (enemyTarget != null)
            {
                moveDir = (enemyTarget.transform.position - transform.position).normalized;
                lastMoveDir = moveDir; // Store the last known move direction.
            }
            else
            {
                // If no target, continue moving in the last known direction.
                moveDir = lastMoveDir;
            }

            // Move the arrow in the calculated direction.
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            // Rotate the arrow to face the direction it's moving.
            transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

            // Decrease the remaining time to live.
            timeToDie -= Time.deltaTime;

            // Destroy the arrow if its time to live has expired.
            if (timeToDie < 0f)
            {
                Destroy(gameObject);
            }
        }

        // Set the enemy target for the arrow.
        public void SetTarget(Enemy enemyTarget)
        {
            this.enemyTarget = enemyTarget;
        }

        // Handle collision with enemy units.
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the arrow has collided with an enemy.
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Deal damage to the enemy and destroy the arrow.
                enemy.GetComponent<HealthSystem>().Damage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
}
