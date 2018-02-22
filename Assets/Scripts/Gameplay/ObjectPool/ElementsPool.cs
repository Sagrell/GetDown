using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsPool : MonoBehaviour {

    //Instance of object
    #region Singleton
    public static ElementsPool Instance;
    void Awake()
    {
        Instance = this;
    }
     #endregion

    //Max count of objects on the level at once ( 22 lines )
    const float MAX_LINES = 22f;
    const float MAX_NORMAL = MAX_LINES * 4f;
    const float MAX_COINS = MAX_LINES * 4f;
    const float MAX_BROKEN = MAX_LINES * 2f;
    const float MAX_DIAMOND = MAX_LINES / 2f;

    //Create a pool class
    [System.Serializable]
    public class PoolSettings
    {
        public string type;
        public int size;
        public GameObject prefab;
    }
    //Create an array of pools (List is bad choice due to TC and MC)
    public PoolSettings[] pools;

    //Create a pool dictionary type=>Queue
    Dictionary<string, Queue<GameObject>> poolDictionary;


    // Initialization
    void Start () {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        for (int i = 0; i < pools.Length; ++i)
        {
            PoolSettings poolSettings = pools[i];
            Queue<GameObject> pool = new Queue<GameObject>();

            for(int j = 0; j < poolSettings.size; ++j)
            {
                GameObject obj = Instantiate(poolSettings.prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            poolDictionary.Add(poolSettings.type, pool);
        }
	}

    //Pick object from pool with specific type and place it at definite position and apply rotation
    public GameObject pickFromPool(string type, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        // If this type of object doesn't exist then return NULL
        if(!poolDictionary.ContainsKey(type))
        {
            Debug.LogError("There is no object with specific type in the pool");
            return null;
        }
        GameObject obj = poolDictionary[type].Dequeue();
        if (obj.activeSelf)
        {
            
            poolDictionary[type].Enqueue(obj);
            return null;
        }
        Transform _transform = obj.transform;
        //SetActive doesn't work on child! Unity should fix this!
        obj.SetActiveRecursively(true);
        _transform.position = position;
        _transform.rotation = rotation;
        if(parent!=null)
        {
            _transform.parent = parent;
        }
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if(pooledObj!=null)
        {
            pooledObj.OnObjectSpawn();
        }
        poolDictionary[type].Enqueue(obj);
        return obj;
    }
    public GameObject pickFromPool(string type, Vector3 position, Transform parent = null)
    {
        // If this type of object doesn't exist then return NULL
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError("There is no object with specific type in the pool");
            return null;
        }
        GameObject obj = poolDictionary[type].Dequeue();
        if (obj.activeSelf)
        {
            poolDictionary[type].Enqueue(obj);
            return null;
        }
        Transform _transform = obj.transform;
        //SetActive doesn't work on child! Unity should fix this!
        obj.SetActiveRecursively(true);
        _transform.position = position;
        if (parent != null)
        {
            _transform.parent = parent;
        }
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        poolDictionary[type].Enqueue(obj);
        return obj;
    }
    public GameObject pickFromPool(string type, Transform parent)
    {
        // If this type of object doesn't exist then return NULL
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError("There is no object with specific type in the pool");
            return null;
        }
        if (poolDictionary[type].Count == 0)
        {
            Debug.LogError("Pool is empty");
            return null;
        }
        GameObject obj = poolDictionary[type].Dequeue();

        if(obj.activeSelf)
        {
            poolDictionary[type].Enqueue(obj);
            return null;
        }
        Transform _transform = obj.transform;
        //SetActive doesn't work on child! Unity should fix this!
        obj.SetActiveRecursively(true);
        _transform.position = parent.position;
        _transform.rotation = parent.rotation;
        if (parent != null)
        {
            _transform.parent = parent;
        }
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        poolDictionary[type].Enqueue(obj);
        return obj;
    }

}
