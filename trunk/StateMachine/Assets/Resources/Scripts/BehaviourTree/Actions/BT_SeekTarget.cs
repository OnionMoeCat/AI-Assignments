using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_SeekTarget: BT_BaseNode
    {
        private GridNode target;
        public BT_SeekTarget(List<BT_BaseNode> i_children, GridNode i_target) : base(i_children)
        {
            target = i_target;
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            List<GridNode> path = tick.Blackboard.Get("path", tick.Tree.Id) as List<GridNode>;
            if (path == null)
            {
                return BT_Status.ERROR;
            }
            Integer pathIndex = tick.Blackboard.Get("pathIndex", tick.Tree.Id) as Integer;
            if (pathIndex == null)
            {
                return BT_Status.ERROR;
            }
            PathfollowingController controller = tick.Target as PathfollowingController;
            if (controller == null)
            {
                return BT_Status.ERROR;
            }
            Grid grid = controller.Grid;
            GridNode current = grid.GetGridForPosition(controller.transform.position);
            if (current == null)
            {
                controller.Idle();
                return BT_Status.RUNNING;
            }
            if (current == target)
            {
                return BT_Status.SUCCESS;
            }
            if (path.Count > 0)
            {
                GridNode seeking = path[pathIndex];
                if (current == seeking)
                {
                    pathIndex += 1;
                    seeking = path[pathIndex];
                    tick.Blackboard.Set("pathIndex", pathIndex, tick.Tree.Id);
                }
                controller.Seek(seeking);
                return BT_Status.RUNNING;
            }
            else
            {
                return BT_Status.ERROR;
            }
        }
    }
}
