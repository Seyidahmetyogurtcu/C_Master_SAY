using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Count_Master_SAY;
using UnityEngine.UI;
using Count_Master_SAY.Control;
/// <summary>
/// This is for event triggers,this class trigger and call events 
/// </summary>
namespace Count_Master_SAY.Trigger
{
    public class Triggers : MonoBehaviour
    {
        float time = 0;
        readonly float delay = 0.3f;
        bool isEnteredAnyTrigger;
        private void FixedUpdate()
        {
            if (!isEnteredAnyTrigger)
            {
                EventManager.singleton.PlayerMoveTriggerEnter();
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == "Replicator" && (Time.timeSinceLevelLoad > time))
            {
                //Add delay to prevent colliders' clash
                time = Time.timeSinceLevelLoad + delay;

                //Get its text
                string ReplicatorText = other.GetComponentInChildren<Text>().text;

                //Call this event
                EventManager.singleton.ReplicatorTriggerEnter(ReplicatorText);
            }

            else if (other.tag == "EnemyZone")
            {
                int id = other.GetComponentInChildren<Enemies>().id;

                //Call this event
                EventManager.singleton.PlayerAtackTriggerEnter(id);
                isEnteredAnyTrigger = true;
                Debug.Log("Entered EnemiesZone");
            }
            else if (other.tag == "FinishZone")
            {
                //Call this event
                EventManager.singleton.FinishTriggerEnter();
                Debug.Log("Finished");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "FinishZone")
            {
                //Call this event
            }
            if (other.tag == "Replicator")
            {
                //Call this event
            }

            if (other.tag == "EnemyZone")
            {
                //Call this event
                isEnteredAnyTrigger = false;
            }
        }
    }
}
