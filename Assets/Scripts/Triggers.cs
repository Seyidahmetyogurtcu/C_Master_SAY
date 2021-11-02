using UnityEngine;
using UnityEngine.UI;
using Count_Master_SAY.Control;
/// <summary>
/// This is for event triggers,this class trigger and call events 
/// </summary>


namespace Count_Master_SAY.Trigger
{
    
    public class Triggers : MonoBehaviour
    {
        public const float Delay = 0.3f;
        public const string FinishZone = "FinishZone";
        public const string EnemyZone = "EnemyZone";
        public const string Replicator = "Replicator";

        float time = 0;
        public bool isEnteredAnyTrigger;
        public static Triggers singleton;
        private void Awake()
        {
            singleton = this;
        }
        private void FixedUpdate()
        {
            if (!isEnteredAnyTrigger)
            {
                EventManager.singleton.PlayerMoveTriggerEnter();
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            //if (other.tag == Replicator && (Time.timeSinceLevelLoad > time))
            //{
            //    //Add Delay to prevent colliders' clash
            //    time = Time.timeSinceLevelLoad + Delay;

            //    //Get its text
            //    string ReplicatorText = other.GetComponentInChildren<Text>().text;

            //    //Call this event
            //    EventManager.singleton.ReplicatorTriggerEnter(ReplicatorText);
            //}

            if (other.tag == EnemyZone)
            {
                int id = other.GetComponentInChildren<EnemyHolder>().id;

                //Call this event
                EventManager.singleton.PlayerAtackTriggerEnter(id);
                isEnteredAnyTrigger = true;
            }
            else if (other.tag == FinishZone)
            {
                //Call this event
                EventManager.singleton.FinishTriggerEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == EnemyZone)
            {
                //Call this event
                isEnteredAnyTrigger = false;
            }
        }
    }
}
