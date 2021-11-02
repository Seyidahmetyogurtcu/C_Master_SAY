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
        private void Awake()
        {
            singleton = this;
        }
       private void Start()
        {
            enemyHolderArray = enemyHolderList.ToArray();
            enemyHolderArray = this.GetComponentsInChildren<EnemyHolder>();
            
            for (int i = 0; i < enemyHolderArray.Length; i++)
            {
                enemyHolderArray[i].id=i;
            }
        }

    }

}
