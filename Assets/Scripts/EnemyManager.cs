using Count_Master_SAY.Core;
using Count_Master_SAY.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Count_Master_SAY.Control
{
    public class EnemyManager : MonoBehaviour
    {
        List<EnemyHolder> enemyHolderList = new List<EnemyHolder>();
        public EnemyHolder[] enemyHolderArray;

        public static EnemyManager singleton;
        GameManager gameManager;
        ObjectPooler objectPooler;
        int id = 0;
        private void Awake()
        {
            singleton = this;
        }
        private void Start()
        {
            gameManager = GameManager.singleton;
            objectPooler = ObjectPooler.singleton;
        }

        public void EnemyInst(int nextBlockPos, int blockLength)
        {

            // int enemyListLength = gameManager.RandomNumber(5, 15);

            //for (int i = 0; i < enemyListLength; i++)
            //{
            GameObject enemyHolder = objectPooler.SpawnFromPool(GameManager.EnemyHolder, transform.position);
            enemyHolder.GetComponent<EnemyHolder>().enemies = gameManager.EnemySpawner(enemyHolder);

            enemyHolder.transform.position += new Vector3(nextBlockPos * blockLength, 7, 0);
            ////enemyHolder.GetComponent<EnemyHolder>().enemies.ForEach(c => c.transform.parent = enemyHolder.transform);
            ////EnemyHolder.GetComponent<EnemyHolder>().id= i;
            //}
            enemyHolderList.Add(enemyHolder.GetComponent<EnemyHolder>());
            enemyHolderArray = enemyHolderList.ToArray();

            //enemyHolderArray = gameManager.Enemies.GetComponentsInChildren<EnemyHolder>();

            enemyHolderArray[id].id = id;
            id++;
        }


    }

}
