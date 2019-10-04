namespace navdi3.jump
{

    using UnityEngine;
    using System.Collections;

    public class JumperSpriterExample : MonoBehaviour
    {
        public Jumper jumper { get { return GetComponent<Jumper>(); } }
        public SpriteRenderer spriter;
        public SpriteLot spriteLot;
        public int floorTurnFrameDuration = 4;
        public bool flipOnLeft = true;
        public bool flipOnRight = false;
        [Header("Sprite Indexes")]
        public int floorIdleSprite = -1;
        public int floorTurnSprite = -1;
        public int[] floorMoveSprites = { -1 };
        public int airSprite = -1;
        public int airJumpingSprite = -1;
        public int airFallingSprite = -1;

        [Header("Animation mods & rates")]
        public float moveAnimSpeed = 0.25f;
        public int moveStartAnimFrame = 0;
        public int postTurnAnimFrame = 0;
        public int landingAnimFrame = 2;

        int floorTurnBuffer = 0;
        public float anim { get; set; }
        
        // Update is called once per frame
        void FixedUpdate()
        {
            if (floorTurnBuffer > 0) floorTurnBuffer--;

            bool moving = Mathf.Abs(jumper.pin.x) > float.Epsilon;

            if (moving)
            {
                var newFlip = jumper.faceleft ? flipOnLeft : flipOnRight;
                if (newFlip != spriter.flipX)
                {
                    spriter.flipX = newFlip;
                    floorTurnBuffer = floorTurnFrameDuration;
                }
            }

            if (jumper.IsFloored())
            {
                if (floorTurnBuffer > 0)
                {
                    anim = postTurnAnimFrame;
                    SetSprite(floorTurnSprite);
                } else if (moving)
                {
                    anim = (anim + moveAnimSpeed) % floorMoveSprites.Length;
                    SetSprite(floorMoveSprites, (int)anim);
                } else
                {
                    anim = moveStartAnimFrame;
                    SetSprite(floorIdleSprite);
                }
            } else
            {
                anim = landingAnimFrame;
                SetSprite(airSprite);
                if (jumper.body.velocity.y > 0) SetSprite(airJumpingSprite);
                else SetSprite(airFallingSprite);
            }
        }

        void SetSprite(int spriteIndex)
        {
            if (spriteIndex >= 0) spriter.sprite = spriteLot[spriteIndex];
        }
        void SetSprite(int[] spriteArray, int arrayIndex)
        {
            if (arrayIndex >= 0) SetSprite(spriteArray[arrayIndex]);
        }
    }
    
}
