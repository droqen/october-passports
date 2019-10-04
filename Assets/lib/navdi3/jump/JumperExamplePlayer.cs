namespace navdi3.jump
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class JumperExamplePlayer : MonoBehaviour
    {
        public KeyCode[] jumpKeys = { KeyCode.Z, KeyCode.Space, KeyCode.UpArrow, KeyCode.W, };

        public Jumper jumper { get { return GetComponent<Jumper>(); } }

        private void Update()
        {
            foreach (var jumpKey in jumpKeys) if (Input.GetKeyDown(jumpKey)) { jumper.PinJump(); break; }
        }
        private void FixedUpdate()
        {
            jumper.pin = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            bool jumpHeld = false;
            foreach (var jumpKey in jumpKeys) if (Input.GetKey(jumpKey)) { jumpHeld = true; break; }
            if (!jumpHeld) jumper.PinJumpRelease();
        }
    }
}