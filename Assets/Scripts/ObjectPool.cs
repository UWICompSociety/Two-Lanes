using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {


    public static ObjectPool instance;

    public GameObject[] objectPrefabs;

    public List<GameObject>[] pools;

    private GameObject poolContainer;

    public int poolAmount = 10;



    void Awake()
    {
        instance = this;
    }


	// Use this for initialization
	void Start () {
        poolContainer = new GameObject("ObjectPool");
        int i = 0 ;
        pools = new List<GameObject>[objectPrefabs.Length];

        foreach(GameObject prefab in objectPrefabs)
        {
           

            pools[i] = new List<GameObject>();

            for(int j=0; j<poolAmount;j++)
            {
                GameObject obj = Instantiate(objectPrefabs[i]) as GameObject;
                obj.name = objectPrefabs[i].name;
                PoolObject(obj);
            }

            i++;
        }
	}

    public GameObject GetPooledObjectForType(string type)
    {
        for(int i=0; i<objectPrefabs.Length;i++)
        {
            if(type == objectPrefabs[i].name)
            {
                foreach(GameObject obj in pools[i])
                {
                    if(!obj.activeInHierarchy)
                    {
                        obj.SetActive(true);
                        return obj;
                    }
                }
                return Instantiate(objectPrefabs[i]) as GameObject;

                
            }
        }
        return null;
    }

    

    void PoolObject(GameObject obj)
    {

        for(int i=0;i<objectPrefabs.Length;i++)
        {

            if(obj.name == objectPrefabs[i].name)
            {
                obj.SetActive(false);
                obj.transform.parent = poolContainer.transform;
                pools[i].Add(obj);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
