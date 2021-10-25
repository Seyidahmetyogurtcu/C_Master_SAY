using UnityEngine;

namespace Count_Master_SAY.Control
{
    public class PlayerController : MonoBehaviour
    {
        const float NormalizeSpeedForPc = 0.003f;
        const float NormalizeSpeedForMobile = 0.005f;

        public  Vector3 initialPosition;
        private Vector3 finalPosition;
        Vector3 clampedPos = new Vector3(1, 1, 1);
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

                clampedPos += (temp2 * NormalizeSpeedForMobile);
                clampedPos.z = Mathf.Clamp(clampedPos.z, -16.5f, +16.5f);
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, clampedPos.z);
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

                clampedPos += (temp2 * NormalizeSpeedForPc);
                clampedPos.z = Mathf.Clamp(clampedPos.z, -16.5f, +16.5f);
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, clampedPos.z);
            }
#endif
        }
    }
}

