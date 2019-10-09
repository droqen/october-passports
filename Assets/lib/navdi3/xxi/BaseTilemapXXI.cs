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

        // if loading from loader

        protected void InitializeTileSystem()
        {
            loader.SetupTileset(sprites, GetSolidTileIds(), GetSpawnTileIds());
            loader.PlaceTiles(loader.Load(firstLevel), tilemap, this.SpawnTileId);
        }

        // if using the 'tt' system

        public void InitializeManualTT()
        {
            loader.SetupTileset(sprites, GetSolidTileIds(), GetSpawnTileIds());
            tts = new Dictionary<twin, int>();
        }
        public void ClearAllTilesTT()
        {
            tts.Clear();
            tilemap.ClearAllTiles();
        }

        Dictionary<twin, int> tts;
        public void Sett(twin cell, int TileId)
        {
            tts[cell] = TileId;
            tilemap.SetTile(cell, loader.tileset[TileId]);
        }
        public int Gett(twin cell)
        {
            if (tts.TryGetValue(cell, out var TileId)) return TileId;
            else return default(int);
        }
    }
}