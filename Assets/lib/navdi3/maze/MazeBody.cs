namespace navdi3.maze
{

    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class MazeBody : MonoBehaviour
    {
        virtual public bool CanMoveTo(twin target_pos) { return !IsSolid(target_pos); }
        virtual public void OnSetup() { }
        virtual public void OnMoved(twin prev_pos, twin target_pos) { }

        public twin lastMove { get; protected set; }

        [HideInInspector] public MazeMaster master;
        twin _my_cell_pos;
        public twin my_cell_pos {

            get { return _my_cell_pos; }

            set {
                if (_my_cell_pos != value)
                {
                    if (master == null)
                    {
                        Dj.Error("MazeBody can't set my_cell_pos without a MazeMaster"); return;
                    }
                    var prev_pos = _my_cell_pos;
                    if (this.isActiveAndEnabled)
                    {
                        master.Unregister(this, _my_cell_pos);
                        master.Register(this, value);
                    }
                    _my_cell_pos = value;
                    OnMoved(prev_pos, value);
                }
            }
        }

        public bool TryMove(twin move)
        {
            if (move == twin.zero) return false; // this isn't a move at all.

            var target_pos = my_cell_pos + move;
            if (CanMoveTo(target_pos))
            {
                lastMove = move;
                my_cell_pos = target_pos;
                return true;
            } else
            {
                return false;
            }
        }

        public bool TryMoves(bool shuffled = false, params twin[] moves)
        {
            if (shuffled) Util.shufl(ref moves);
            for(int i=0;i<moves.Length;i++)
            {
                if (TryMove(moves[i])) return true;
            }
            return false;
        }

        public void Setup(MazeMaster master, twin cell_pos)
        {
            if (master==null) throw new System.Exception("MazeBody.Setup bad param: MazeMaster is NULL");

            this.master = master;
            this.my_cell_pos = cell_pos;

            transform.position = master.grid.GetCellCenterWorld(cell_pos);

            this.OnSetup();
        }

        public Vector3 ToCentered()
        {
            return GetCellCenterWorld(this.my_cell_pos) - transform.position;
        }

        public bool IsWithinDistOfCentered(float maxDist = 8f)
		{
			return ToCentered().sqrMagnitude <= maxDist * maxDist;
		}

        public Vector3 GetCellCenterWorld(twin cell_pos)
        {
            return master.grid.GetCellCenterWorld(cell_pos);
        }


        public HashSet<MazeBody> GetMazeBodiesNear(float maxDist = 8f, Vector3? position = null, bool exclude_self = true)
        {
            if (!position.HasValue) position = transform.position;

            var maxCellDist = maxDist / master.grid.cellSize.x;
            var cellBodies = master.GetBodiesNearCell(new twin(master.grid.WorldToCell(position.Value)), maxCellDist);
            var nearBodies = new HashSet<MazeBody>();
            foreach(var mazeBody in cellBodies)
            {
                if ( (mazeBody.transform.position - position.Value).sqrMagnitude <= maxDist * maxDist )
                {
                    nearBodies.Add(mazeBody);
                }
            }
            if (exclude_self) nearBodies.Remove(this);
            return nearBodies;
        }

        protected bool IsSolid(twin pos)
        {
            var tile = master.tilemap.GetTile(pos);
            if (tile == null || ((UnityEngine.Tilemaps.Tile)tile).colliderType == UnityEngine.Tilemaps.Tile.ColliderType.None)
            {
                return false; // not solid
            } else
            {
                return true;
            }
        }

        private void OnEnable()
        {
            if (master != null) master.Register(this, my_cell_pos);
        }
        private void OnDisable()
        {
            if (master != null) master.Unregister(this, my_cell_pos);
        }
    }

}
