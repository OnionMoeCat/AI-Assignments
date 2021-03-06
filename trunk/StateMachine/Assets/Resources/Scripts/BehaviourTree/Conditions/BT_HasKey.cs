﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_HasKey: BT_BaseNode
    {
        private uint index;
        public BT_HasKey(List<BT_BaseNode> i_children, uint i_index) : base(i_children)
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
            OrientedActor orientedActor = controller.GetComponent<OrientedActor>();
            if (orientedActor == null)
            {
                return BT_Status.ERROR;
            }
            if (orientedActor.Keys[index] > 0)
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
