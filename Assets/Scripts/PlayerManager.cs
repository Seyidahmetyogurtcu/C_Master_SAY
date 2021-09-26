using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Count_Master_SAY.Control
{

    public class PlayerManager : MonoBehaviour
    {
        void Start()
        {
            EventManager.singleton.onPlayerMoveTriggerEnter += OnPlayerMove;
        }

        private void OnPlayerMove()
        {
            //Todo:Move all player characters horizontaly
            //Debug.Log("Player is moving");
            transform.Translate(new Vector3(Time.fixedDeltaTime*5, 0,0), Space.Self) ;
        }


    }
}
