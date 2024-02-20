using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] leftSpawnPoints;
    [SerializeField] private Transform[] middleSpawnPoints;
    [SerializeField] private Transform[] rightSpawnPoints;
    [SerializeField] private Transform[] coinPoints;

    [SerializeField] GameObject[] spawnObjects;

    private int _spawnObjectsCount;
    private int _spawnPointsCount;

    private void Awake()
    {
        _spawnObjectsCount = spawnObjects.Length;
        _spawnPointsCount = leftSpawnPoints.Length;
    }
    void Start()
    {
        List<int> usedIndexes = new();

        for (int i = 0; i < 5; i++)
        {
            int randomIndex = GetUniqueRandomIndex(usedIndexes, _spawnPointsCount);

            Instantiate(spawnObjects[Random.Range(0, _spawnObjectsCount)], leftSpawnPoints[randomIndex].position, Quaternion.identity, leftSpawnPoints[randomIndex]);
            usedIndexes.Add(randomIndex);

            randomIndex = GetUniqueRandomIndex(usedIndexes, _spawnPointsCount);
            Instantiate(spawnObjects[Random.Range(0, _spawnObjectsCount)], middleSpawnPoints[randomIndex].position, Quaternion.identity, middleSpawnPoints[randomIndex]);
            usedIndexes.Add(randomIndex);

            randomIndex = GetUniqueRandomIndex(usedIndexes, _spawnPointsCount);
            Instantiate(spawnObjects[Random.Range(0, _spawnObjectsCount)], rightSpawnPoints[randomIndex].position, Quaternion.identity, rightSpawnPoints[randomIndex]);
            usedIndexes.Add(randomIndex);
        }
    }

    int GetUniqueRandomIndex(List<int> usedIndexes, int range)
    {
        if (usedIndexes.Count >= range) return -1;

        int randomIndex = Random.Range(0, range);
        while (usedIndexes.Contains(randomIndex))
        {
            randomIndex = Random.Range(0, range);
        }
        return randomIndex;
    }
}
