using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AISandbox {
    public class Grid : MonoBehaviour
    {
        public GridNode gridNodePrefab;

        private GridNode[ , ] _nodes;
        private float _node_width;
        private float _node_height;

        private int _num_row;
        private int _num_column;

        private bool _diagnoal;

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
        public Vector2 GetGridPositionFromViewport(Vector2 i_position)
        {
            Vector3 world_pos = Camera.main.ScreenToWorldPoint(i_position);
            Vector3 local_pos = transform.InverseTransformPoint(world_pos);
            return local_pos;
        }

        public GridNode GetGridFromViewport(Vector2 i_position)
        {
            Vector3 world_pos = Camera.main.ScreenToWorldPoint(i_position);
            return GetGridForPosition(world_pos);
        }
    }
}