using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject[] pieces;
    [SerializeField] private int zPos = 150;

    private GameObject _spawnObject;

    private Queue<GameObject> _pooledObjects;
    private int _lastPieceIndex;

    private void OnEnable() => SubscribeEvents();

    private void Awake()
    {
        SetPooledObject();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            GenerateLevel();
        }
    }

    private void OnDisable() => UnSubscribeEvents();

    private void SubscribeEvents()
    {
        Signals.Instance.OnGenerateLevel += GenerateLevel;
    }

    private void UnSubscribeEvents()
    {
        Signals.Instance.OnGenerateLevel -= GenerateLevel;
    }

    private void GenerateLevel()
    {
        _spawnObject = GetPooledObject();
        _spawnObject.transform.SetPositionAndRotation(new Vector3(0, 0, zPos), Quaternion.identity);
        zPos += 150;
    }

    private void SetPooledObject()
    {
        _pooledObjects = new();
        foreach (GameObject piece in pieces)
        {
            Instantiate(piece, transform);
            piece.SetActive(false);
            _pooledObjects.Enqueue(piece);
        }
    }


    private GameObject GetPooledObject()
    {
        int index = Random.Range(0, _pooledObjects.Count);
        _lastPieceIndex = index;

        while(index == _lastPieceIndex)
        {
            index = Random.Range(0, _pooledObjects.Count);
        }

        GameObject[] pooledArray = _pooledObjects.ToArray();
        GameObject obj = pooledArray[index];

        _pooledObjects = new Queue<GameObject>(_pooledObjects.ToArray().Where(item => item != obj));
        _pooledObjects.Enqueue(obj);
        obj.SetActive(true);

        return obj;

    }
}
