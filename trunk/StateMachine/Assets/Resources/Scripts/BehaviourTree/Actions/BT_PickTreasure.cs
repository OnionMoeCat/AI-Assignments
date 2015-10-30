using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_PickTreasure: BT_BaseNode
    {
        public BT_PickTreasure(List<BT_BaseNode> i_children) : base(i_children)
        {
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            EntityManager.Treasure.IsTaken = true;
            EntityManager.Treasure.GridNode.EntityType = EntityType.Nothing;
            return BT_Status.SUCCESS;
        }
    }
}
