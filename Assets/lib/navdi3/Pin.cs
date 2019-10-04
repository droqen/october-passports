namespace navdi3
{
    using UnityEngine;
    using System.Collections;

    public static class Pin
    {
        public static twin GetTwinDown()
        {
            return new twin(
                (Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : 0) + (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0),
                (Input.GetKeyDown(KeyCode.DownArrow) ? -1 : 0) + (Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0)
                );
        }
    }

}