using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Count_Master_SAY.Control
{
    public class EnemyManager : MonoBehaviour
    {
        List<Enemies> enemiesGroupList = new List<Enemies>();
        public Enemies[] enemiesGroupArray;
        public static EnemyManager singleton;
        private void Awake()
        {
            singleton = this;
        }
       private void Start()
        {
            enemiesGroupArray = enemiesGroupList.ToArray();
            enemiesGroupArray = this.GetComponentsInChildren<Enemies>();
            int i = 0;
            foreach (var enemiesGroup in enemiesGroupArray)
            {
                enemiesGroup.id = i;
                i++;
            }
        }

    }

}
