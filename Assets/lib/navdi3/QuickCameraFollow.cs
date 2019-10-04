namespace navdi3
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class QuickCameraFollow : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            Camera.main.transform.position = this.transform.position + Vector3.back * 10;
        }
    }


}