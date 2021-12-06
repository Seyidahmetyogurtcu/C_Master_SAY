using Count_Master_SAY.Pool;
using Count_Master_SAY.Core;
using System.Collections.Generic;
using UnityEngine;
using Count_Master_SAY.Control;

namespace Count_Master_SAY.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        public List<GameObject> levels = new List<GameObject>();
        [HideInInspector] public int currentLevel = 0;
        public static LevelGenerator singleton;
        ObjectPooler objectPooler;
        // const float floorRate = 0.9f;
        // const float bridgeRate = 0.1f;
        int totalNumberOfLevelBlock;
        string nextFloor = "Floor";
        string nextBlock = "InitialObject";
        int nextBlockPos;
        public GameObject finishPrefab;
        int blockLength = 30;
        int minEnemyNum=1;
        int maxEnemyNum=5;
        GameObject replicator;
        int probabilityOfSign;
        int probabilityOfNum;
        bool finLineCreated;
        void Awake()
        {
            singleton = this;
            Instantiate(levels[currentLevel]);
        }
        private void Start()
        {
            objectPooler = ObjectPooler.singleton;
            totalNumberOfLevelBlock =  UnityEngine.Random.Range(15, 21);
            nextBlockPos = 3;
        }

        void LevelInst()
        {
            //instantiate first floor
            for (int i = -1; i < nextBlockPos; i++)
            {
                objectPooler.SpawnFromPool(nextFloor, new Vector3(i * blockLength, 0, 0));
            }
             nextFloor = "Floor";
             nextBlock = "InitialObject";
        }

        
        void NextFloor(string nextObject)
        {
            nextFloor = nextObject;
        }
        void NextObjcet(string nextObject)
        {
            nextBlock = nextObject;
        }

        void CreateNextObject()
        {
            //Look person counts
            int numberOfPerson = PlayerManager.singleton.persons.Count;

            int rand = UnityEngine.Random.Range(1, 11);//Generetes numbers from 1 to 10

            if (rand <= 9)
            {
                NextFloor(GameManager.Floor);

                //if floor then put object on it

                int possibilityOfEnemy = RandomNumber(0, 1000);//Generetes numbers from 1 to 10

                if (possibilityOfEnemy <= (numberOfPerson)) //<%40
                {
                    NextObjcet(GameManager.Enemy);
                }
                else if ((numberOfPerson) < possibilityOfEnemy && possibilityOfEnemy <= numberOfPerson + 300) //<%30
                {
                    NextObjcet(GameManager.Replicator);
                }
                else if (nextBlock == "InitialObject")
                {
                    NextObjcet(GameManager.Replicator);
                    nextBlock = "Empty";
                }
                else
                {
                    NextObjcet("Empty");
                }


            }
            else
            {
                NextFloor(GameManager.Bridge);
            }

            objectPooler.SpawnFromPool(nextFloor, new Vector3(nextBlockPos * blockLength, 0, 0));
            if (nextBlock == GameManager.Enemy)
            {
                EnemyManager.singleton.EnemyInst(nextBlockPos, blockLength);
            }
            else if (nextBlock == GameManager.Replicator)
            {
                replicator = objectPooler.SpawnFromPool(nextBlock, new Vector3(nextBlockPos * blockLength, 0, 0));
            }
            nextBlockPos++;

            int finalPart=9;
            if (nextBlockPos == (totalNumberOfLevelBlock - finalPart))
            {
                CancelInvoke("CreateNextObject");

                //instantiate finish line;
                for (int i = nextBlockPos; i <= nextBlockPos + finalPart; i++)
                {
                    objectPooler.SpawnFromPool(GameManager.Floor, new Vector3(i * blockLength, 0, 0));
                }
                //finishline
                GameObject platform = GameObject.Find("Platform");
                Vector3 finishPos = new Vector3((nextBlockPos) * blockLength, 0, 0);
                Instantiate(finishPrefab, finishPos, Quaternion.identity, platform.transform);
                finLineCreated = true;
            }
            if (!finLineCreated)
            {
                CalculateNextObjNum(numberOfPerson);
            }
        }

        void CalculateNextObjNum(int numberOfPerson)
        {
            //probability of floor and bridge

            //probability of replicator and enemy

            //if number of person is less then 100 greater then 0
            if (numberOfPerson >= 0 && numberOfPerson < 100)
            {
                if (nextBlock == GameManager.Enemy)
                {
                    minEnemyNum = 1;
                    maxEnemyNum = 5;
                }
                else if (nextBlock == GameManager.Replicator)
                {
                    probabilityOfSign = RandomNumber(0, 100);
                    probabilityOfNum = RandomNumber(0,10);
                    replicator.GetComponent<Replicator>().CalculateReplicator(probabilityOfSign, probabilityOfNum);
                }
            }
            //Create  replicator more likely   

            //if number of person is less then 200 greater then 100
            else if (numberOfPerson >= 100 && numberOfPerson < 200)
            {
                if (nextBlock == GameManager.Enemy)
                {
                    minEnemyNum = 2;
                    maxEnemyNum = 10;
                }
                else if (nextBlock == GameManager.Replicator)
                {
                    probabilityOfSign = RandomNumber(10, 100);
                    probabilityOfNum = RandomNumber(1, 10);
                    replicator.GetComponent<Replicator>().CalculateReplicator(probabilityOfSign, probabilityOfNum);
                }
            }
            //Create replicator and enemy at the same rate

            //if number of person is less then 300 greater then 200
            else if (numberOfPerson >= 200 && numberOfPerson < 300)
            {
                if (nextBlock == GameManager.Enemy)
                {
                    minEnemyNum = 4;
                    maxEnemyNum = 15;
                }
                else if (nextBlock == GameManager.Replicator)
                {
                    probabilityOfSign = RandomNumber(20, 100);
                    probabilityOfNum = RandomNumber(1, 10);
                    replicator.GetComponent<Replicator>().CalculateReplicator(probabilityOfSign, probabilityOfNum);
                }
            }
            //Create enemy more likely

            //if number of person is greater then 300
            else if (numberOfPerson >= 300)
            {
                if (nextBlock == GameManager.Enemy)
                {
                    minEnemyNum = 10;
                    maxEnemyNum = 20;
                }
                else if (nextBlock == GameManager.Replicator)
                {
                    probabilityOfSign = RandomNumber(20, 100);
                    probabilityOfNum = RandomNumber(2, 10);
                    replicator.GetComponent<Replicator>().CalculateReplicator(probabilityOfSign, probabilityOfNum);
                }
            }
            //Just Create enemy 
        }
        void GenerateFloor()
        {
            for (int i = 0; i < 5; i++)
            {
                objectPooler.SpawnFromPool(GameManager.Floor, new Vector3(i * 30, 0, 0));
            }
            for (int i = 6; i < 13; i++)
            {
                objectPooler.SpawnFromPool(GameManager.Floor, new Vector3(i * 30, 0, 0));
            }
            for (int i = 14; i < 25; i++)
            {
                objectPooler.SpawnFromPool(GameManager.Floor, new Vector3(i * 30, 0, 0));
            }
        }
        void GenerateBridge()
        {
            objectPooler.SpawnFromPool(GameManager.Bridge, new Vector3(150, 0, 0));
            objectPooler.SpawnFromPool(GameManager.Bridge, new Vector3(390, 0, 0));
        }

        public List<GameObject> EnemySpawner(GameObject enemyHolder)
        {
            List<GameObject> EnemyObjectList = new List<GameObject>();
            int numberOfEnemy = RandomNumber(minEnemyNum, maxEnemyNum, 5);

            for (int i = 0; i < numberOfEnemy; i++)
            {
                Vector3 position = new Vector3(RandomNumber(-10, +10), 0, RandomNumber(-10, +10));
                GameObject gameObj = objectPooler.SpawnFromPool(GameManager.Enemy, enemyHolder.transform.position + position);
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

