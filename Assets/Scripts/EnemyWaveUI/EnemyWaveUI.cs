using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BuilderDefender
{
    public class EnemyWaveUI : MonoBehaviour
    {
        [SerializeField] private EnemyWaveManager enemyWaveManager;
        private TextMeshProUGUI waveNumberText;
        private TextMeshProUGUI waveMessageText;
        private RectTransform enemyWaveSpawnPositionIndicator;
        private RectTransform enemyClosestPositionIndicator;
        private Camera cam;
        private void Awake()
        {
            cam = Camera.main;

            enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();

            enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();

            waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();

            waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            enemyWaveManager.OnWaveNumberChange += HandleOnWaveNumberChange;
            SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
        }

        private void HandleOnWaveNumberChange(object sender, EventArgs e)
        {
            SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
        }

        private void Update()
        {
            HandleNextWaveMessage();

            HandleEnemyWaveSpawnPositionIndicator();

            HandleEnemyClosestPositionIndicator();
        }

        private void HandleNextWaveMessage()
        {
            float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
            if (nextWaveSpawnTimer <= 0)
            {
                SetMessageText("");
            }
            else
            {
                SetMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F1") + "s");
            }
        }

        private void HandleEnemyWaveSpawnPositionIndicator()
        {
            Vector3 dirToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - cam.transform.position).normalized;

            enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition * 300f;

            enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));

            float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), cam.transform.position);

            enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > cam.orthographicSize * 1.5f);
        }

        private void HandleEnemyClosestPositionIndicator()
        {
            float targetMaxRadius = 9999f;
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(cam.transform.position, targetMaxRadius);

            Enemy targetEnemy = null;

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

            if (targetEnemy != null)
            {
                Vector3 dirToClosestEnemy = (targetEnemy.transform.position - cam.transform.position).normalized;

                enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy * 250f;

                enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));

                float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, cam.transform.position);

                enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > cam.orthographicSize * 1.5f);
            }
            else
            {
                enemyClosestPositionIndicator.gameObject.SetActive(false);
            }
        }

        private void SetMessageText(string message)
        {
            waveMessageText.SetText(message);
        }

        private void SetWaveNumberText(string text)
        {
            waveNumberText.SetText(text);
        }
    }
}
