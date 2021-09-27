using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Count_Master_SAY;
using UnityEngine.UI;
/// <summary>
/// This is for event triggers,this class trigger and call events 
/// </summary>
public class Triggers : MonoBehaviour
{
    private void FixedUpdate()
    {
        EventManager.singleton.PlayerMoveTriggerEnter();
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Replicator")
        {
            //Get its text
            string ReplicatorText = other.GetComponentInChildren<Text>().text;

            //Call this event
            EventManager.singleton.ReplicatorTriggerEnter(ReplicatorText);         
        }

        else if (other.tag == "EnemyZone")
        {
            string EnemiesCountText = other.GetComponentInChildren<Text>().text;

            //Call this event
            EventManager.singleton.PlayerAtackTriggerEnter(EnemiesCountText);
        }
        else if (other.tag == "FinishZone")
        {
            //Call this event
            EventManager.singleton.FinishTriggerEnter();
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
        }
    }
}
