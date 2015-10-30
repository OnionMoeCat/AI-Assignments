using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_HasValidPath: BT_BaseNode 
    {
        public BT_HasValidPath(List<BT_BaseNode> i_children) : base(i_children)
        {
        }
        public override void Open(BT_Tick tick)
        {
            Boolean lastOpenNodes = tick.Blackboard.Get("isPathValid", tick.Tree.Id) as Boolean;
            if (lastOpenNodes == null)
            {
                tick.Blackboard.Set("isPathValid", false, tick.Tree.Id);
            }
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            Boolean lastOpenNodes = tick.Blackboard.Get("isPathValid", tick.Tree.Id) as Boolean;
            if (lastOpenNodes == null)
            {
                return BT_Status.ERROR;
            }
            if (lastOpenNodes)
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
