namespace navdi3
{

    using UnityEngine;
    using System.Collections;

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [ExecuteAlways]

    public class SmartSetupRigidbody2D : MonoBehaviour
    {

        public SpriteRenderer spriter { get { return GetComponent<SpriteRenderer>(); } }
        public BoxCollider2D box { get { return GetComponent<BoxCollider2D>(); } }
        public Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }

        // Update is called once per frame
        void Update()
        {
            if (body != null)
            {
                body.freezeRotation = true;
                body.bodyType = RigidbodyType2D.Dynamic;
                body.gravityScale = 0;
                body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                box.size = new Vector2(4, 8);

                Object.DestroyImmediate(this);
            }
        }
    }

}