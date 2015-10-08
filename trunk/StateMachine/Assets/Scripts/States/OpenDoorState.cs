﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using UnityEngine;

namespace AISandbox
{
    class OpenDoorState : StateMachine<PathfollowingController>.State
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
            bool getAllDoors = true;
            m_target = null;
            m_path = null;
            m_path_index = -1;
            for (uint i = 0; i < EntityManager.DoorOpen.Length; i++)
            {
                if (EntityManager.DoorOpen[i] == false)
                {
                    getAllDoors = false;
                    Grid grid = i_pathfollowingController.Grid;
                    GridNode target = EntityManager.DoorNodes[i];
                    GridNode current = grid.GetGridForPosition(i_pathfollowingController.transform.position);
                    if (current != null && i_pathfollowingController.Keys[EntityColorIndex.GetIndex(EntityManager.DoorNodes[i].EntityColor)] > 0)
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
            if (getAllDoors)
            {
                i_pathfollowingController.StateMachine.SetActiveState("GetTreasure");
                return;
            }
            if (m_path == null)
            {
                if (i_pathfollowingController.StateMachine.PreviousState.Name == "SeekKey")
                {
                    i_pathfollowingController.StateMachine.SetActiveState("End");
                }
                else
                {
                    i_pathfollowingController.StateMachine.SetActiveState("SeekKey");
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
                if (current == seeking)
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
            if (msg.messageType == FSMMsgType.GETTODOOR)
            {
                GridNode gridNode = msg.content as GridNode;
                if (gridNode != null)
                {
                    int index = EntityColorIndex.GetIndex(gridNode.EntityColor);
                    if (index >= 0)
                    {
                        if (i_pathfollowingController.Keys[index] > 0)
                        {
                            Telegram message = new Telegram();
                            message.messageType = FSMMsgType.OPENDOOR;
                            message.sender = i_pathfollowingController;
                            EntityManager.HandleMessage(message);
                            i_pathfollowingController.StateMachine.SetActiveState("OpenDoor");
                        }
                    }
                }
            }
            return true;
        }
    }
}
