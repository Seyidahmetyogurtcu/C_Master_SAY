using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowers : MonoBehaviour
{
    public GameObject backGround;

    void Update()
    {
        Vector3 pos = new Vector3(this.transform.position.x,0,0);
        
        backGround.transform.position= pos;
            //.Translate(this.GetComponent<Rigidbody>().velocity.x*Vector3.forward);
    }
}
