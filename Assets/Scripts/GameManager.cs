using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Count_Master_SAY.Pool;
using Count_Master_SAY.Trigger;
using Count_Master_SAY.Control;

namespace Count_Master_SAY.Core
{
    public class GameManager : MonoBehaviour
    {
        public const string Person = "Person";
        public const string Enemy = "Enemy";
        public const string EnemyHolder = "EnemyHolder";
        public const string Floor = "Floor";

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
            Invoke("ReplicatorSpawner", 0.01f);

            /*Enemy*/
            Enemies = new GameObject("Enemies2");
            Enemies.AddComponent<EnemyManager>();
            Enemies.transform.parent = GameObject.Find("Map(Clone)").transform;
        }


        public void ReplicatorSpawner()
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 position = new Vector3(i * 100, 0, 0);
                objectPooler.SpawnFromPool("Replicator", position);
            }
        }

        public List<GameObject> EnemySpawner(GameObject enemyHolder)
        {
            List<GameObject> EnemyObjectList = new List<GameObject>();
            int numberOfEnemy = RandomNumber(1, 20, 5);

            for (int i = 0; i < numberOfEnemy; i++)
            {
                Vector3 position = new Vector3(RandomNumber(-10, +10), 0, RandomNumber(-10, +10));
                GameObject gameObj = objectPooler.SpawnFromPool(Enemy, enemyHolder.transform.position + position);
                gameObj.transform.parent = enemyHolder.transform;
                EnemyObjectList.Add(gameObj);

            }
            return EnemyObjectList;
        }

        public int RandomNumber(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public int RandomNumber(int min, int max, int coefficientConstant)
        {
            return coefficientConstant * UnityEngine.Random.Range(min, max);
        }

    }
}

