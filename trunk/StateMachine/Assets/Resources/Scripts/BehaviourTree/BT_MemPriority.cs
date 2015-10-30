using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_MemPriority: BT_BaseNode
    {
        public override void Open(BT_Tick tick)
        {
            tick.Blackboard.Set("runningChild", 0, tick.Tree.Id, this.id);
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            int child = tick.Blackboard.Get("runningChild", tick.Tree.Id, id) as Integer;
            for (int i = child; i < children.Count; i++)
            {
                var status = children[i]._execute(tick);

                if (status != BT_Status.FAILURE)
                {
                    if (status == BT_Status.RUNNING)
                    {
                        tick.Blackboard.Set("runningChild", i, tick.Tree.Id, this.id);
                    }
                    return status;
                }
            }

            return BT_Status.FAILURE;
        }
    }
}
