using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class ArrowProjectile : MonoBehaviour
    {
        float moveSpeed = 20f;
        private Enemy enemyTarget;
        private Vector3 lastMoveDir;
        private float timeToDie = 2f;
        private int damageAmount = 10;

        public static ArrowProjectile Create(Vector3 position, Enemy enemy)
        {
            Transform arrowGo = Resources.Load<Transform>("ArrowProjectile");
            Transform arrowTranform = Instantiate(arrowGo, position, Quaternion.identity);
            ArrowProjectile arrowProjectile = arrowTranform.GetComponent<ArrowProjectile>();
            arrowProjectile.SetTarget(enemy);
            return arrowProjectile;
        }

        private void Update()
        {
            Vector3 moveDir;

            if (enemyTarget != null)
            {
                moveDir = (enemyTarget.transform.position - transform.position).normalized;
                lastMoveDir = moveDir;
            }
            else
            {
                moveDir = lastMoveDir;
            }

            transform.position += moveDir * moveSpeed * Time.deltaTime;

            transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

            timeToDie -= Time.deltaTime;

            if (timeToDie < 0f)
            {
                Destroy(gameObject);
            }
        }

        public void SetTarget(Enemy enemyTarget)
        {
            this.enemyTarget = enemyTarget;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.GetComponent<HealthSystem>().Damage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
}
