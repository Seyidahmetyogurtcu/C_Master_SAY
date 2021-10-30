using System.Collections.Generic;
using UnityEngine;
using Count_Master_SAY.Control;
using Count_Master_SAY.Trigger;
using Count_Master_SAY.Core;

namespace Count_Master_SAY.Pool
{
    public class ObjectPooler : MonoBehaviour
    {
        PlayerManager playerManager;

        public static ObjectPooler singleton;
        private void Awake()
        {
            singleton = this;
        }

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        private void Start()
        {
            playerManager = PlayerManager.singleton;

            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools) //for each pools like person pool,multiplier pool, enemy pool...
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                GameObject gates = GameObject.Find("Gates");
                
                for (int i = 0; i < pool.size; i++) // fill one tag of pool
                {
                    GameObject obj;

                    if (pool.tag == Triggers.Replicator)
                    {
                        obj = Instantiate(pool.prefab, gates.transform);
                        obj.AddComponent<Replicator>(); //add sign and number properties for each Replicator            
                    }
                    else
                    {
                        obj = Instantiate(pool.prefab, playerManager.transform);
                    }
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);

                }

                poolDictionary.Add(pool.tag, objectPool);



            }
        }
        public GameObject SpawnFromPool(string tag, Vector3 position)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag" + tag + "doesn't exist");
                return null;
            }
            //Spawn First object which was added to the queue at desire position 
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            if (true)
            {

            }
            if (playerManager.persons.Count == poolDictionary[tag].Count)
            {
                GameObject obj = Instantiate(pools[0].prefab, playerManager.transform); // TODO:fix and make better this part
                obj.SetActive(false);
                poolDictionary[tag].Enqueue(obj);
            }
            return objectToSpawn;
        }


        public void DisappearFromPool(string tag, GameObject objectToDisappear)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag" + tag + "doesn't exist");
            }
            //Add last used Gameobjcet to poll again to reuse 
            if (tag == GameManager.Person)
            {
                poolDictionary[tag].Enqueue(objectToDisappear);
            }
            objectToDisappear.SetActive(false);
        }
    }

}
