using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Count_Master_SAY.Pool;
using Count_Master_SAY.Trigger;
namespace Count_Master_SAY.Core
{
    public class GameManager : MonoBehaviour
    {
        public const string Person = "Person";

        ObjectPooler objectPooler;
        private void Awake()
        {
           
        }
        private void Start()
        {
            objectPooler = ObjectPooler.singleton;
            Invoke("ReplicatorSpawner", 0);
        }


        public void ReplicatorSpawner()
        {
            for (int i = 0; i < 50; i++)

            {
                Vector3 position = new Vector3(i * 100, 0, 0);
                objectPooler.SpawnFromPool("Replicator", position);
            }
           
          
        }
    }
}

