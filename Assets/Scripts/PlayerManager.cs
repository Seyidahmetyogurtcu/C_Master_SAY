using Count_Master_SAY.UI;
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

        public int fMagnitude = 3;
        readonly float slowingMultiplier = 10;
        public static PlayerManager singleton;
        int personSpeed = 25;
        int floorHeigth = 5;
        //float appliedGravityDelay = 0.5f;
        //float timer = 0;
        private void Awake()
        {
            singleton = this;
        }
        void Start()
        {
            //subscribe
            EventManager.singleton.onPlayerMoveTriggerEnter += OnPlayerMove;
            EventManager.singleton.onPlayerAtackTriggerEnter += OnPlayerAtack;
            EventManager.singleton.onReplicatorTriggerEnter += OnReplicator;
            EventManager.singleton.onFinishTriggerEnter += OnFinish;
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
            //persons.Count
            //ClimbPersonSeperatly()

            //MoveObByLeaving()

            //WinLevel()

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
                for (int i = 1; i <= addCount * slowingMultiplier; i++)
                {
                    //Instantiate() persons slowingMultiplier times slowly 
                    if (i % slowingMultiplier == 0)
                    {
                        persons.Add(Instantiate(personPrefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5)), Quaternion.identity, this.transform));
                    }
                }
                return;
            }
            //Multiply
            else if (text.Contains("x"))
            {
                string[] numberPart = text.Split('x');
                //Calculate() the transaction
                int multiplyCount = int.Parse(numberPart[1]); //  1 this means get string after ' ' space , 0 means get string before ' ' space
                int totalAdd = (multiplyCount - 1) * persons.Count;
                for (int i = 1; i <= totalAdd * slowingMultiplier; i++)
                {
                    //Instantiate() persons slowingMultiplier times slowly 
                    if (i % slowingMultiplier == 0)
                    {
                        persons.Add(Instantiate(personPrefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5)), Quaternion.identity, this.transform));
                    }
                }
                return;
            }
        }

        private void OnPlayerAtack(int id)
        {
            //GetEnemyList()
            int enemiesCount = EnemyManager.singleton.enemiesGroupArray[id].enemies.Count;
            for (int i = (enemiesCount - 1); i >= 0; i--)
            {
                //TODO:Then Decrease the enemy with our persons 1 by 1 (start with nearer positions)
                Destroy(EnemyManager.singleton.enemiesGroupArray[id].enemies[EnemyManager.singleton.enemiesGroupArray[id].enemies.Count - 1], 1f);
                EnemyManager.singleton.enemiesGroupArray[id].enemies.RemoveAt(EnemyManager.singleton.enemiesGroupArray[id].enemies.Count - 1);
            }
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
            if (Time.timeScale == 1)
            {
                FallDetection();
                Invoke("Attract", 0.01f);
                LoseDetection();

            }
            //}       
        }
        private void Attract()
        {
            for (int i = 0; i < persons.Count; i++)
            {
                distanceToCenter.Add(this.transform.position - persons[i].transform.position);//gets vector from person to center
                float dotted = Vector3.Dot(distanceToCenter[i], Vector3.up);  //get vertical(Y-axsis) projection
                Vector3 temp = distanceToCenter[i] - (dotted * Vector3.up);    //delete vertical(Y-axsis) projection to get X-Z vector
                persons[i].GetComponent<Rigidbody>().AddForce(temp * fMagnitude, ForceMode.Force);
            }
            distanceToCenter.Clear();
        }

        List<int> positionInArray = new List<int>();
        private void FallDetection()
        {
            //for (int i = 1; i < persons.Count; i++)
            //{
            //    if (persons[i].transform.position.y < floorHeigth)
            //    {
            //        int positionInArray = System.Array.IndexOf(persons.ToArray(), persons[i]);
            //        DestroyImmediate(persons[positionInArray]);
            //        persons.RemoveAt(positionInArray);
            //    }
            //}

            foreach (GameObject person in persons)
            {
                if (person.transform.position.y < floorHeigth)
                {
                    CancelInvoke("Attract");
                    positionInArray.Add(System.Array.IndexOf(persons.ToArray(), person));
                    Destroy(persons[System.Array.IndexOf(persons.ToArray(), person)]);
                }
            }
            BubbleSort(positionInArray);
            foreach (int position in positionInArray)
            {
                persons.RemoveAt(position);
            }
            positionInArray.Clear();
        }
        private void LoseDetection()
        {
            if (persons.Count==0)
            {
                Die();
            }
        }

        void Die()
        {
            UIManager.singleton.Lose();
        }

        public static void BubbleSort(List<int> input)
        {
            bool itemMoved = false;
            do
            {
                itemMoved = false;
                for (int i = 0; i < input.Count - 1; i++)
                {
                    if (input[i] < input[i + 1])
                    {
                        int higherValue = input[i + 1];
                        input[i + 1] = input[i];
                        input[i] = higherValue;
                        itemMoved = true;
                    }
                }
            } while (itemMoved);
        }
    }
}
