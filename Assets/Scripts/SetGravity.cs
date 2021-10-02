using UnityEngine;

namespace Count_Master_SAY.Control
{
    public class SetGravity : MonoBehaviour
    {
        Rigidbody rb;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        void Update()
        {
            //6 time additional gravity
            rb.velocity += 6* 9.81f * Time.deltaTime * (-Vector3.up);
        }
    }
}

