using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Count_Master_SAY.Control
{
    public class PlayerManager : MonoBehaviour
    {
        public List<GameObject> persons = new List<GameObject>();
        [Space]
        public GameObject personPrefab;
        List<Vector3> distanceToCenter = new List<Vector3>();
        public int fMagnitude = 4;
        readonly float slowingMultiplier = 10;
        public static PlayerManager singleton;
        int personSpeed=15;
        //float appliedGravityDelay = 0.5f;
        //float timer = 0;
        private void Awake()
        {
            singleton= this;
        }
        void Start()
        {
            //subscribe
            EventManager.singleton.onPlayerMoveTriggerEnter += OnPlayerMove;
            EventManager.singleton.onPlayerAtackTriggerEnter += OnPlayerAtack;
            EventManager.singleton.onReplicatorTriggerEnter += OnReplicator;
            EventManager.singleton.onFinishTriggerEnter +=OnFinish;
        }
        void OnDestroy()
        {
            //unsubscribe
            EventManager.singleton.onPlayerMoveTriggerEnter -= OnPlayerMove;
            EventManager.singleton.onPlayerAtackTriggerEnter -= OnPlayerAtack;
            EventManager.singleton.onReplicatorTriggerEnter -= OnReplicator;
            EventManager.singleton.onFinishTriggerEnter -= OnFinish;
        }
        private void OnFinish()
        {
            Debug.Log("Finish Event Worked");

            //GetPersonsCount()

            //CalculateSeperation()

            //ClimbPersonSeperatly()

            //MoveObByLeaving()

        }

        private void OnReplicator(string text)
        {
            //Look() the replicator then get its sign and number
           
            //Addition
            if (text.Contains("+"))
            {
                string[] numberPart = text.Split('+');
                //Calculate() the transaction
                int addCount = int.Parse(numberPart[1]); //  1 this means get string after ' ' space , 0 means get string before ' ' space
                for (int i = 1; i <= addCount* slowingMultiplier; i++)
                {
                    //Instantiate() persons slowingMultiplier times slowly 
                    if (i% slowingMultiplier == 0)
                    { 
                        persons.Add(Instantiate(personPrefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5)), Quaternion.identity));
                    }
                }
                return;
            }
            //Multiply
            else if (text.Contains("x"))
            { 
                string[] numberPart =text.Split('x');
                //Calculate() the transaction
                int multiplyCount = int.Parse(numberPart[1]); //  1 this means get string after ' ' space , 0 means get string before ' ' space
                int totalAdd = (multiplyCount - 1) * persons.Count;
                for (int i = 0; i < totalAdd * slowingMultiplier; i++)
                {
                    //Instantiate() persons slowingMultiplier times slowly 
                    if (i % slowingMultiplier == 0)
                    {
                        persons.Add(Instantiate(personPrefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 0, UnityEngine.Random.Range(-5, 5)), Quaternion.identity));
                    }
                }
                return;
            }
        }

        private void OnPlayerAtack(string text)
        {
            Debug.Log("PlayerAtack Event Worked");

            //TODO:GetEnemyList()

            //TODO:Then Decrease the enemy with our persons 1 by 1 (start with nearer positions)

        }

        private void OnPlayerMove()
        {
            //Todo:Move all player characters horizontaly
            transform.Translate(new Vector3(Time.fixedDeltaTime * personSpeed, 0, 0), Space.Self);
        }




        void Update()
        {
            //if (Time.timeSinceLevelLoad > timer)
            //{
            //    timer = appliedGravityDelay + Time.timeSinceLevelLoad;
                Attract();
            //}       
        }

        private void Attract()
        {
            for (int i = 0; i < persons.Count; i++)
            {
                distanceToCenter.Add(this.transform.position - persons[i].transform.position);
                persons[i].GetComponent<Rigidbody>().AddForce(distanceToCenter[i] * fMagnitude, ForceMode.Force);
            }
            distanceToCenter.Clear();
        }
    }
}
