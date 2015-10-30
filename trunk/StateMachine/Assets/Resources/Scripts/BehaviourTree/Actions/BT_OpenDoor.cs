using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_OpenDoor: BT_BaseNode
    {
        private uint index;
        public BT_OpenDoor(List<BT_BaseNode> i_children, uint i_index) : base(i_children)
        {
            index = i_index;
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            PathfollowingController controller = tick.Target as PathfollowingController;
            if (controller == null)
            {
                return BT_Status.ERROR;
            }
            Entity entity = EntityManager.Doors[index];
            entity.IsTaken = true;
            entity.GridNode.EntityType = EntityType.OpenedDoor;
            OrientedActor orientedActor = controller.GetComponent<OrientedActor>();
            if (orientedActor == null)
            {
                return BT_Status.ERROR;
            }
            orientedActor.Keys[index] -= 1;
            return BT_Status.SUCCESS;
        }
    }
}
