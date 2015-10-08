using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using UnityEngine;

namespace AISandbox
{
    class SeekKeyState : StateMachine<PathfollowingController>.State
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
            bool getAllKeys = true;
            m_target = null;
            m_path = null;
            m_path_index = -1;
            for (uint i = 0; i < EntityManager.KeyPick.Length; i ++)
            {
                if (EntityManager.KeyPick[i] == false)
                {
                    getAllKeys = false;
                    Grid grid = i_pathfollowingController.Grid;
                    GridNode target = EntityManager.DoorNodes[i];
                    GridNode current = grid.GetGridForPosition(i_pathfollowingController.transform.position);
                    if (current != null)
                    {
                        List<GridNode> path;
                        if (AStar.GetShortestPath(current, target, grid.diagnoal, out path))
                        {
                            m_target = target;
                            m_path = path;
                            m_path_index = 1;
                            break;
                        }                       
                    }
                }
            }
            if (getAllKeys)
            {
                i_pathfollowingController.StateMachine.SetActiveState("OpenDoor");
                return;
            }
            if (m_path == null)
            {
                if (i_pathfollowingController.KeyNum > 0)
                {
                    i_pathfollowingController.StateMachine.SetActiveState("OpenDoor");
                }
                else
                {
                    i_pathfollowingController.StateMachine.SetActiveState("End");
                }
            }
        }

        public override void Update(PathfollowingController i_pathfollowingController)
        {
            if (m_path.Count > 1)
            {
                GridNode seeking = m_path[m_path_index];
                Grid grid = i_pathfollowingController.Grid;
                GridNode current = grid.GetGridForPosition(i_pathfollowingController.transform.position);
                if (current == seeking && m_path_index + 1 < m_path.Count)
                {
                    m_path_index += 1;
                    seeking = m_path[m_path_index];
                }
                if (seeking == m_target)
                {
                    i_pathfollowingController.Arrive(seeking);
                    return;
                }
                else
                {
                    i_pathfollowingController.Seek(seeking);
                    return;
                }
            }
            i_pathfollowingController.Idle();
        }

        public override void Exit(PathfollowingController i_pathfollowingController)
        {
            
        }

        public override bool OnMessage(PathfollowingController i_pathfollowingController, Telegram msg)
        {
            if (msg.messageType == FSMMsgType.GETTOKEY)
            {
                GridNode gridNode = msg.content as GridNode;
                if (gridNode != null)
                {
                    int index = EntityColorIndex.GetIndex(gridNode.EntityColor);
                    if (index >= 0)
                    {
                        Telegram message = new Telegram();
                        message.messageType = FSMMsgType.PICKUPKEY;
                        message.sender = i_pathfollowingController;
                        EntityManager.HandleMessage(message);
                        i_pathfollowingController.StateMachine.SetActiveState("SeekKey");
                    }
                }
            }
            return true;
        }
    }
}
