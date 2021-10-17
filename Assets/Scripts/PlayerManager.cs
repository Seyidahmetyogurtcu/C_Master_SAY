using Count_Master_SAY.UI;
using Count_Master_SAY.Trigger;
using System.Collections.Generic;
using Count_Master_SAY.Pool;
using UnityEngine;

namespace Count_Master_SAY.Control
{
    public class PlayerManager : MonoBehaviour
    {
        public List<GameObject> persons = new List<GameObject>();
        [Space]
        public GameObject personPrefab;
        List<Vector3> distanceToCenter = new List<Vector3>();
        List<Vector3> distanceToEnemy = new List<Vector3>();
        List<Vector3> distanceToPlayer = new List<Vector3>();
        List<int> positionInArray = new List<int>();
        int[] cLimbOrder = new int[40];
        public int fMagnitude = 3;
        readonly float slowingMultiplier = 10;
        public static PlayerManager singleton;
        int personSpeed = 25;
        int floorHeigth = 5;
        public int enemyGroupID;
        public bool doesPlayerTriggered;
        public int enemiesCount;
        int maxArragementLineLength;
        int remaining;
        int lineOfAdditionalMaxArragement;
        int lineOfAdditionalArragement;
        ObjectPooler objectPooler;

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

            //Initialize
            objectPooler = ObjectPooler.singleton;
        }
        void OnDestroy()
        {
            //unsubscribe
            EventManager.singleton.onPlayerMoveTriggerEnter -= OnPlayerMove;
            EventManager.singleton.onPlayerAtackTriggerEnter -= OnPlayerAtack;
            EventManager.singleton.onReplicatorTriggerEnter -= OnReplicator;
            EventManager.singleton.onFinishTriggerEnter -= OnFinish;
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
        private void OnFinish()
        {
            CalculateSeperation();
            ClimbPersonSeperatly();
            MoveOnByLeaving();
            CameraCahnge();
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
                        persons.Add(objectPooler.SpawnFromPool("Person", this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5))));
                        #region old code 
                        // persons.Add(Instantiate(personPrefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5)), Quaternion.identity, this.transform));
                        #endregion
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
                       persons.Add(objectPooler.SpawnFromPool("Person", this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5))));
                        #region old code
                        // persons.Add(Instantiate(personPrefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-3, 3), 2, UnityEngine.Random.Range(-5, 5)), Quaternion.identity, this.transform));
                        #endregion
                    }
                }
                return;
            }
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
            transform.Translate(new Vector3(Time.fixedDeltaTime * personSpeed, 0, 0), Space.Self);
        }

        private void CalculateSeperation()
        {
            //triangular arrengement formula
            cLimbOrder[0] = 0;
            for (int i = 0; i < 39; i++)
            {
                cLimbOrder[i + 1] = cLimbOrder[i] + 2 * (i + 1);
            }
            //find range of person count
            int midPoint = (cLimbOrder.Length / 2) - 1;//for 40=>19
            int leftLimit = 0;
            int rightLimit = cLimbOrder.Length;


            //bilinear search algorithm
            for (int i = 0; i < persons.Count; i++)
            {
                if (persons.Count < cLimbOrder[midPoint])
                {
                    rightLimit = midPoint;
                    midPoint = (midPoint + leftLimit) / 2;
                    if (rightLimit - leftLimit == 1)
                    {
                        //find it
                        maxArragementLineLength = leftLimit;
                        remaining = persons.Count - cLimbOrder[leftLimit];
                        return;
                    }
                }
                else if (persons.Count > cLimbOrder[midPoint])
                {

                    leftLimit = midPoint;
                    midPoint = (midPoint + rightLimit) / 2;
                    if (rightLimit - leftLimit == 1)
                    {
                        //find it
                        maxArragementLineLength = leftLimit;
                        remaining = persons.Count - cLimbOrder[leftLimit];
                        return;
                    }
                }
                else if (persons.Count == cLimbOrder[midPoint])
                {
                    //founded!
                    maxArragementLineLength = midPoint;
                    remaining = 0;
                    return;
                }
            }

        }
        private void ClimbPersonSeperatly()
        {
            if (remaining != 0)
            {
                int lineOfAdditionalMaxArragement = remaining / maxArragementLineLength;
                if (lineOfAdditionalMaxArragement != 0)
                {
                    lineOfAdditionalArragement = remaining - lineOfAdditionalMaxArragement * maxArragementLineLength;
                }
                else
                {
                    lineOfAdditionalArragement = 0;
                }
            }
            else
            {
                lineOfAdditionalMaxArragement = 0;
                lineOfAdditionalArragement = 0;
            }
        }
        private void MoveOnByLeaving()
        {
            int maxLine = maxArragementLineLength * 2 + lineOfAdditionalArragement + lineOfAdditionalMaxArragement;
            int numberofObjectPerLine = 1;
            int indexCounter = 0;
            int dubleIteration = 0;
            bool isAdded = false;
            for (int iter = 0; iter < maxLine; iter++)
            {
                for (int i = 0; i < numberofObjectPerLine; i++)
                {
                    if (indexCounter >= persons.Count)
                    {
                        return;
                    }
                    persons[indexCounter].transform.position = new Vector3(this.transform.position.x, 7, (iter - 2 * i));
                    persons[indexCounter].GetComponent<Rigidbody>().isKinematic = true;
                    indexCounter++;
                }
                for (int k = 0; k < indexCounter; k++)
                {
                    persons[k].transform.position += new Vector3(0, 4, 0);

                }
                if (lineOfAdditionalArragement == iter && !isAdded)
                {
                    numberofObjectPerLine--;
                    iter--;
                    dubleIteration--;
                    isAdded = true;
                }
                if (dubleIteration % 2 == 0 && indexCounter < persons.Count)
                {
                    numberofObjectPerLine--;
                    iter--;
                }
                numberofObjectPerLine++;
                dubleIteration++;
            }
            for (int j = 0; j < lineOfAdditionalMaxArragement; j++)
            {
                persons[indexCounter].transform.position = new Vector3(this.transform.position.x, 0, maxLine - 2 * j);
                indexCounter++;
            }
        }
        private void KillNextEnemyAndPerson()
        {
            doesPlayerTriggered = true;
            //GetEnemyList()
            enemiesCount = EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies.Count;
  
            if ((enemiesCount - 1) >= 0)
            {
                /*for enemy*/
                objectPooler.DisappearFromPool("Enemy", EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies[EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies.Count - 1]);//Todo:add anemy to taguse pooler way
                EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies.RemoveAt(EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies.Count - 1);

                /*for person*/
                objectPooler.DisappearFromPool("Person", persons[persons.Count - 1]);
                persons.RemoveAt(persons.Count - 1);

                #region old code
                //doesPlayerTriggered = true;
                ////Decrease the enemy with our persons 1 by 1 (start with nearer positions)
                //Destroy(EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies[EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies.Count - 1]);
                //EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies.RemoveAt(EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies.Count - 1);

                //Destroy(persons[persons.Count - 1]);
                //persons.RemoveAt(persons.Count - 1);
                #endregion
            }
            else
            {
                CancelInvoke("KillNextEnemyAndPerson");
                doesPlayerTriggered = false;
                Vector3 shiftedPosition = new Vector3(0, 0, 100);
                EnemyManager.singleton.enemiesGroupArray[enemyGroupID].GetComponent<BoxCollider>().transform.position += shiftedPosition;
            }


        }
        private void CameraCahnge()
        {
            Camera.main.transform.position += new Vector3(-15,120,-30);
            Camera.main.transform.rotation = new Quaternion(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y - 27, Camera.main.transform.rotation.z, Camera.main.transform.rotation.w);
            Camera.main.transform.LookAt(persons[0].transform,Vector3.up); 
        }

        private void StopMoving() 
        {
            if (this.transform.childCount< 3)
            {
                Triggers.singleton.isEnteredAnyTrigger=true;
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
                persons[i].GetComponent<Rigidbody>().AddForce(temp * fMagnitude*2, ForceMode.Force);
            }
            distanceToCenter.Clear();
        }
        private void AttractPersonsAndEnemies()
        {
            //persons walk to enemy
            for (int i = 0; i < persons.Count; i++)
            {
                distanceToEnemy.Add(EnemyManager.singleton.enemiesGroupArray[enemyGroupID].transform.position - persons[i].transform.position);//gets vector from person to center
                float dotted = Vector3.Dot(distanceToEnemy[i], Vector3.up);  //get vertical(Y-axsis) projection
                Vector3 temp = distanceToEnemy[i] - (dotted * Vector3.up);    //delete vertical(Y-axsis) projection to get X-Z vector
                persons[i].GetComponent<Rigidbody>().AddForce(temp * fMagnitude / 2, ForceMode.Force);
            }
            distanceToEnemy.Clear();


            //Enemies walk to persons
            for (int i = 0; i < EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies.Count; i++)
            {
                distanceToPlayer.Add(this.transform.position - EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies[i].transform.position);//gets vector from person to center
                float dotted = Vector3.Dot(distanceToPlayer[i], Vector3.up);  //get vertical(Y-axsis) projection
                Vector3 temp = distanceToPlayer[i] - (dotted * Vector3.up);    //delete vertical(Y-axsis) projection to get X-Z vector
                EnemyManager.singleton.enemiesGroupArray[enemyGroupID].enemies[i].GetComponent<Rigidbody>().AddForce(temp * fMagnitude / 2, ForceMode.Force);
            }
            distanceToPlayer.Clear();
        }
        private void FallDetection()
        {
            foreach (GameObject person in persons)
            {
                if (person.transform.position.y < floorHeigth)
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
    }
}
