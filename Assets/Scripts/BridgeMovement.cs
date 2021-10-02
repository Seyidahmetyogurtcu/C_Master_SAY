using UnityEngine;

namespace Count_Master_SAY.Control
{
    public class BridgeMovement : MonoBehaviour
    {
        private Transform _movingBridge;
        private int _sign = 1;
        private float _widthOfPlatform = 15;
        private int _platformSpeed = 5;

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
            if (Mathf.Abs((int)_movingBridge.localPosition.z) <= _widthOfPlatform)
            {
                _movingBridge.localPosition += (_sign) * (new Vector3(0, 0, (_platformSpeed) * Time.fixedDeltaTime));
            }
            if ((int)_movingBridge.localPosition.z > _widthOfPlatform)
            {
                _movingBridge.localPosition = new Vector3(_movingBridge.localPosition.x, _movingBridge.localPosition.y, _widthOfPlatform);
                _sign = -_sign;
            }
            if ((int)_movingBridge.localPosition.z < -(_widthOfPlatform))
            {
                _movingBridge.localPosition = new Vector3(_movingBridge.localPosition.x, _movingBridge.localPosition.y, -(_widthOfPlatform));
                _sign = -_sign;
            }
        }
    }
}
