using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AISandbox
{
    class InputManager: MonoBehaviour
    {
        private bool m_enableEdit = true;
        public bool EnableEdit
        {
            get
            {
                return m_enableEdit;
            }
            set
            {
                m_enableEdit = value;
            }
        }

        private Grid _grid = null;
        private ButtonManager _buttonManager;
        private TerrainType _draw_terrain_type;
        private EntityType _draw_entity_type;
        private Color _draw_color;
        void Start()
        {
            _grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
            _buttonManager = GameObject.FindGameObjectWithTag("ButtonManager").GetComponent<ButtonManager>();
        }
        void Update()
        {
            if (m_enableEdit && Input.GetMouseButton(0))
            {
                GridNode node = _grid.GetGridFromViewport(Input.mousePosition);
                if (node != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _draw_terrain_type = _buttonManager.TerrainType;
                        _draw_entity_type = _buttonManager.EntityType;
                        _draw_color = _buttonManager.Color;
                    }

                    if (_draw_entity_type == EntityType.Nothing)
                    {
                        if (node.TerrainType != _draw_terrain_type)
                        {
                            node.TerrainType = _draw_terrain_type;
                        }
                    }
                    else
                    {
                        if (node.EntityType != _draw_entity_type || node.EntityColor != _draw_color)
                        {
                            if (_draw_entity_type == EntityType.LockedDoor)
                            {
                                int index = EntityColorIndex.GetIndex(_draw_color);
                                Debug.Assert(index >= 0 && index < EntityColorIndex.GetColorLength());
                                EntityManager.DereferenceEntityAt(node);
                                GridNode old = EntityManager.Doors[index].GridNode;
                                if (old != null)
                                {
                                    old.EntityType = EntityType.Nothing;
                                }
                                EntityManager.Doors[index].GridNode = node;
                            }
                            if (_draw_entity_type == EntityType.Key)
                            {
                                int index = EntityColorIndex.GetIndex(_draw_color);
                                Debug.Assert(index >= 0 && index < EntityColorIndex.GetColorLength());
                                EntityManager.DereferenceEntityAt(node);
                                GridNode old = EntityManager.Keys[index].GridNode;
                                if (old != null)
                                {
                                    old.EntityType = EntityType.Nothing;
                                }
                                EntityManager.Keys[index].GridNode = node;
                            }
                            if (_draw_entity_type == EntityType.Treasure)
                            {
                                EntityManager.DereferenceEntityAt(node);
                                if (EntityManager.Treasure.GridNode != null)
                                {
                                    EntityManager.Treasure.GridNode.EntityType = EntityType.Nothing;
                                }
                                EntityManager.Treasure.GridNode = node;
                            }
                            node.EntityType = _draw_entity_type;
                            node.EntityColor = _draw_color;
                        }
                    }
                }
            }
        }
    }
}
