using Count_Master_SAY.Pool;
using Count_Master_SAY.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Count_Master_SAY.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        public List<GameObject> levels = new List<GameObject>();
        [HideInInspector] public int currentLevel = 0;
        public static LevelGenerator singleton;
        ObjectPooler objectPooler;
        void Awake()
        {
            singleton = this;
            Instantiate(levels[currentLevel]);
        }
        private void Start()
        {
            objectPooler = ObjectPooler.singleton;
            Invoke("GenerateFloor", 0.01f);
            Invoke("GenerateBridge", 0.01f);
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
            objectPooler.SpawnFromPool(GameManager.Bridge,new Vector3(150,0,0));
            objectPooler.SpawnFromPool(GameManager.Bridge, new Vector3(390, 0, 0));
        }
    }
}

