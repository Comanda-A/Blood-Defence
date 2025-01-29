using System.Collections;
using UnityEngine;
using WayOfBlood.Character.Enemy;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab; // Префаб врага
    public int maxEnemies = 10; // Максимальное количество врагов на сцене
    public float spawnInterval = 2f; // Интервал между спавном врагов
    public float spawnDistance = 2f; // Дополнительное расстояние за пределами камеры

    private Camera mainCamera;
    private int currentEnemyCount = 0;

    private void Start()
    {
        mainCamera = Camera.main; // Получаем основную камеру
        StartCoroutine(SpawnEnemies()); // Запускаем корутину для спавна врагов
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Если количество врагов на сцене меньше максимального, спавним нового
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;
            }

            // Ждем указанный интервал перед следующим спавном
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        // Получаем случайную точку за пределами камеры
        Vector2 spawnPosition = GetRandomPositionOutsideCamera();

        // Создаем врага в этой точке
        var go = Instantiate(
            EnemyPrefab,
            new Vector3(spawnPosition.x, spawnPosition.y, EnemyPrefab.transform.position.z),
            Quaternion.identity
        );

        go.GetComponent<EnemyController>().OnDeath += EnemyDestroyed;
    }

    private Vector2 GetRandomPositionOutsideCamera()
    {
        // Определяем границы камеры в мировых координатах
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Вычисляем границы с учетом дополнительного расстояния
        float minX = mainCamera.transform.position.x - cameraWidth / 2 - spawnDistance;
        float maxX = mainCamera.transform.position.x + cameraWidth / 2 + spawnDistance;
        float minY = mainCamera.transform.position.y - cameraHeight / 2 - spawnDistance;
        float maxY = mainCamera.transform.position.y + cameraHeight / 2 + spawnDistance;

        // Выбираем случайную сторону для спавна (верх, низ, лево, право)
        int side = Random.Range(0, 4);

        Vector2 spawnPosition = Vector2.zero;

        switch (side)
        {
            case 0: // Сверху
                spawnPosition = new Vector2(Random.Range(minX, maxX), maxY);
                break;
            case 1: // Снизу
                spawnPosition = new Vector2(Random.Range(minX, maxX), minY);
                break;
            case 2: // Слева
                spawnPosition = new Vector2(minX, Random.Range(minY, maxY));
                break;
            case 3: // Справа
                spawnPosition = new Vector2(maxX, Random.Range(minY, maxY));
                break;
        }

        return spawnPosition;
    }

    // Метод для уменьшения количества врагов на сцене (вызывается при уничтожении врага)
    public void EnemyDestroyed()
    {
        if (currentEnemyCount > 0)
        {
            currentEnemyCount--;
        }
    }
}