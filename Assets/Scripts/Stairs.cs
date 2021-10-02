using UnityEngine;
namespace Count_Master_SAY.Trigger
{
    public class Stairs : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag=="Player")
            {
                other.GetComponent<Rigidbody>().isKinematic = false;
                other.transform.SetParent(this.transform);
            }
        }
    }
}

