using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_TakenTreasure: BT_BaseNode
    {
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
