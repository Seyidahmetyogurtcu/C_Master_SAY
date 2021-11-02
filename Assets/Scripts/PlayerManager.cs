using Count_Master_SAY.UI;
using Count_Master_SAY.Trigger;
using System.Collections.Generic;
using Count_Master_SAY.Pool;
using UnityEngine;
using Count_Master_SAY.Control;

namespace Count_Master_SAY.Control
{
    public class PlayerManager : MonoBehaviour
    {
        public const int FMagnitude = 6;
        const float SlowingMultiplier = 10;
        const int PersonSpeed = 25;
        const int FloorHeigth = 5;

        public List<GameObject> persons = new List<GameObject>();
        [Space]
        public GameObject personPrefab;
        List<Vector3> distanceToCenter = new List<Vector3>();
        List<Vector3> distanceToEnemy = new List<Vector3>();
        List<Vector3> distanceToPlayer = new List<Vector3>();
        List<int> positionInArray = new List<int>();     

        public static PlayerManager singleton;
        public int enemyGroupID;
        public bool doesPlayerTriggered;
        public int enemiesCount;
        ObjectPooler objectPooler;
        int replicatorValue;
        string sign;
        float time = 0;


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

            //Initialize
            objectPooler = ObjectPooler.singleton;
        }
        void OnDestroy()
        {
            //unsubscribe
            EventManager.singleton.onPlayerMoveTriggerEnter -= OnPlayerMove;
            EventManager.singleton.onPlayerAtackTriggerEnter -= OnPlayerAtack;
            EventManager.singleton.onReplicatorTriggerEnter -= OnReplicator;
        }
        void Update()
        {
            if (Time.timeScale == 1)
            {
                FallDetection();

                if (!doesPlayerTriggered)
                {
                    Invoke("Attract", 0.01f);
                }
                else
                {
                    //Stop walking also enemy and persons attract each others
                    //When all enemies die, continue walking
                    Invoke("AttractPersonsAndEnemies", 0.01f);
                }

                StopMoving();
                LoseDetection();
            }
        }


        private void OnReplicator(string text)
        {
            if (sign == "+")
            {
                Vibrator.Vibrate(80);   //Add vibration

                for (int i = 1; i <= replicatorValue * SlowingMultiplier; i++)
                {
                    //Instantiate() persons SlowingMultiplier times slowly 
                    if (i % SlowingMultiplier == 0)
                    {
                        persons.Add(objectPooler.SpawnFromPool("Person", this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5))));
                    }
                }
                return;
            }

            else if (sign == "x")
            {
                Vibrator.Vibrate(90); //Add vibration

                int totalAdd = (replicatorValue - 1) * persons.Count;
                for (int i = 1; i <= totalAdd * SlowingMultiplier; i++)
                {
                    //Instantiate() persons SlowingMultiplier times slowly 
                    if (i % SlowingMultiplier == 0)
                    {
                        persons.Add(objectPooler.SpawnFromPool("Person", this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5))));
                    }
                }
                return;
            }

            #region old code
            ////Look() the replicator then get its sign and number

            ////Addition
            //if (text.Contains("+"))
            //{
            //    string[] numberPart = text.Split('+');
            //    //Calculate() the transaction
            //    int addCount = int.Parse(numberPart[1]); //  1 this means get string after ' ' space , 0 means get string before ' ' space
            //    for (int i = 1; i <= addCount * SlowingMultiplier; i++)
            //    {
            //        //Instantiate() persons SlowingMultiplier times slowly 
            //        if (i % SlowingMultiplier == 0)
            //        {

            //            persons.Add(Instantiate(personPrefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5)), Quaternion.identity, this.transform));

            //        }
            //    }
            //    return;
            //}
            ////Multiply
            //else if (text.Contains("x"))
            //{
            //    string[] numberPart = text.Split('x');
            //    //Calculate() the transaction
            //    int multiplyCount = int.Parse(numberPart[1]); //  1 this means get string after ' ' space , 0 means get string before ' ' space
            //    int totalAdd = (multiplyCount - 1) * persons.Count;
            //    for (int i = 1; i <= totalAdd * SlowingMultiplier; i++)
            //    {
            //        //Instantiate() persons SlowingMultiplier times slowly 
            //        if (i % SlowingMultiplier == 0)
            //        {

            //            persons.Add(Instantiate(personPrefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5)), Quaternion.identity, this.transform));

            //        }
            //    }
            //    return;
            //}
            #endregion

        }
        private void OnPlayerAtack(int id)
        {
            AttractPersonsAndEnemies();
            enemyGroupID = id;
            InvokeRepeating("KillNextEnemyAndPerson", 0, 0.05f);
        }
        private void OnPlayerMove()
        {
            //Move all player characters horizontaly
            transform.Translate(new Vector3(Time.fixedDeltaTime * PersonSpeed, 0, 0), Space.Self);
        }

        private void KillNextEnemyAndPerson()
        {
            doesPlayerTriggered = true;
            //GetEnemyList()
            enemiesCount = EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies.Count;

            if ((enemiesCount - 1) >= 0)
            {
                /*for enemy*/
                objectPooler.DisappearFromPool("Enemy", EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies[EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies.Count - 1]);//Todo:add anemy to taguse pooler way
                EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies.RemoveAt(EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies.Count - 1);

                /*for person*/
                objectPooler.DisappearFromPool("Person", persons[persons.Count - 1]);
                persons.RemoveAt(persons.Count - 1);

                #region old code
                //doesPlayerTriggered = true;
                ////Decrease the enemy with our persons 1 by 1 (start with nearer positions)
                //Destroy(EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies[EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies.Count - 1]);
                //EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies.RemoveAt(EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies.Count - 1);

                //Destroy(persons[persons.Count - 1]);
                //persons.RemoveAt(persons.Count - 1);
                #endregion
            }
            else
            {
                CancelInvoke("KillNextEnemyAndPerson");
                doesPlayerTriggered = false;
                Vector3 shiftedPosition = new Vector3(0, 0, 100);
                EnemyManager.singleton.enemyHolderArray[enemyGroupID].GetComponent<BoxCollider>().transform.position += shiftedPosition;
            }


        }
        private void StopMoving()
        {
            if (this.transform.childCount < 3)
            {
                Triggers.singleton.isEnteredAnyTrigger = true;
                Invoke("WinLevel", 3);
            }
        }
        private void Attract()
        {
            for (int i = 0; i < persons.Count; i++)
            {
                distanceToCenter.Add(this.transform.position - persons[i].transform.position);//gets vector from person to center
                float dotted = Vector3.Dot(distanceToCenter[i], Vector3.up);  //get vertical(Y-axsis) projection
                Vector3 temp = distanceToCenter[i] - (dotted * Vector3.up);    //delete vertical(Y-axsis) projection to get X-Z vector
                persons[i].GetComponent<Rigidbody>().AddForce(temp * FMagnitude * 2, ForceMode.Force);
            }
            distanceToCenter.Clear();
        }
        private void AttractPersonsAndEnemies()
        {
            //persons walk to enemy
            for (int i = 0; i < persons.Count; i++)
            {
                distanceToEnemy.Add(EnemyManager.singleton.enemyHolderArray[enemyGroupID].transform.position - persons[i].transform.position);//gets vector from person to center
                float dotted = Vector3.Dot(distanceToEnemy[i], Vector3.up);  //get vertical(Y-axsis) projection
                Vector3 temp = distanceToEnemy[i] - (dotted * Vector3.up);    //delete vertical(Y-axsis) projection to get X-Z vector
                persons[i].GetComponent<Rigidbody>().AddForce(temp * FMagnitude / 2, ForceMode.Force);
            }
            distanceToEnemy.Clear();


            //Enemies walk to persons
            for (int i = 0; i < EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies.Count; i++)
            {
                distanceToPlayer.Add(this.transform.position - EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies[i].transform.position);//gets vector from person to center
                float dotted = Vector3.Dot(distanceToPlayer[i], Vector3.up);  //get vertical(Y-axsis) projection
                Vector3 temp = distanceToPlayer[i] - (dotted * Vector3.up);    //delete vertical(Y-axsis) projection to get X-Z vector
                EnemyManager.singleton.enemyHolderArray[enemyGroupID].enemies[i].GetComponent<Rigidbody>().AddForce(temp * FMagnitude / 2, ForceMode.Force);
            }
            distanceToPlayer.Clear();
        }
        private void FallDetection()
        {
            foreach (GameObject person in persons)
            {
                if (person.transform.position.y < FloorHeigth)
                {
                    positionInArray.Add(System.Array.IndexOf(persons.ToArray(), person));
                    Destroy(persons[System.Array.IndexOf(persons.ToArray(), person)]);
                }
            }
            BubbleSort(positionInArray);//sort to remove
            foreach (int position in positionInArray)
            {
                persons.RemoveAt(position);//remove started with biggest number
            }
            positionInArray.Clear();
        }
        private void LoseDetection()
        {
            if (persons.Count == 0)
            {
                Die();
            }
        }
        private void Die()
        {
            UIManager.singleton.Lose();
        }
        private void WinLevel()
        {
            UIManager.singleton.WinLevel();
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
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Triggers.Replicator) && (Time.timeSinceLevelLoad > time))
            {
                time = Time.timeSinceLevelLoad + Triggers.Delay;

                sign  = other.gameObject.GetComponent<Replicator>().Sign;
                replicatorValue = other.gameObject.GetComponent<Replicator>().ReplicatorValue;
            }
            EventManager.singleton.ReplicatorTriggerEnter(sign); //call add event
        }

    }
}
