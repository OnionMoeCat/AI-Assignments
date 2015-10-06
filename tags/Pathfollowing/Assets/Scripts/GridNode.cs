using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace AISandbox {
    public class GridNode : MonoBehaviour
    {

        private Color start_color = Color.red;
        private Color end_color = Color.yellow;
        private Color path_color = Color.magenta;

        public Grid grid;
        public int column;
        public int row;

        private SpriteRenderer sprite_renderer;

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
        private  bool start;
        public bool Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        [SerializeField]
        private bool end;
        public bool End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        [SerializeField]
        private bool path;
        public bool Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        private void Awake()
        {
            sprite_renderer = GetComponent<SpriteRenderer>();
            sprite_renderer.color = TerrainTypeManager.GetColor(terrainType);
        }

        void Update()
        {
            if (start)
            {
                sprite_renderer.color = start_color;
            }
            else if (end)
            {
                sprite_renderer.color = end_color;
            }
            else if (path)
            {
                sprite_renderer.color = path_color;
            }
            else
            {
                sprite_renderer.color = TerrainTypeManager.GetColor(terrainType);
            }
        }

        public IList<GridNode> GetNeighbors( bool include_diagonal = false ) {
            return grid.GetNodeNeighbors( row, column, include_diagonal );
        }
    }
}