using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Vector3 initialPosition;
    private Vector3 finalPosition;
    float normalizeSpeed = 0.003f;
    private void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR_64
        if (Input.touchCount < 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase ==TouchPhase.Began)
            {
                initialPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                finalPosition = touch.position;
                
                Vector3 temp = initialPosition- finalPosition;
                Vector3 temp2=Vector3.zero;
                temp2.z = temp.x;// this is because screen UI use x(horizontal)-y(vertical) coordinate 
                this.gameObject.transform.position += (temp2*normalizeSpeed);
            }
        }
#endif

#if UNITY_EDITOR_64
        if (Input.GetMouseButtonDown(0))
        {
            initialPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            finalPosition = Input.mousePosition;

            Vector3 temp = initialPosition - finalPosition;
            Vector3 temp2 = Vector3.zero;
            temp2.z = temp.x; // this is because screen UI use x(horizontal)-y(vertical) coordinate 

            this.gameObject.transform.position += (temp2 * normalizeSpeed);
        }
        Debug.Log(" Input.mousePosition: " + Input.mousePosition);
#endif
    }
}
