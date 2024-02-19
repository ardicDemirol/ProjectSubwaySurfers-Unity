using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform[] extrenePoints;
    [SerializeField] private GameObject[] pieces;

    private GameObject _spawnObject;
    private Queue<GameObject> _pooledObjects;

    private float _tileDistance;
    private float _zPos;

    private int _lastIndex = -1;
    private int _currentIndex;

    private void Awake()
    {
        SetPooledObject();
        _tileDistance = Mathf.Abs(extrenePoints[0].position.z - extrenePoints[1].position.z);
        _zPos = _tileDistance;

        //InvokeRepeating("GenerateLevel", 1f, 5f);
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

        _spawnObject = GetRandomPooledObject();
        _spawnObject.transform.SetPositionAndRotation(new Vector3(0, 0, _zPos), Quaternion.identity);
        _zPos += _tileDistance;
    }

    private void SetPooledObject()
    {
        _pooledObjects = new();
        foreach (GameObject piece in pieces)
        {
            piece.SetActive(false);
            _pooledObjects.Enqueue(piece);
        }
    }


    private GameObject GetRandomPooledObject()
    {
        // The same piece cannot be used twice in last 2 choice.
        while (_currentIndex == _pooledObjects.Count - 1)
            _currentIndex = Random.Range(0, _pooledObjects.Count);
        
        _lastIndex = _currentIndex;

        GameObject[] pooledArray = _pooledObjects.ToArray();
        GameObject obj = pooledArray[_currentIndex];


        if (_lastIndex != -1) pooledArray[pooledArray.Length - 1].SetActive(false);
        _pooledObjects = new Queue<GameObject>(_pooledObjects.ToArray().Where(item => item != obj));
        _pooledObjects.Enqueue(obj);
        obj.SetActive(true);

        return obj;

    }
}
