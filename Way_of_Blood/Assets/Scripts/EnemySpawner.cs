using System.Collections;
using UnityEngine;
using WayOfBlood.Character.Enemy;

public class EnemySpawner : MonoBehaviour
{
    public GameObject MeleeEnemyPrefab; // Префаб врага с мечом
    public GameObject RangedEnemyPrefab; // Префаб стрелка
    public int maxEnemies = 10; // Максимальное количество врагов на сцене
    public float spawnInterval = 2f; // Интервал между спавном врагов
    public float spawnDistance = 2f; // Дополнительное расстояние за пределами камеры
    public float meleeEnemyChance = 0.5f; // Вероятность спавна врага ближнего боя

    private Camera mainCamera;
    private int currentEnemyCount = 0;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomPositionOutsideCamera();
        GameObject enemyPrefab = Random.value < meleeEnemyChance ? MeleeEnemyPrefab : RangedEnemyPrefab;
        var enemy = Instantiate(
            enemyPrefab,
            new Vector3(spawnPosition.x, spawnPosition.y, enemyPrefab.transform.position.z),
            Quaternion.identity
        );

        enemy.GetComponent<EnemyController>().OnDeath += EnemyDestroyed;
    }

    private Vector2 GetRandomPositionOutsideCamera()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float minX = mainCamera.transform.position.x - cameraWidth / 2 - spawnDistance;
        float maxX = mainCamera.transform.position.x + cameraWidth / 2 + spawnDistance;
        float minY = mainCamera.transform.position.y - cameraHeight / 2 - spawnDistance;
        float maxY = mainCamera.transform.position.y + cameraHeight / 2 + spawnDistance;

        int side = Random.Range(0, 4);
        Vector2 spawnPosition = side switch
        {
            0 => new Vector2(Random.Range(minX, maxX), maxY),
            1 => new Vector2(Random.Range(minX, maxX), minY),
            2 => new Vector2(minX, Random.Range(minY, maxY)),
            _ => new Vector2(maxX, Random.Range(minY, maxY)),
        };

        return spawnPosition;
    }

    public void EnemyDestroyed()
    {
        if (currentEnemyCount > 0)
        {
            currentEnemyCount--;
        }
    }
}
