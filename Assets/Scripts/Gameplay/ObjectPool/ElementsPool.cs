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

    public Transform level;
    // Initialization
    void Start () {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        for (int i = 0; i < pools.Length; ++i)
        {
            PoolSettings poolSettings = pools[i];
            Queue<GameObject> pool = new Queue<GameObject>();

            for(int j = 0; j < poolSettings.size; ++j)
            {
                GameObject obj = Instantiate(poolSettings.prefab, level);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            poolDictionary.Add(poolSettings.type, pool);
        }
	}
    public void ChangeCoins()
    {
        foreach (GameObject platform in poolDictionary["Normal"])
        {
            Coin coin = platform.GetComponentInChildren<Coin>();
            DoubleCoin doubleCoin = platform.GetComponentInChildren<DoubleCoin>();
            if (coin)
            {
                coin.gameObject.SetActive(false);
                coin.transform.SetParent(null);
                PickFromPool("DoubleCoin", platform.transform, Coin.startPosition, Coin.startRotation);
            }
            if (doubleCoin)
            {
                doubleCoin.gameObject.SetActive(false);
                doubleCoin.transform.SetParent(null);
                PickFromPool("Coin", platform.transform, DoubleCoin.startPosition, DoubleCoin.startRotation);
            }
        }
    }
    public void RemovePlatformsFromPool()
    {
        foreach (GameObject platform in poolDictionary["Normal"])
        {
            if(platform.activeSelf)
            {
                for (int i = 0; i < platform.transform.childCount; i++)
                {
                    Transform child = platform.transform.GetChild(i);
                    child.gameObject.SetActive(false);
                    child.SetParent(null);
                }
                PickFromPool("PlatformChange", platform.transform.position, platform.transform.rotation, transform.parent).GetComponent<ParticleSystem>().Play();
                platform.SetActive(false);
            } 
        }
        foreach (GameObject platform in poolDictionary["Broken"])
        {
            platform.SetActive(false);
        }
        foreach (GameObject platform in poolDictionary["NormalWithSpike"])
        {
            
            for (int i = 0; i < platform.transform.childCount; i++)
            {
                Transform child = platform.transform.GetChild(i);
                child.gameObject.SetActive(false);
            }
            platform.SetActive(false);
        }
    }
    public void RemoveEnemyiesFromPool()
    {
        foreach (GameObject laser in poolDictionary["Laser"])
        {
            laser.SetActive(false);
        }
        foreach (GameObject diamond in poolDictionary["Diamond"])
        {
            diamond.SetActive(false);
        }

    }

    //Pick object from pool with specific type and place it at definite position and apply rotation
    public GameObject PickFromPool(string type, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        // If this type of object doesn't exist then return NULL
        if(!poolDictionary.ContainsKey(type))
        {
            Debug.LogError("There is no object with specific type in the pool");
            return null;
        }
        GameObject obj = poolDictionary[type].Dequeue();
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();  
        if (obj.activeSelf)
        {
            poolDictionary[type].Enqueue(obj);
            return null;
        }       
        Transform _transform = obj.transform;
        //SetActive doesn't work on child! Unity should fix this!
        obj.SetActive(true);
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        _transform.position = position;
        _transform.rotation = rotation;
        if(parent!=null)
        {
            _transform.parent = parent;
        } 
        poolDictionary[type].Enqueue(obj);
        return obj;
    }
    public GameObject PickFromPool(string type, Vector3 position, Transform parent = null)
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
        if(type == "NormalWithSpike")
        {
            _transform.GetComponentInChildren<Spike>().gameObject.SetActive(true);
        }
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
    public GameObject PickFromPool(string type, Transform parent, Vector3 localPosition, Quaternion localRotation)
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

        if (obj.activeSelf)
        {
            poolDictionary[type].Enqueue(obj);
            return null;
        }
        Transform _transform = obj.transform;
        //SetActive doesn't work on child! Unity should fix this!
        obj.SetActive(true);
        _transform.parent = parent;
        
        _transform.localPosition = localPosition;
        _transform.localRotation = localRotation;

        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        poolDictionary[type].Enqueue(obj);
        return obj;
    }
    public GameObject PickFromPool(string type, Transform parent)
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

        if (obj.activeSelf)
        {
            poolDictionary[type].Enqueue(obj);
            return null;
        }
        Transform _transform = obj.transform;
        //SetActive doesn't work on child! Unity should fix this!
        obj.SetActive(true);

        _transform.parent = parent;
        _transform.position = parent.position;
        _transform.rotation = parent.rotation;

        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        poolDictionary[type].Enqueue(obj);
        return obj;
    }

}
