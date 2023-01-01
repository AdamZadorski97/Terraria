using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] protected GameObject objectToPool;
    [SerializeField] protected int poolSize = 10;
    protected Queue<GameObject> objectPool;
    public Transform spawnedObjectsParrent;

    private void Awake()
    {
        objectPool = new Queue<GameObject>();
    }

    public void Initialize(GameObject objectToPool, int poolSize = 10)
    {
        this.objectToPool = objectToPool;
        this.poolSize = poolSize;
    }

    public GameObject CreateObject()
    {
        GameObject spawnedObject = null;
        CreateObjectParentIfNeeded();
        if (objectPool.Count < poolSize)
        {
            spawnedObject = Instantiate(objectToPool, transform.position, Quaternion.identity);
            spawnedObject.name = transform.root.name + "_" + objectToPool.name + "_" + objectPool.Count;
            spawnedObject.transform.SetParent(spawnedObjectsParrent);
        }
        else
        {
            spawnedObject = objectPool.Dequeue();
            spawnedObject.transform.position = transform.position;
            spawnedObject.transform.rotation = Quaternion.identity;
            spawnedObject.SetActive(true);
        }
        objectPool.Enqueue(spawnedObject);
        return spawnedObject;

    }

    private void CreateObjectParentIfNeeded()
    {
        string name = "ObjectPool_" + objectToPool.name;
        var parentObject = GameObject.Find(name);
        if (parentObject != null)
        {
            spawnedObjectsParrent = parentObject.transform;
        }
        else
        {
            spawnedObjectsParrent = new GameObject(name).transform;
        }
    }

    private void OnDestroy()
    {
        if (spawnedObjectsParrent.gameObject != null)
        {
            Destroy(spawnedObjectsParrent.gameObject);
        }

    }

}
