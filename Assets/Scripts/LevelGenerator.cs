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
        string nextFloor= "Floor";
        string nextBlock="Replicator";
        int nextBlockPos;
        public GameObject finishPrefab;
        int blockLength = 30;


        void Awake()
        {
            singleton = this;
            Instantiate(levels[currentLevel]);
        }
        private void Start()
        {
            objectPooler = ObjectPooler.singleton;
            totalNumberOfLevelBlock = UnityEngine.Random.Range(10,51);
            nextBlockPos = 6;
        }

        void LevelInst()
        {
            //instantiate first floor
            for (int i = 0; i < nextBlockPos; i++)
            {
                objectPooler.SpawnFromPool(nextFloor, new Vector3(i * blockLength, 0, 0));
            }
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
            int rand = UnityEngine.Random.Range(1, 11);//Generetes numbers from 1 to 10

            if (rand <= 9)
            {
                NextFloor(GameManager.Floor);
            }
            else
            {
                NextFloor(GameManager.Bridge);
            }

            if (nextBlock == GameManager.Floor)
            {
                int rand2 = UnityEngine.Random.Range(1, 11);//Generetes numbers from 1 to 10

                if (rand2 <= 6)
                {
                    NextObjcet("Replicator");
                }
                else
                {
                    NextObjcet(GameManager.Enemy);
                }
            }
       
            objectPooler.SpawnFromPool(nextFloor, new Vector3(nextBlockPos*blockLength,0,0));
            nextBlockPos++;

            //Look person count
            int numberOfPerson = PlayerManager.singleton.persons.Count;

            //probability of floor and bridge

            //probability of replicator and enemy

            //if number of person is less then 100 greater then 0
            if (numberOfPerson >= 0 && numberOfPerson < 100)
            {

            }
            //Create  replicator more likely   

            //if number of person is less then 200 greater then 100
            else if (numberOfPerson >= 100 && numberOfPerson < 200)
            {

            }
            //Create replicator and enemy at the same rate

            //if number of person is less then 300 greater then 200
            else if (numberOfPerson >= 200 && numberOfPerson < 300)
            {

            }
            //Create enemy more likely

            //if number of person is greater then 300
            else if (numberOfPerson >= 300)
            {

            }
            //Just Create enemy 

            if (nextBlockPos==(totalNumberOfLevelBlock-8))
            {
                CancelInvoke("CreateNextObject");

                //instantiate finish line;
                for (int i = nextBlockPos; i <= nextBlockPos+8; i++)
                {
                    objectPooler.SpawnFromPool(GameManager.Floor, new Vector3(i * blockLength, 0, 0));
                }
                //finishline
                GameObject platform = GameObject.Find("Platform");
                Vector3 finishPos = new Vector3((nextBlockPos) * blockLength,0,0);
                Instantiate(finishPrefab, finishPos, Quaternion.identity, platform.transform); 
            }
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
    }
}

