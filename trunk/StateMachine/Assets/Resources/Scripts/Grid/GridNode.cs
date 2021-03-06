﻿using UnityEngine;
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
            }
        }

        [SerializeField]
        private EntityType entityType;
        public EntityType EntityType
        {
            get
            {
                return entityType;
            }
            set
            {
                entityType = value;
            }
        }

        [SerializeField]
        private Color entityColor;
        public Color EntityColor
        {
            get
            {
                return entityColor;
            }
            set
            {
                entityColor = value;
            }
        }

        private void Awake()
        {
            sprite_renderer = GetComponent<SpriteRenderer>();
            child_sprite_renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (entityType != EntityType.LockedDoor)
            {
                sprite_renderer.color = TerrainTypeManager.GetColor(terrainType);
            }
            else
            {
                sprite_renderer.color = Color.black;
            }

            if (entityType == EntityType.Nothing)
            {
                child_sprite_renderer.enabled = false;
            }
            else
            {
                child_sprite_renderer.enabled = true;
                child_sprite_renderer.color = entityColor;
                child_sprite_renderer.sprite = EntityTypeManager.GetSprite(entityType);
            }
        }

        public IList<GridNode> GetNeighbors( bool include_diagonal = false ) {
            return grid.GetNodeNeighbors( row, column, include_diagonal );
        }
        public bool Intersect(PathfollowingController i_pathfollowingController)
        {
            float xMin = transform.position.x - sprite_renderer.bounds.extents.x;
            float xMax = transform.position.x + sprite_renderer.bounds.extents.x;
            float yMin = transform.position.y - sprite_renderer.bounds.extents.y;
            float yMax = transform.position.y + sprite_renderer.bounds.extents.y;
            Vector2 position = i_pathfollowingController.transform.position;
            return (position.x > xMin && position.x < xMax && position.y > yMin && position.y < yMax);
        }
    }
}