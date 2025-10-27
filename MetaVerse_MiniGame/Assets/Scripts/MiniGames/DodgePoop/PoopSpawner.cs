using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopSpawner : MonoBehaviour
{
    [Header("똥 프리팹")]
    [SerializeField] private GameObject poopPrefab;

    [Header("스폰 설정")]
    [SerializeField] private float initialSpawnTime;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float decreaseRate;

    [Header("스폰 범위")]
    [SerializeField] private float spawnHeight;
    [SerializeField] private float spawnRangeX ;

    private float currentSpawnInterval;
    private float spawnTimer;
    private bool isSpawning = true;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        currentSpawnInterval = initialSpawnTime;
        spawnTimer = 0f;
    }

    private void Update()
    {
        if (!isSpawning) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnPoop();
            spawnTimer = 0f;

            if (currentSpawnInterval > minSpawnTime)
            {
                currentSpawnInterval -= decreaseRate;
                currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnTime);
            }
        }
    }

    private void Init()
    {
        initialSpawnTime = 2f;
        minSpawnTime = 0.5f;
        decreaseRate = 0.05f;

        spawnHeight = 6f;
        spawnRangeX = 9f;
    }

    private void SpawnPoop()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0);

        Instantiate(poopPrefab, spawnPosition, Quaternion.identity);
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}

