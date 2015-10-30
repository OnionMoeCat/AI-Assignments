using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_BehaviourTree
    {
        string id = BT_Constant.CreateUUID();
        public string Id
        {
            get
            {
                return id;
            }
        }
        BT_BaseNode root;
        public void Tick(object target, BT_BlackBoard blackboard)
        {
            /* CREATE A TICK OBJECT */
            BT_Tick tick = new BT_Tick();
            tick.Target = target;
            tick.Blackboard = blackboard;
            tick.Tree = this;

            /* TICK NODE */
            root._execute(tick);

            /* CLOSE NODES FROM LAST TICK, IF NEEDED */
            List<BT_BaseNode> lastOpenNodes = blackboard.Get("openNodes", this.id) as List<BT_BaseNode>;
            List<BT_BaseNode> currOpenNodes = tick.OpenNodes as List<BT_BaseNode>;

            // does not close if it is still open in this tick
            var start = 0;
            for (var i = 0; i < Math.Min(lastOpenNodes.Count, currOpenNodes.Count); i++)
            {
                start = i + 1;
                if (lastOpenNodes[i] != currOpenNodes[i])
                {
                    break;
                }
            }

            // close the nodes
            for (var i = lastOpenNodes.Count - 1; i >= start; i--)
            {
                lastOpenNodes[i]._close(tick);
            }

            /* POPULATE BLACKBOARD */
            blackboard.Set("openNodes", currOpenNodes, this.id);
            blackboard.Set("nodeCount", tick.OpenNodes.Count, this.id);
        }
    }
}
