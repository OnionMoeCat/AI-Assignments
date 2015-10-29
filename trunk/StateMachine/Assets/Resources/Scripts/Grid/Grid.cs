﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AISandbox {
    public class Grid : MonoBehaviour
    {
        public GridNode gridNodePrefab;
        public Pathfollowing pathfollowing;
        public ButtonManager buttonManager;

        private GridNode[ , ] _nodes;
        private float _node_width;
        private float _node_height;

        private int _num_row;

        private int _num_column;

        private bool _diagnoal;

        private TerrainType _draw_terrain_type;
        private EntityType _draw_entity_type;
        private Color _draw_color;

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

        public bool diagnoal
        {
            get
            {
                return _diagnoal;
            }
            set
            {
                _diagnoal = value;
            }
        }

        private GridNode CreateNode( int row, int col ) {
            GridNode node = Instantiate<GridNode>( gridNodePrefab );
            node.name = string.Format( "Node {0}{1}", (char)('A'+row), col );
            node.grid = this;
            node.row = row;
            node.column = col;
            node.TerrainType = TerrainType.Plain;
            node.transform.parent = transform;
            node.gameObject.SetActive( true );
            return node;
        }

        public void Create(int rows, int columns) {
            _num_row = rows;
            _num_column = columns;
            _node_width = gridNodePrefab.GetComponent<Renderer>().bounds.size.x;
            _node_height = gridNodePrefab.GetComponent<Renderer>().bounds.size.y;
            Vector2 node_position = new Vector2( _node_width * 0.5f, _node_height * -0.5f );
            _nodes = new GridNode[ rows, columns ];
            for( int row = 0; row < rows; ++row ) {
                for( int col = 0; col < columns; ++col ) {
                    GridNode node = CreateNode( row, col );
                    node.transform.localPosition = node_position;
                    _nodes[ row, col ] = node;

                    node_position.x += _node_width;
                }
                node_position.x = _node_width * 0.5f;
                node_position.y -= _node_height;
            }
        }

        public Vector2 size {
            get {
                return new Vector2( _node_width * _nodes.GetLength( 1 ), _node_height * _nodes.GetLength( 0 ) );
            }
        }

        public GridNode GetNode( int row, int col ) {
            return _nodes[row, col];
        }

        public IList<GridNode> GetNodeNeighbors( int row, int col, bool include_diagonal = false ) {
            IList<GridNode> neighbors = new List<GridNode>();

            int start_row = Mathf.Max( row - 1, 0 );
            int start_col = Mathf.Max( col - 1, 0 );
            int end_row = Mathf.Min( row + 1, _nodes.GetLength( 0 ) - 1 );
            int end_col = Mathf.Min( col + 1, _nodes.GetLength( 1 ) - 1 );

            for( int row_index = start_row; row_index <= end_row; ++row_index ) {
                for( int col_index = start_col; col_index <= end_col; ++col_index ) {
                    if( include_diagonal || row_index == row || col_index == col ) {
                        neighbors.Add( _nodes[ row_index, col_index ] );
                    }
                }
            }
            return neighbors;
        }

        private void Update()
        {
            ProcessInput();
        }

        private void RemoveEntityAt(GridNode i_gridNode)
        {
            i_gridNode.EntityType = EntityType.Nothing;
        }


        public void ProcessInput()
        {
            if (m_enableEdit && Input.GetMouseButton(0))
            {
                Vector3 world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 local_pos = transform.InverseTransformPoint(world_pos);
                // This trick makes a lot of assumptions that the nodes haven't been modified since initialization.
                int column = Mathf.FloorToInt(local_pos.x / _node_width);
                int row = Mathf.FloorToInt(-local_pos.y / _node_height);
                if (row >= 0 && row < _nodes.GetLength(0)
                 && column >= 0 && column < _nodes.GetLength(1))
                {
                    GridNode node = _nodes[row, column];
                    if (Input.GetMouseButtonDown(0))
                    {
                        _draw_terrain_type = buttonManager.TerrainType;
                        _draw_entity_type = buttonManager.EntityType;
                        _draw_color = buttonManager.Color;
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
                                    RemoveEntityAt(old);
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
                                    RemoveEntityAt(old);
                                }
                                EntityManager.Keys[index].GridNode = node;
                            }
                            if (_draw_entity_type == EntityType.Treasure)
                            {
                                EntityManager.DereferenceEntityAt(node);
                                if (EntityManager.Treasure != null)
                                {
                                    RemoveEntityAt(EntityManager.Treasure.GridNode);
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

        public float GetGridCostForPosition(Vector2 i_position)
        {
            GridNode gridNode = GetGridForPosition(i_position);
            if (gridNode != null)
            {
                return gridNode.Cost;
            }
            else
            {
                return 1;
            }
        }

        public GridNode GetGridForPosition(Vector2 i_position)
        {
            // This trick makes a lot of assumptions that the nodes haven't been modified since initialization.
            Vector3 local_pos = transform.InverseTransformPoint(i_position);
            int column = Mathf.FloorToInt(local_pos.x / _node_width);
            int row = Mathf.FloorToInt(-local_pos.y / _node_height);
            if (row >= 0 && row < _nodes.GetLength(0)
                && column >= 0 && column < _nodes.GetLength(1))
            {
                return _nodes[row, column];
            }
            return null;
        }

        public GridNode GetRandGrid()
        {
            int row = Random.Range(0, _num_row);
            int column = Random.Range(0, _num_column);
            return _nodes[row, column];
        }

        public void SetDiagnoal(bool i_diagnoal)
        {
            _diagnoal = i_diagnoal;
        }
    }
}