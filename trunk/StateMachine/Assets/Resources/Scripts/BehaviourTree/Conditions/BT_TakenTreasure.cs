using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_TakenTreasure: BT_BaseNode
    {
        public BT_TakenTreasure(List<BT_BaseNode> i_children) : base(i_children)
        {
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            if (EntityManager.Treasure.IsTaken)
            {
                return BT_Status.SUCCESS;
            }
            else
            {
                return BT_Status.FAILURE;
            }
        }
    }
}
