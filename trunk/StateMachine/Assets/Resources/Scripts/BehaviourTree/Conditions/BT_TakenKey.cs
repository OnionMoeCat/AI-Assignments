﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_TakenKey: BT_BaseNode
    {
        private uint index;
        public BT_TakenKey(uint i_index)
        {
            index = i_index;
        }
        public override BT_Status Tick(BT_Tick tick)
        {
            if (EntityManager.Keys[index].IsTaken)
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