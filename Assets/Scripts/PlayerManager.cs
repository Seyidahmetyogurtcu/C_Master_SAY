using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            EventManager.singleton.onPlayerMoveTriggerEnter += OnPlayerMove;
        }

        private void OnPlayerMove()
        {
            //Todo:Move all player characters horizontaly
            //Debug.Log("Player is moving");
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
                Debug.Log("calculated");
            }
            distanceToCenter.Clear();
        }
    }
}
