using System.Collections;
using UnityEngine;
using WayOfBlood.Character.Enemy;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab; // ������ �����
    public int maxEnemies = 10; // ������������ ���������� ������ �� �����
    public float spawnInterval = 2f; // �������� ����� ������� ������
    public float spawnDistance = 2f; // �������������� ���������� �� ��������� ������

    private Camera mainCamera;
    private int currentEnemyCount = 0;

    private void Start()
    {
        mainCamera = Camera.main; // �������� �������� ������
        StartCoroutine(SpawnEnemies()); // ��������� �������� ��� ������ ������
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // ���� ���������� ������ �� ����� ������ �������������, ������� ������
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;
            }

            // ���� ��������� �������� ����� ��������� �������
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        // �������� ��������� ����� �� ��������� ������
        Vector2 spawnPosition = GetRandomPositionOutsideCamera();

        // ������� ����� � ���� �����
        var go = Instantiate(
            EnemyPrefab,
            new Vector3(spawnPosition.x, spawnPosition.y, EnemyPrefab.transform.position.z),
            Quaternion.identity
        );

        go.GetComponent<EnemyController>().OnDeath += EnemyDestroyed;
    }

    private Vector2 GetRandomPositionOutsideCamera()
    {
        // ���������� ������� ������ � ������� �����������
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // ��������� ������� � ������ ��������������� ����������
        float minX = mainCamera.transform.position.x - cameraWidth / 2 - spawnDistance;
        float maxX = mainCamera.transform.position.x + cameraWidth / 2 + spawnDistance;
        float minY = mainCamera.transform.position.y - cameraHeight / 2 - spawnDistance;
        float maxY = mainCamera.transform.position.y + cameraHeight / 2 + spawnDistance;

        // �������� ��������� ������� ��� ������ (����, ���, ����, �����)
        int side = Random.Range(0, 4);

        Vector2 spawnPosition = Vector2.zero;

        switch (side)
        {
            case 0: // ������
                spawnPosition = new Vector2(Random.Range(minX, maxX), maxY);
                break;
            case 1: // �����
                spawnPosition = new Vector2(Random.Range(minX, maxX), minY);
                break;
            case 2: // �����
                spawnPosition = new Vector2(minX, Random.Range(minY, maxY));
                break;
            case 3: // ������
                spawnPosition = new Vector2(maxX, Random.Range(minY, maxY));
                break;
        }

        return spawnPosition;
    }

    // ����� ��� ���������� ���������� ������ �� ����� (���������� ��� ����������� �����)
    public void EnemyDestroyed()
    {
        if (currentEnemyCount > 0)
        {
            currentEnemyCount--;
        }
    }
}