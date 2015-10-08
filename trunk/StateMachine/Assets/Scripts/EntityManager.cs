using UnityEngine;
using System.Collections;
using UnityEngineInternal;

namespace AISandbox
{
    public class EntityManager : MonoBehaviour
    {
        private static bool[] doorOpen = new bool[EntityColorIndex.GetColorLength()];
        private static bool[] keyPick = new bool[EntityColorIndex.GetColorLength()];
        private static bool treasurePick = false;
        private static GridNode[] doorNodes = new GridNode[EntityColorIndex.GetColorLength()];
        private static GridNode[] keyNodes = new GridNode[EntityColorIndex.GetColorLength()];
        private static GridNode treasure;

        private static Pathfollowing pathfollowing;

        public static GridNode[] DoorNodes 
        {
            get { return doorNodes; }
            set { doorNodes = value; }
        }

        public static GridNode[] KeyNodes
        {
            get { return keyNodes; }
            set { keyNodes = value; }
        }

        public static GridNode Treasure
        {
            get { return treasure; }
            set { treasure = value; }
        }

        public static bool[] DoorOpen
        {
            get { return doorOpen; }
        }

        public static bool[] KeyPick
        {
            get { return keyPick; }
        }

        public static bool TreasurePick
        {
            get { return treasurePick; }
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
                        for (int i = 0; i < keyNodes.Length; i++)
                        {
                            if (keyNodes[i] == gridNode)
                            {
                                pfc.Keys[EntityColorIndex.GetIndex(gridNode.EntityColor)] ++;
                                keyPick[i] = true;
                                gridNode.EntityType = EntityType.Nothing;
                                break;
                            }
                        }
                        break;
                    case FSMMsgType.OPENDOOR:
                        for (int i = 0; i < doorNodes.Length; i++)
                        {
                            if (doorNodes[i] == gridNode)
                            {
                                pfc.Keys[EntityColorIndex.GetIndex(gridNode.EntityColor)] --;
                                doorOpen[i] = true;
                                gridNode.EntityType = EntityType.OpenedDoor;
                                break;
                            }
                        }
                        break;
                    case FSMMsgType.PICKUPTREASURE:
                        if (treasure == gridNode)
                        {
                            treasurePick = true;
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
            for (int i = 0; i < doorNodes.Length; i++)
            {
                if (doorNodes[i] == i_gridNode)
                {
                    doorNodes[i] = null;
                }
            }

            for (int i = 0; i < keyNodes.Length; i++)
            {
                if (keyNodes[i] == i_gridNode)
                {
                    keyNodes[i] = null;
                }
            }

            if (treasure == i_gridNode)
            {
                treasure = null;
            }
        }

        public static bool IsReady()
        {        
            for (int i = 0; i < doorNodes.Length; i++)
            {
                if (doorNodes[i] == null)
                {
                    return false;
                }
            }

            for (int i = 0; i < keyNodes.Length; i++)
            {
                if (keyNodes[i] == null)
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
            for (int i = 0 ; i < doorOpen.Length ; i++)
            {
                doorOpen[i] = false;
            }
            for (int i = 0; i < keyPick.Length; i++)
            {
                keyPick[i] = false;
            }
            for (int i = 0; i < doorNodes.Length; i++)
            {
                doorNodes[i].EntityType = EntityType.LockedDoor;
            }
            for (int i = 0; i < KeyNodes.Length; i++)
            {
                KeyNodes[i].EntityType = EntityType.Key;
            }
            treasure.EntityType = EntityType.Treasure;
        }

        void Awake()
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Pathfollowing");
            if (temp)
            {
                pathfollowing = temp.GetComponent<Pathfollowing>();
            }
        }

        void Update()
        {
            for (int i = 0; i < doorNodes.Length; i++)
            {
                foreach (PathfollowingController actor in pathfollowing.Actors)
                {
                    if (doorNodes[i].Intersect(actor))
                    {
                        Telegram telegram = new Telegram();
                        telegram.messageType = FSMMsgType.GETTODOOR;
                        telegram.content = actor;
                        actor.HandleMessage(telegram);
                    }
                }                
            }

            for (int i = 0; i < keyNodes.Length; i++)
            {
                foreach (PathfollowingController actor in pathfollowing.Actors)
                {
                    if (keyNodes[i].Intersect(actor))
                    {
                        Telegram telegram = new Telegram();
                        telegram.messageType = FSMMsgType.GETTOKEY;
                        telegram.content = actor;
                        actor.HandleMessage(telegram);
                    }
                }
            }

            foreach (PathfollowingController actor in pathfollowing.Actors)
            {
                if (treasure.Intersect(actor))
                {
                    Telegram telegram = new Telegram();
                    telegram.messageType = FSMMsgType.GETTOTREASURE;
                    telegram.content = actor;
                    actor.HandleMessage(telegram);
                }
            }
        }
    }

}

