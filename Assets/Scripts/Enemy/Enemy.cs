using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class Enemy : MonoBehaviour
    {
        public static Enemy Create(Vector3 position)
        {
            Transform enemyGo = Resources.Load<Transform>("Enemy");
            Transform enemyTransform = Instantiate(enemyGo, position, Quaternion.identity);
            Enemy enemy = enemyTransform.GetComponent<Enemy>();
            return enemy;
        }
        private Rigidbody2D rb2d;
        private Transform targetTransform;
        float moveSpeed = 6f;
        private float lookForTargetTimer;
        private float lookForTargetTimerMax = .2f;
        private HealthSystem healthSystem;
        void Start()
        {
            if (BuildingManager.instance.GetHQBuilding() != null)
                targetTransform = BuildingManager.instance.GetHQBuilding().transform;
            rb2d = GetComponent<Rigidbody2D>();
            healthSystem = GetComponent<HealthSystem>();
            healthSystem.OnDied += HandleOnDied;
            healthSystem.OnDamage += HandleOnDamage;
            lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
        }

        private void HandleOnDamage(object sender, System.EventArgs e)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.EnemyHit);
            CinemachineShake.instance.ShakeCamera(2f, .1f);
            ChromaticAberration.instance.SetWeight(.5f);
        }

        void Update()
        {
            HandleMovement();

            HandleTargeting();
        }

        private void HandleOnDied(object sender, System.EventArgs e)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.EnemyDie);

            CinemachineShake.instance.ShakeCamera(4f, .15f);

            Instantiate(Resources.Load<Transform>("pfEnemyDieParticles"), transform.position, Quaternion.identity);

            ChromaticAberration.instance.SetWeight(.5f);

            Destroy(gameObject);
        }

        private void HandleMovement()
        {
            if (targetTransform != null)
            {
                Vector3 moveDir = (targetTransform.position - transform.position).normalized;
                rb2d.velocity = moveDir * moveSpeed;
            }
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

        private void LookForTarget()
        {
            float targetMaxRadius = 10f;
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

            foreach (Collider2D collider2D in collider2Ds)
            {
                Building building = collider2D.GetComponent<Building>();
                if (building != null)
                {
                    if (targetTransform == null)
                    {
                        targetTransform = building.transform;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTransform.position))
                        {
                            targetTransform = building.transform;
                        }
                    }
                }
            }

            if (targetTransform == null)
            {
                if (BuildingManager.instance.GetHQBuilding() != null)
                {
                    targetTransform = BuildingManager.instance.GetHQBuilding().transform;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Building building = other.gameObject.GetComponent<Building>();
            if (building != null)
            {
                HealthSystem healthSystem = building.GetComponent<HealthSystem>();
                healthSystem.Damage(10);
                this.healthSystem.Damage(999);
            }
        }
    }
}
