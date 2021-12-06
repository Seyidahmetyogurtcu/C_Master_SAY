using UnityEngine;
using Count_Master_SAY.Control;
using DG.Tweening;
using System.Collections;

namespace Count_Master_SAY.Trigger
{
    public class Stairs : MonoBehaviour
    { 
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //other.GetComponent<Rigidbody>().isKinematic = false;
                other.transform.SetParent(this.transform);             
            }
        }
    }
}

