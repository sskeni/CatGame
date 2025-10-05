using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectCleaner : MonoBehaviour
{
    private static PersistentObjectCleaner instance;
    public static PersistentObjectCleaner Instance { get { return instance; } }

    public List<GameObject> objectsToClean = new List<GameObject>();

    private void Awake()
    {
        CheckSingleton();
    }

    // Set up singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Destroys all persistent objects in list
    public void CleanObjects()
    {
        foreach (GameObject obj in objectsToClean)
        {
            Destroy(obj);
        }
        Destroy(this.gameObject);
    }
}
