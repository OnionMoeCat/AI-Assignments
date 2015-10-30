using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_BaseNode
    {    
        protected string id = BT_Constant.CreateUUID();
        protected List<BT_BaseNode> children = new List<BT_BaseNode>();

        public void AddChildren(List<BT_BaseNode> i_children)
        {
            if (i_children != null)
            {
                for (var i = 0; i < children.Count; i++)
                {
                    children.Add(i_children[i]);
                }
            }
        }

        public BT_Status _execute(BT_Tick tick)
        {
            this._enter(tick);

            /* OPEN */
            if (tick.Blackboard.Get("isOpen", tick.Tree.Id, this.id) == null)
            {
                this._open(tick);
            }

            /* TICK */
            BT_Status status = this._tick(tick);

            /* CLOSE */
            if (status != BT_Status.RUNNING)
            {
                this._close(tick);
            }

            /* EXIT */
            _exit(tick);

            return status;
        }
        public void _enter(BT_Tick tick)
        {
            tick.EnterNode(this);
            Enter(tick);
        }
        public void _open(BT_Tick tick)
        {
            tick.OpenNode(this);
            tick.Blackboard.Set("isOpen", true, tick.Tree.Id, id);
            Open(tick);
        }
        public BT_Status _tick(BT_Tick tick)
        {
            tick.TickNode(this);
            return Tick(tick);
        }
        public void _close(BT_Tick tick)
        {
            tick.CloseNode(this);
            tick.Blackboard.Set("isOpen", false, tick.Tree.Id, id);
            Close(tick);
        }
        public void _exit(BT_Tick tick)
        {
            tick.ExitNode(this);
            Exit(tick);
        }
        public virtual void Enter(BT_Tick tick)
        {
        }
        public virtual void Open(BT_Tick tick)
        {
        }
        public virtual BT_Status Tick(BT_Tick tick)
        {
            return BT_Status.ERROR;
        }
        public virtual void Close(BT_Tick tick)
        {
        }
        public virtual void Exit(BT_Tick tick)
        {
        }
    }
}
