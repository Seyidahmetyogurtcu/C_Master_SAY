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

        public event Action<int> onPlayerAtackTriggerEnter;
        public void PlayerAtackTriggerEnter(int id)
        {
            onPlayerAtackTriggerEnter?.Invoke(id);
        }
        
        public event Action<string> onReplicatorTriggerEnter;
        public void ReplicatorTriggerEnter(string text)
        {
            onReplicatorTriggerEnter?.Invoke(text);
        }

        public event Action onFinishTriggerEnter;
        public void FinishTriggerEnter()
        {
            onFinishTriggerEnter?.Invoke();
        }
    }
}
