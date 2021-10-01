using UnityEngine;

namespace Count_Master_SAY.Control
{
    public class PlayerFollowers : MonoBehaviour
    {
        public GameObject backGround;

        void Update()
        {
            Vector3 pos = new Vector3(this.transform.position.x, 0, 0);
            backGround.transform.position = pos;
        }
    }
}