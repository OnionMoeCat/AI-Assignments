using UnityEngine;
using System.Collections.Generic;

namespace AISandbox
{
    public enum TerrainType
    {
        Plain,
        Forest,
        Swamp,
        Impassable
    };

    public class TerrainTypeManager
    {
        private static Dictionary<TerrainType, float> m_terrainCostDict = new Dictionary<TerrainType, float>()
        {
            {TerrainType.Plain, 1},
            {TerrainType.Forest, 2},
            {TerrainType.Swamp, 3},
            {TerrainType.Impassable, 1}
        };
        private static Dictionary<TerrainType, bool> m_terrainPassabletDict = new Dictionary<TerrainType, bool>()
        {
            {TerrainType.Plain, true},
            {TerrainType.Forest, true},
            {TerrainType.Swamp, true},
            {TerrainType.Impassable, false}
        };
        private static Dictionary<TerrainType, Color> m_terrainColortDict = new Dictionary<TerrainType, Color>()
        {
            {TerrainType.Plain, Color.gray},
            {TerrainType.Forest, Color.green},
            {TerrainType.Swamp, Color.blue},
            {TerrainType.Impassable, Color.black}
        };
        public static float GetCost(TerrainType i_terrainType)
        {
            return m_terrainCostDict[i_terrainType];
        }

        public static bool GetPassable(TerrainType i_terrainType)
        {
            return m_terrainPassabletDict[i_terrainType];
        }

        public static Color GetColor(TerrainType i_terrainType)
        {
            return m_terrainColortDict[i_terrainType];
        }
    }
}