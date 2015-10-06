using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace AISandbox {
    public class GridNode : MonoBehaviour
    {
        public Grid grid;
        public int column;
        public int row;

        private SpriteRenderer sprite_renderer;

        private SpriteRenderer child_sprite_renderer;

        [SerializeField]
        private float cost;
        public float Cost
        {
            get { return cost; }
        }

        [SerializeField]
        private bool passable;
        public bool Passable
        {
            get { return passable; }
        }

        [SerializeField]
        private TerrainType terrainType;
        public TerrainType TerrainType
        {
            get
            {
                return terrainType;
            }
            set
            {
                terrainType = value;
                cost = TerrainTypeManager.GetCost(terrainType);
                passable = TerrainTypeManager.GetPassable(terrainType);
            }
        }

        [SerializeField]
        private Entity entity;
        public Entity Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
                passable = EntityTypeManager.GetPassable(entity.EntityType);
                if (!passable)
                {
                    terrainType = TerrainType.Impassable;
                }
            }
        }

        private void Awake()
        {
            entity = new Entity();
            sprite_renderer = GetComponent<SpriteRenderer>();
            child_sprite_renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            sprite_renderer.color = TerrainTypeManager.GetColor(terrainType);
            if (entity.EntityType == EntityType.Nothing)
            {
                child_sprite_renderer.enabled = false;
            }
            else
            {
                child_sprite_renderer.enabled = true;
                child_sprite_renderer.color = entity.Color;
                child_sprite_renderer.sprite = EntityTypeManager.GetSprite(entity.EntityType);
            }
        }

        public IList<GridNode> GetNeighbors( bool include_diagonal = false ) {
            return grid.GetNodeNeighbors( row, column, include_diagonal );
        }
    }
}