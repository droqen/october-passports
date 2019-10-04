namespace navdi3.xxi
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using System.Collections;
    using System.Collections.Generic;

    using navdi3.tiled;

    [RequireComponent(typeof(BankLot))]
    [RequireComponent(typeof(SpriteLot))]
    [RequireComponent(typeof(TiledLoader))]

    abstract public class BaseTilemapXXI : MonoBehaviour
    {
        public BankLot banks { get { return GetComponent<BankLot>(); } }
        public SpriteLot sprites { get { return GetComponent<SpriteLot>(); } }
        public TiledLoader loader { get { return GetComponent<TiledLoader>(); } }

        public Tilemap tilemap; // must be assigned
        public TextAsset firstLevel; // load first level

        abstract public int[] GetSolidTileIds();
        abstract public int[] GetSpawnTileIds();
        abstract public void SpawnTileId(int TileId, Vector3Int TilePos);

        protected Dictionary<string, EntityLot> entlots = new Dictionary<string, EntityLot>();
        protected EntityLot GetEntLot(string entLotName)
        {
            if (!entlots.ContainsKey(entLotName)) entlots.Add(entLotName, EntityLot.NewEntLot(entLotName));
            return entlots[entLotName];
        }
        protected void InitializeTileSystem()
        {
            loader.SetupTileset(sprites, GetSolidTileIds(), GetSpawnTileIds());
            loader.PlaceTiles(loader.Load(firstLevel), tilemap, this.SpawnTileId);
        }
    }
}