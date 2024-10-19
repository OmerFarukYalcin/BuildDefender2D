using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    public class EnemyWaveManager : MonoBehaviour
    {
        // Singleton instance of EnemyWaveManager
        public static EnemyWaveManager instance { get; private set; }

        // Event triggered when the wave number changes
        public event EventHandler OnWaveNumberChange;

        // Enum representing the two states of the wave manager
        private enum State
        {
            WaitingToSpawnNextWave,
            SpawningWave
        }

        // List of positions where enemies can spawn
        [SerializeField] private List<Transform> spawnPositionTransformList;

        // Transform indicating the position where the next wave will spawn
        [SerializeField] private Transform nextWaveSpawnPositionTransform;

        // Current state of the wave manager
        private State state;

        // Current wave number
        private int waveNumber;

        // Timer for when the next wave will spawn
        private float nextWaveSpawnTimer;

        // Timer for when the next enemy in the current wave will spawn
        private float nextEnemySpawnTimer;

        // Number of remaining enemies to spawn in the current wave
        private int remainingEnemySpawnAmount;

        // Position where the current wave is spawning from
        private Vector3 spawnPosition;

        private void Awake()
        {
            // Set up singleton instance
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            // Set initial state and wave timers
            state = State.WaitingToSpawnNextWave;
            nextWaveSpawnTimer = 3f;

            // Randomly select a spawn position for the next wave
            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
            nextWaveSpawnPositionTransform.position = spawnPosition;
        }

        private void Update()
        {
            switch (state)
            {
                case State.WaitingToSpawnNextWave:
                    // Countdown timer until the next wave spawns
                    nextWaveSpawnTimer -= Time.deltaTime;
                    if (nextWaveSpawnTimer < 0)
                    {
                        SpawnWave();
                    }
                    break;

                case State.SpawningWave:
                    // Handle enemy spawning for the current wave
                    if (remainingEnemySpawnAmount > 0)
                    {
                        nextEnemySpawnTimer -= Time.deltaTime;

                        if (nextEnemySpawnTimer < 0)
                        {
                            // Spawn an enemy at a random position near the spawn point
                            nextEnemySpawnTimer = UnityEngine.Random.Range(0, 0.2f);
                            Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0, 10f));

                            remainingEnemySpawnAmount--;

                            // Check if all enemies in the wave have spawned
                            if (remainingEnemySpawnAmount <= 0)
                            {
                                state = State.WaitingToSpawnNextWave;

                                // Set the next spawn position and timer for the next wave
                                spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                                nextWaveSpawnPositionTransform.position = spawnPosition;
                                nextWaveSpawnTimer = 15f;
                            }
                        }
                    }
                    break;
            }
        }

        // Method to initiate spawning of a new wave
        private void SpawnWave()
        {
            // Determine the number of enemies to spawn based on the wave number
            remainingEnemySpawnAmount = 5 + 3 * waveNumber;

            state = State.SpawningWave;

            waveNumber += 1;

            // Trigger event to notify subscribers that the wave number has changed
            OnWaveNumberChange?.Invoke(this, EventArgs.Empty);
        }

        // Getter method to retrieve the current wave number
        public int GetWaveNumber()
        {
            return waveNumber;
        }

        // Getter method to retrieve the time remaining until the next wave spawns
        public float GetNextWaveSpawnTimer()
        {
            return nextWaveSpawnTimer;
        }

        // Getter method to retrieve the position where the next wave will spawn
        public Vector3 GetSpawnPosition()
        {
            return spawnPosition;
        }
    }
}
