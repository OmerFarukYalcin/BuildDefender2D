using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float shootTimerMax;
        private float shootTimer;
        private Enemy targetEnemy;
        private Transform projectileSpawnPosition;
        private float lookForTargetTimer;
        private float lookForTargetTimerMax = .2f;

        private void Awake()
        {
            projectileSpawnPosition = transform.Find("projectileSpawnPosition");
        }

        private void Update()
        {
            HandleTargeting();
            HandleShooting();
        }

        private void HandleTargeting()
        {
            lookForTargetTimer -= Time.deltaTime;
            if (lookForTargetTimer <= 0f)
            {
                lookForTargetTimer += lookForTargetTimerMax;
                LookForTarget();
            }
        }

        private void HandleShooting()
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                shootTimer += shootTimerMax;
                if (targetEnemy != null)
                {
                    ArrowProjectile.Create(projectileSpawnPosition.position, targetEnemy);
                }
            }
        }

        private void LookForTarget()
        {
            float targetMaxRadius = 20f;
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

            foreach (Collider2D collider2D in collider2Ds)
            {
                Enemy enemy = collider2D.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (targetEnemy == null)
                    {
                        targetEnemy = enemy;
                    }
                    else
                    {
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
