using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_OpenedDoor: BT_BaseNode
    {
        private uint index;
        public BT_OpenedDoor(uint i_index)
        {
            index = i_index;
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            if (EntityManager.Doors[index].IsTaken)
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
