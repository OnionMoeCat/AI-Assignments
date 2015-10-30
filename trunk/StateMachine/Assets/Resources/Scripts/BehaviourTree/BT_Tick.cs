using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_Tick
    {
        private BT_BehaviourTree tree;
        public BT_BehaviourTree Tree
        {
            get
            {
                return tree;
            }
            set
            {
                tree = value;
            }
        }
        List<BT_BaseNode> openNodes = new List<BT_BaseNode>();
        public List<BT_BaseNode> OpenNodes
        {
            get
            {
                return openNodes;
            }
            set
            {
                openNodes = value;
            }
        }
        object target;
        public object Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }
        BT_BlackBoard blackboard;
        public BT_BlackBoard Blackboard
        {
            get
            {
                return blackboard;
            }
            set
            {
                blackboard = value;
            }
        }


        public void EnterNode(BT_BaseNode node)
        {
            openNodes.Add(node);
        }

        public void OpenNode(BT_BaseNode node)
        {
        }

        public void TickNode(BT_BaseNode node)
        {
        }

        public void CloseNode(BT_BaseNode node)
        {
            openNodes.Remove(node);
        }

        public void ExitNode(BT_BaseNode node)
        {
        }
    }
}
