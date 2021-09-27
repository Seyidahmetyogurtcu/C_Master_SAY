using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Count_Master_SAY.Control
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> persons = new List<GameObject>();
        List<Vector3> distanceToCenter = new List<Vector3>();
        public int fMagnitude = 25;
        readonly float delay = 0.4f;
        float timer = 0;

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
            if (text.Contains("+"))
            {
                //TODO:Addition
                Debug.Log("Addition and text is"+ text);
            }
            if (text.Contains("x"))
            {
                //TODO:Multiply
                Debug.Log("Multiplyand text is" + text);
            }

            //TODO:Look() the replicator then get its sign and number

            //TODO:Calculate() the transaction

            //TODO:Instantiate() persons 

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
            transform.Translate(new Vector3(Time.fixedDeltaTime * 5, 0, 0), Space.Self);
        }




        void Update()
        {
            if (Time.timeSinceLevelLoad> timer)
            {
                timer = delay + Time.timeSinceLevelLoad;
                Attract();
            }       
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
