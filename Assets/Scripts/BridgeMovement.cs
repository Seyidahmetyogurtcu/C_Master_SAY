using UnityEngine;

namespace Count_Master_SAY.Control
{
    public class BridgeMovement : MonoBehaviour
    {
        private const float WidthOfPlatform = 15;
        private const int PlatformSpeed = 5;

        private Transform _movingBridge;
        private int _sign = 1;


        void Start()
        {
            _movingBridge = GetComponent<Transform>();
        }

        void FixedUpdate()
        {
            MovePlatform();
        }
        void MovePlatform()
        {
            if (Mathf.Abs((int)_movingBridge.localPosition.z) <= WidthOfPlatform)
            {
                _movingBridge.localPosition += (_sign) * (new Vector3(0, 0, (PlatformSpeed) * Time.fixedDeltaTime));
            }
            if ((int)_movingBridge.localPosition.z > WidthOfPlatform)
            {
                _movingBridge.localPosition = new Vector3(_movingBridge.localPosition.x, _movingBridge.localPosition.y, WidthOfPlatform);
                _sign = -_sign;
            }
            if ((int)_movingBridge.localPosition.z < -(WidthOfPlatform))
            {
                _movingBridge.localPosition = new Vector3(_movingBridge.localPosition.x, _movingBridge.localPosition.y, -(WidthOfPlatform));
                _sign = -_sign;
            }
        }
    }
}
