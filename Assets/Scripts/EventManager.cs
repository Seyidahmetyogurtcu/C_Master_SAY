using UnityEngine;
using System;

namespace Count_Master_SAY
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager singleton;
        void Awake()
        {
            singleton = this;
        }

        public event Action onPlayerMoveTriggerEnter;
        public void PlayerMoveTriggerEnter()
        {
            onPlayerMoveTriggerEnter?.Invoke(); //if it exist then invoke this action.
        }

    }

}
