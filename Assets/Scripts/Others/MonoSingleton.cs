﻿using UnityEngine;
public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject newGO = new();
                    _instance = newGO.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
    #region Unity Callbacks
    protected virtual void Awake()
    {
        _instance = this as T;
    }

    #endregion
}