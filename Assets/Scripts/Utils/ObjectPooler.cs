using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public class ObjectPooler
    {
        private GameObject pooledObjectPrefab;
        private List<GameObject> pooledObjects = new List<GameObject>();
        private int poolSize;

        public ObjectPooler(GameObject prefab, int size)
        {
            pooledObjectPrefab = prefab;
            poolSize = size;

            PopulatePool();
        }

        private void PopulatePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Object.Instantiate(pooledObjectPrefab);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            foreach (var obj in pooledObjects)
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
            
            GameObject newObj = Object.Instantiate(pooledObjectPrefab);
            newObj.SetActive(false);
            pooledObjects.Add(newObj);

            return newObj;
        }
        
        public void DeactivatePooledObject(GameObject obj)
        {
            obj.SetActive(false);
        }

    }
}
