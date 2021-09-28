﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Count_Master_SAY.Control
{
    public class PlayerController : MonoBehaviour
    {
        public Vector3 initialPosition;
        private Vector3 finalPosition;
        float normalizeSpeedForPc = 0.003f;
        float normalizeSpeedForMobile = 0.005f;

        private void Update()
        {
#if UNITY_ANDROID && !UNITY_EDITOR_64
        if (Input.touchCount > 0)
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
                this.gameObject.transform.position += (temp2*normalizeSpeedForMobile);
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

                this.gameObject.transform.position += (temp2 * normalizeSpeedForPc);
            }
#endif
        }
    }
}

