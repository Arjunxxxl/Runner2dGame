using System.Collections.Generic;
using UnityEngine;

public class TilePooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size = 4;
    }

    [Header("Object Pool")]
    public Transform parentT;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region SingleTon
    public static TilePooler Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        SetUpPool();
    }
    #endregion

    #region pool
    void SetUpPool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);

                obj.SetActive(false);
                obj.transform.SetParent(parentT);

                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFormPool(string tileTag, Vector3 pos, Quaternion rot)
    {

        if (!poolDictionary.ContainsKey(tileTag))
        {
            Debug.Log(tileTag);
            return null;
        }

        GameObject objToSpawn = poolDictionary[tileTag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = pos;
        objToSpawn.transform.localRotation = rot;
        poolDictionary[tileTag].Enqueue(objToSpawn);

        return objToSpawn;
    }
    #endregion
}
