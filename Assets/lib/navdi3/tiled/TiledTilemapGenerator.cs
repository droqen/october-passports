namespace navdi3.tiled
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [ExecuteAlways]

    public class TiledTilemapGenerator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.AddComponent<Tilemap>();
            gameObject.AddComponent<CompositeCollider2D>();
            gameObject.AddComponent<TilemapCollider2D>().usedByComposite = true;
            gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            this.enabled = false;
        }
    }

}