using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] leftSpawnPoints;
    [SerializeField] private Transform[] middleSpawnPoints;
    [SerializeField] private Transform[] rightSpawnPoints;
    [SerializeField] private Transform[] coinPoints;

    [SerializeField] GameObject[] spawnObjects;

    void Start()
    {
        Instantiate(spawnObjects[0], leftSpawnPoints[0].position, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
