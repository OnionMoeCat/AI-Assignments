using UnityEngine;
using System.Collections.Generic;

namespace AISandbox
{
    public enum EntityType
    {
        Nothing,
        LockedDoor,
        Key,
        Treasure,
        OpenedDoor
    };

    public struct Entity
    {
        public EntityType EntityType;
        public Color Color;
    };

    public class EntityTypeManager
    {
        private static Dictionary<EntityType, bool> m_entityPassableDict = new Dictionary<EntityType, bool>()
        {
            {EntityType.Nothing, true},
            {EntityType.LockedDoor, false},
            {EntityType.Key, true},
            {EntityType.OpenedDoor, true},
            {EntityType.Treasure, true}
        };

        private static Dictionary<EntityType, Sprite> m_entitySpriteDict = new Dictionary<EntityType, Sprite>()
        {
            {EntityType.Nothing, null},
            {EntityType.LockedDoor, Resources.Load<Sprite>("Sprites/lock")},
            {EntityType.Key, Resources.Load<Sprite>("Sprites/key")},
            {EntityType.OpenedDoor, Resources.Load<Sprite>("Sprites/unlock")},
            {EntityType.Treasure, Resources.Load<Sprite>("Sprites/treasure")}
        };

        public static bool GetPassable(EntityType i_entityType)
        {
            return m_entityPassableDict[i_entityType];
        }

        public static Sprite GetSprite(EntityType i_entityType)
        {
            return m_entitySpriteDict[i_entityType];
        }
    }
}