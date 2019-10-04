namespace navdi3.maze
{

    using UnityEngine;
    using UnityEngine.Tilemaps;
    using System.Collections;
    using System.Collections.Generic;

    [RequireComponent(typeof(Grid))]

    public class MazeMaster : MonoBehaviour
    {
        public Grid grid { get { return GetComponent<Grid>(); } }

        public Tilemap tilemap;

        public Dictionary<twin, HashSet<MazeBody>> bodyHeaps;

        private void Awake()
        {
            bodyHeaps = new Dictionary<twin, HashSet<MazeBody>>();
        }

        static HashSet<MazeBody> noBodies = new HashSet<MazeBody>();

        public HashSet<MazeBody> GetBodiesAt(twin cell_pos)
        {
            if (bodyHeaps.TryGetValue(cell_pos, out var bodies))
                return bodies;
            else
                return noBodies;
        }

        public HashSet<MazeBody> GetBodiesNearCell(twin cell_pos, float cell_dist)
        {
            int max_int_cell_dist = Mathf.CeilToInt(cell_dist+.5f);
            twin max_rel_twin = twin.one * max_int_cell_dist;
            twinrect zone = new twinrect(cell_pos - max_rel_twin, cell_pos + max_rel_twin);
            var bodies = new HashSet<MazeBody>();
            zone.DoEach((test_pos) =>
            {
                if ((test_pos-cell_pos).sqrLength <= cell_dist * cell_dist)
                {
                    bodies.UnionWith(GetBodiesAt(test_pos));
                }
            });
            return bodies;
        }

        public void Register(MazeBody body, twin cell_pos)
        {
            if (!bodyHeaps.ContainsKey(cell_pos)) bodyHeaps[cell_pos] = new HashSet<MazeBody>();
            bodyHeaps[cell_pos].Add(body);
        }
        public void Unregister(MazeBody being, twin cell_pos)
        {
            if (bodyHeaps.ContainsKey(cell_pos)) bodyHeaps[cell_pos].Remove(being);
        }
    }

}
