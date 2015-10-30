using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_Inverter: BT_BaseNode
    {
        public override BT_Status Tick(BT_Tick tick)
        {
            BT_BaseNode child = children[0];

            if (child == null)
            {
                return BT_Status.ERROR;
            }

            BT_Status status = child._execute(tick);

            if (status == BT_Status.SUCCESS)
            {
                status = BT_Status.FAILURE;
            }
            else if (status == BT_Status.FAILURE)
            {
                status = BT_Status.SUCCESS;
            }

            return status;
        }
    }
}
