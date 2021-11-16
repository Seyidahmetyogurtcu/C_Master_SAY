using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Count_Master_SAY.Pool;
using Count_Master_SAY.Trigger;
using Count_Master_SAY.Control;
using Count_Master_SAY.Level;

namespace Count_Master_SAY.Core
{
    public class GameManager : MonoBehaviour
    {
        public const string Person = "Person";
        public const string Enemy = "Enemy";
        public const string EnemyHolder = "EnemyHolder";
        public const string Floor = "Floor";
        public const string Bridge = "Bridge";
        public const string Replicator = "Replicator";


        ObjectPooler objectPooler;
        public static GameManager singleton;
        [HideInInspector] public GameObject Enemies;
        EnemyManager enemyManager;
        private void Awake()
        {
            singleton = this;
        }
        private void Start()
        {
            objectPooler = ObjectPooler.singleton;
            enemyManager = EnemyManager.singleton;
            //Invoke("ReplicatorSpawner", 0.01f);

            /*Enemy*/
            Enemies = new GameObject("Enemies2");
            Enemies.AddComponent<EnemyManager>();
            Enemies.transform.parent = GameObject.Find("Map(Clone)").transform;

            InstantiateGame();
        }

        void InstantiateGame()
        {
            //Level Generator
            float nextFloorTime = 30 / PlayerManager.PersonSpeed;
            LevelGenerator.singleton.Invoke("LevelInst", 0.01f);
            LevelGenerator.singleton.InvokeRepeating("CreateNextObject", 0.02f, nextFloorTime);

            //
        }
        public void ReplicatorSpawner()
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 position = new Vector3(i * 100, 0, 0);
                objectPooler.SpawnFromPool(Replicator, position);
            }
        }

    }
}

