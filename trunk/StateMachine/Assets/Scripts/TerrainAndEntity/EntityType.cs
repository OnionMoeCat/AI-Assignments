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

    public class Entity
    {
        public EntityType EntityType = EntityType.Nothing;
        public Color Color = Color.black;
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
            {EntityType.LockedDoor, Resources.Load<Sprite>("Sprite/lock.png")},
            {EntityType.Key, Resources.Load<Sprite>("Sprite/key.png")},
            {EntityType.OpenedDoor, Resources.Load<Sprite>("Sprite/unlock.png")},
            {EntityType.Treasure, Resources.Load<Sprite>("Sprite/treasure.png")}
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