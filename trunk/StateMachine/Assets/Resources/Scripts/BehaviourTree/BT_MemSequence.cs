using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_MemSequence: BT_BaseNode
    {
        public BT_MemSequence(List<BT_BaseNode> i_children) : base(i_children)
        {
        }
        public override void Open(BT_Tick tick)
        {
            tick.Blackboard.Set("runningChild", 0, tick.Tree.Id, this.id);
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            Integer child = tick.Blackboard.Get("runningChild", tick.Tree.Id, id) as Integer;

            if (child == null)
            {
                return BT_Status.ERROR;
            }

            for (int i = child; i < children.Count; i++)
            {
                BT_Status status = children[i]._execute(tick);

                if (status != BT_Status.SUCCESS)
                {
                    if (status == BT_Status.RUNNING)
                    {
                        tick.Blackboard.Set("runningChild", i, tick.Tree.Id, this.id);
                    }
                    return status;
                }
            }

            return BT_Status.SUCCESS;
        }
    }
}
