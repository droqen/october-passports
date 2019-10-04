namespace navdi3.maze
{
    public class MazeBodyKinematic : MazeBody
    {
        override public bool CanMoveTo(twin target_pos) { return true; }
        virtual public void FixedUpdate()
        {
            my_cell_pos = new twin(master.grid.WorldToCell(this.transform.position));
        }
    }
}