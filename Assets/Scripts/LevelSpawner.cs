using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] leftSpawnPoints;
    [SerializeField] private Transform[] middleSpawnPoints;
    [SerializeField] private Transform[] rightSpawnPoints;
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private Transform[] coinSpawnPoints;
    [SerializeField] private GameObject coinObject;
    [SerializeField] private int spawnCoinCount;

    private int _spawnObjectsCount;
    private int _objectPointsCount;
    private int _coinPointsCount;

    private void Awake()
    {
        _spawnObjectsCount = spawnObjects.Length;
        _objectPointsCount = leftSpawnPoints.Length;
        _coinPointsCount = coinSpawnPoints.Length;
        InstantiateObjects();
    }
    private int GetUniqueRandomIndex(List<int> usedIndexes, int range)
    {
        if (usedIndexes.Count >= range) return -1;

        int randomIndex = Random.Range(0, range);
        while (usedIndexes.Contains(randomIndex))
        {
            randomIndex = Random.Range(0, range);
        }
        return randomIndex;
    }

    private void InstantiateObjects()
    {
        List<int> usedCoinIndexes = new();

        for (int i = 0; i < spawnCoinCount; i++)
        {
            int randomIndex = GetUniqueRandomIndex(usedCoinIndexes, _coinPointsCount);
            Instantiate(coinObject, coinSpawnPoints[randomIndex].position, Quaternion.identity, coinSpawnPoints[randomIndex]);
            usedCoinIndexes.Add(randomIndex);
        }

        List<int> usedObstacleIndexes = new();

        for (int i = 0; i < 5; i++)
        {
            int randomIndex = GetUniqueRandomIndex(usedObstacleIndexes, _objectPointsCount);
            Instantiate(spawnObjects[Random.Range(0, _spawnObjectsCount)], leftSpawnPoints[randomIndex].position, Quaternion.identity, leftSpawnPoints[randomIndex]);
            usedObstacleIndexes.Add(randomIndex);

            randomIndex = GetUniqueRandomIndex(usedObstacleIndexes, _objectPointsCount);
            Instantiate(spawnObjects[Random.Range(0, _spawnObjectsCount)], middleSpawnPoints[randomIndex].position, Quaternion.identity, middleSpawnPoints[randomIndex]);
            usedObstacleIndexes.Add(randomIndex);

            randomIndex = GetUniqueRandomIndex(usedObstacleIndexes, _objectPointsCount);
            Instantiate(spawnObjects[Random.Range(0, _spawnObjectsCount)], rightSpawnPoints[randomIndex].position, Quaternion.identity, rightSpawnPoints[randomIndex]);
            usedObstacleIndexes.Add(randomIndex);
        }

    }


}
