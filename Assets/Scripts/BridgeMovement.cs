using UnityEngine;
using DG.Tweening;

namespace Count_Master_SAY.Control
{
    public class BridgeMovement : MonoBehaviour
    {
        #region old values
        private const float WidthOfPlatform = 15;
        private const int PlatformSpeed = 5;
        private Transform _movingBridge;
        private int _sign = 1;

        #endregion
        private int animationSpeed = 6;
        [SerializeField] Ease easeType = Ease.Linear;

        private void Awake()
        {
            DOTween.Init();
        }
        void Start()
        {
            #region old Code
            //_movingBridge = GetComponent<Transform>();
            #endregion

            MoveLeft();//initial call
        }
        void MoveLeft()
        {
            transform.DOMoveZ(15, animationSpeed).SetSpeedBased(true).SetEase(easeType)
                     .OnComplete(() => MoveRight());
        }

        void MoveRight()
        {
            transform.DOMoveZ(-15, animationSpeed).SetSpeedBased(true).SetEase(easeType)
                     .OnComplete(() => MoveLeft());
        }
        #region Old Method
        void FixedUpdate()
        {
            OldMethod();
        }
        void OldMethod()
        {
            //if (Mathf.Abs((int)_movingBridge.localPosition.z) <= WidthOfPlatform)
            //{
            //    _movingBridge.localPosition += (_sign) * (new Vector3(0, 0, (PlatformSpeed) * Time.fixedDeltaTime));
            //}
            //if ((int)_movingBridge.localPosition.z > WidthOfPlatform)
            //{
            //    _movingBridge.localPosition = new Vector3(_movingBridge.localPosition.x, _movingBridge.localPosition.y, WidthOfPlatform);
            //    _sign = -_sign;
            //}
            //if ((int)_movingBridge.localPosition.z < -(WidthOfPlatform))
            //{
            //    _movingBridge.localPosition = new Vector3(_movingBridge.localPosition.x, _movingBridge.localPosition.y, -(WidthOfPlatform));
            //    _sign = -_sign;
            //}
        }
        #endregion
    }
}
