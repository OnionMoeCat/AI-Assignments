using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_CalPath: BT_BaseNode
    {
        private GridNode target;
        public BT_CalPath(List<BT_BaseNode> i_children, GridNode i_target) : base(i_children)
        {
            target = i_target;
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            PathfollowingController controller = tick.Target as PathfollowingController;
            if (controller == null)
            {
                return BT_Status.ERROR;
            }
            OrientedActor orientedActor = controller.GetComponent<OrientedActor>();
            if (orientedActor == null)
            {
                return BT_Status.ERROR;
            }
            Grid grid = controller.Grid;
            GridNode current = grid.GetGridForPosition(controller.transform.position);
            if (current == null)
            {
                return BT_Status.FAILURE;
            }
            List<GridNode> path;
            if (AStar.GetShortestPath(orientedActor, current, target, grid.diagnoal, out path))
            {
                tick.Blackboard.Set("path", path, tick.Tree.Id);
                tick.Blackboard.Set("pathIndex", 0, tick.Tree.Id);
                return BT_Status.SUCCESS;
            }
            else
            {
                return BT_Status.FAILURE;
            }            
        }
    }
}
