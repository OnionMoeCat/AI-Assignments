using UnityEngine;
using System.Collections;
using UnityEngineInternal;

namespace AISandbox
{
    public class EntityManager : MonoBehaviour
    {
        private static GridNode[] doorNodes = new GridNode[EntityColorIndex.GetColorLength()];
        private static GridNode[] keyNodes = new GridNode[EntityColorIndex.GetColorLength()];
        private static GridNode treasure;

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
    }

}

