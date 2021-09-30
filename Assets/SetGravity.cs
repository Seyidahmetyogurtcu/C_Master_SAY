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
            //additional gravity
            rb.velocity += 4 * 9.81f * Time.deltaTime * (-Vector3.up);
        }
    }
}

