using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform[] extrenePoints;
    [SerializeField] private GameObject levelTemplate;
    [SerializeField] private short templateCount;
    [SerializeField] private Transform levelParent;



    private GameObject _spawnObject;
    private Queue<GameObject> _pooledObjects = new();

    private float _tileDistance;
    private float _zPos;

    private int _pieceCount;

    private void Awake()
    {
        //_tileDistance = Mathf.Abs(extrenePoints[0].position.z - extrenePoints[1].position.z);
        _tileDistance = 100;
    }

    private void Start()
    {
        SetPooledObject();
        GenerateLevel();
    }

    private void OnEnable() => SubscribeEvents();

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
        if(_pieceCount < 3) _pieceCount++;
        _spawnObject = GetPooledObject();
        _spawnObject.transform.SetPositionAndRotation(new Vector3(0, 0, _zPos), Quaternion.identity);
        _zPos += _tileDistance;
    }

    private void SetPooledObject()
    {
        for (int i = 0; i < templateCount; i++)
        {
            GameObject newObj = Instantiate(levelTemplate);
            levelTemplate.SetActive(false);
            _pooledObjects.Enqueue(newObj);
        }
    }


    private GameObject GetPooledObject()
    {
        GameObject obj = _pooledObjects.Dequeue();
        obj.SetActive(true);
        _pooledObjects.Enqueue(obj);

        GameObject[] pooledArray = _pooledObjects.ToArray();
        if (_pieceCount == 3) pooledArray[0].SetActive(false);

        return obj;

    }
}
