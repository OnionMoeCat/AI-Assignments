using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_Priority: BT_BaseNode
    {
        public override BT_Status Tick(BT_Tick tick)
        {
            for (var i = 0; i < children.Count; i++)
            {
                var status = this.children[i]._execute(tick);

                if (status != BT_Status.FAILURE)
                {
                    return status;
                }
            }

            return BT_Status.FAILURE;
        }
    }
}
}
