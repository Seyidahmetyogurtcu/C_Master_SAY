using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Count_Master_SAY;
/// <summary>
/// This is for event triggers,this class trigger and call events 
/// </summary>
public class Triggers : MonoBehaviour
{

    private void FixedUpdate()
    {
        //Call this event as default
        EventManager.singleton.PlayerMoveTriggerEnter();
    }


    private void OnTriggerEnter(Collider other)
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
