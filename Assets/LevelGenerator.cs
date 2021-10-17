using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Count_Master_SAY.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        public List<GameObject> levels = new List<GameObject>();
        [HideInInspector] public int currentLevel = 0;
        public static LevelGenerator singleton;
        void Awake()
        {
            singleton = this;
            Instantiate(levels[currentLevel]);
        }
    }
}

