using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject levelTemplate;
    [SerializeField] private Transform levelParent;
    [SerializeField] private short templateCount;


    private GameObject _spawnObject;
    private readonly Queue<GameObject> pooledObjects = new();

    private float _tileDistance;
    private float _zPos;

    private int _pieceCount;
    #endregion

    #region Unity Callbacks

    private void Awake() => _tileDistance = 100;
    private void Start()
    {
        SetPooledObject();
        GenerateLevel();
    }

    private void OnEnable() => SubscribeEvents();

    private void OnDisable() => UnSubscribeEvents();

    #endregion

    #region Other Methods
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
            GameObject newObj = Instantiate(levelTemplate,levelParent);
            levelTemplate.SetActive(false);
            pooledObjects.Enqueue(newObj);
        }
    }

    private GameObject GetPooledObject()
    {
        GameObject obj = pooledObjects.Dequeue();
        obj.SetActive(true);
        pooledObjects.Enqueue(obj);

        GameObject[] pooledArray = pooledObjects.ToArray();
        if (_pieceCount == 3) pooledArray[0].SetActive(false);

        return obj;

    }
    #endregion


}
