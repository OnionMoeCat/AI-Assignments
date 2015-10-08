using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using UnityEngine;

namespace AISandbox
{
    class EndState : StateMachine<PathfollowingController>.State
    {
        private GridNode m_target;
        List<GridNode> m_path;
        private int m_path_index;
        private string m_name = "SeekKey";

        public override string Name
        {
            get
            {
                return m_name;
            }
        }

        public override void Enter(PathfollowingController i_pathfollowingController)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Pathfollowing");
            if (temp != null)
            {
                temp.GetComponent<Pathfollowing>().Reset();
            }
        }

        public override void Update(PathfollowingController i_pathfollowingController)
        {

        }

        public override void Exit(PathfollowingController i_pathfollowingController)
        {

        }

        public override bool OnMessage(PathfollowingController i_pathfollowingController, Telegram msg)
        {
            return true;
        }
    }
}
