using UnityEngine;
using System.Collections;
using UnityEngineInternal;

namespace AISandbox
{
    public class Entity
    {
        private GridNode gridNode;
        public GridNode GridNode
        {
            get
            {
                return gridNode;
            }
            set
            {
                gridNode = value;
            }
        }
        private bool isTaken;
        public bool IsTaken
        {
            get
            {
                return isTaken;
            }
            set
            {
                isTaken = value;
            }
        }              
    }
    public class EntityManager : MonoBehaviour
    {


        private static Entity[] doors = new Entity[EntityColorIndex.GetColorLength()];
        private static Entity[] keys = new Entity[EntityColorIndex.GetColorLength()];
        private static Entity treasure;

        public static Entity[] Doors 
        {
            get { return doors; }
            set { doors = value; }
        }

        public static Entity[] Keys
        {
            get { return keys; }
            set { keys = value; }
        }

        public static Entity Treasure
        {
            get { return treasure; }
            set { treasure = value; }
        }

        public static bool HandleMessage(Telegram msg)
        {
            GridNode gridNode = msg.content as GridNode;
            PathfollowingController pfc = msg.sender as PathfollowingController;
            if (gridNode != null)
            {
                switch (msg.messageType)
                {
                    case FSMMsgType.PICKUPKEY:
                        for (int i = 0; i < keys.Length; i++)
                        {
                            if (keys[i].GridNode == gridNode)
                            {
                                pfc.Keys[EntityColorIndex.GetIndex(gridNode.EntityColor)] ++;
                                keys[i].IsTaken = true;
                                gridNode.EntityType = EntityType.Nothing;
                                break;
                            }
                        }
                        break;
                    case FSMMsgType.OPENDOOR:
                        for (int i = 0; i < doors.Length; i++)
                        {
                            if (doors[i].GridNode == gridNode)
                            {
                                pfc.Keys[EntityColorIndex.GetIndex(gridNode.EntityColor)] --;
                                doors[i].IsTaken = true;
                                gridNode.EntityType = EntityType.OpenedDoor;
                                break;
                            }
                        }
                        break;
                    case FSMMsgType.PICKUPTREASURE:
                        if (treasure.GridNode == gridNode)
                        {
                            treasure.IsTaken = true;
                            gridNode.EntityType = EntityType.Nothing;
                        }
                        break;
                    default:
                        break;
                }
            }
        
            return true;
        }

        public static void DereferenceEntityAt(GridNode i_gridNode)
        {
            switch(i_gridNode.EntityType)
            {
                case EntityType.LockedDoor:
                    {
                        int index = EntityColorIndex.GetIndex(i_gridNode.EntityColor);
                        doors[index].GridNode = null;
                    }
                    break;
                case EntityType.Key:
                    {
                        int index = EntityColorIndex.GetIndex(i_gridNode.EntityColor);
                        keys[index].GridNode = null;
                    }
                    break;
                case EntityType.Treasure:
                    {
                        treasure.GridNode = null;
                    }
                    break;
                default:
                    break;
            }
        }

        public static bool IsReady()
        {        
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i] == null)
                {
                    return false;
                }
            }

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == null)
                {
                    return false;
                }
            }

            if (treasure == null)
            {
                return false;
            }

            return true;
        }

        public static void Reset()
        {
            for (int i = 0 ; i < doors.Length ; i++)
            {
                doors[i].IsTaken = false;
                doors[i].GridNode.EntityType = EntityType.LockedDoor;
            }
            for (int i = 0; i < doors.Length; i++)
            {
                keys[i].IsTaken = false;
                keys[i].GridNode.EntityType = EntityType.Key;
            }
            treasure.IsTaken = false;
            treasure.GridNode.EntityType = EntityType.Treasure;
        }

        public static bool GridPassable(GridNode i_gridnode, GridNode i_endNode, PathfollowingController i_pathfollowingController)
        {
            if (i_gridnode.EntityType != EntityType.LockedDoor)
            {
                return TerrainTypeManager.GetPassable(i_gridnode.TerrainType);
            }
            else
            {
                int index = EntityColorIndex.GetIndex(i_gridnode.EntityColor);
                return (i_pathfollowingController.Keys[index] > 0 && i_gridnode == i_endNode);
            }
        }

        public static void QueryEveryActor()
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Pathfollowing");
            Pathfollowing pathfollowing = null;
            if (temp)
            {
                pathfollowing = temp.GetComponent<Pathfollowing>();

                for (int i = 0; i < doors.Length; i++)
                {
                    foreach (PathfollowingController actor in pathfollowing.Actors)
                    {
                        if (doors[i].GridNode.Intersect(actor))
                        {
                            Telegram telegram = new Telegram();
                            telegram.messageType = FSMMsgType.GETTODOOR;
                            telegram.content = doors[i].GridNode;
                            actor.HandleMessage(telegram);
                        }
                    }
                }

                for (int i = 0; i < keys.Length; i++)
                {
                    foreach (PathfollowingController actor in pathfollowing.Actors)
                    {
                        if (keys[i].GridNode.Intersect(actor))
                        {
                            Telegram telegram = new Telegram();
                            telegram.messageType = FSMMsgType.GETTOKEY;
                            telegram.content = keys[i].GridNode;
                            actor.HandleMessage(telegram);
                        }
                    }
                }

                for (int i = 0; i < pathfollowing.Actors.Count; i ++)
                {
                    if (treasure.GridNode.Intersect(pathfollowing.Actors[i]))
                    {
                        Telegram telegram = new Telegram();
                        telegram.messageType = FSMMsgType.GETTOTREASURE;
                        telegram.content = treasure;
                        pathfollowing.Actors[i].HandleMessage(telegram);
                    }
                }
            }       
        }
    }

}

