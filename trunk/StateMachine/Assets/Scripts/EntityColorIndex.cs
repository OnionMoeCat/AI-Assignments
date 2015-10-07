using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AISandbox
{
    public class EntityColorIndex
    {
        private static Dictionary<Color, int> m_doorColorDict = new Dictionary<Color, int>()
        {
            {Color.red, 0},
            {Color.green, 1},
            {Color.blue, 2}
        };

        public static int GetIndex(Color i_color)
        {
            return m_doorColorDict[i_color];
        }

        public static int GetColorLength()
        {
            return m_doorColorDict.Count;
        }
    }

}

