using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AISandbox {
    public class Game : MonoBehaviour {
        public Grid grid;
        public int numRow = 30;
        public int numColumn = 30;
        public PathfollowingController pathfollowingController;
        public GameObject ui;
        private const float SPAWN_POSITION_RANGE = 10f;
        private const float SPAWN_VELOCITY_RANGE = 0.1f;

        private PathfollowingController m_controller;
        private InputManager m_inputManager;
        private BT_BlackBoard m_blackboard;
        private BT_BehaviourTree m_tree;
        private bool m_Ticking = false;

        public PathfollowingController Actor
        {
            get { return m_controller; }
        }

        private void Start() {
            // Create and center the grid
            grid.Create(numRow, numColumn);
            Vector2 gridSize = grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            grid.transform.position = gridPos;

            m_controller = Instantiate<PathfollowingController>(pathfollowingController);
            m_inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
            BuildBehaviourTree();      
        }

        private void Update()
        {
            if (m_Ticking)
            {
                m_tree.Tick(m_controller, m_blackboard);
            }
        }

        public void Launch() {
            m_controller.gameObject.SetActive(true);
            Vector2 controllerPos = new Vector2(Random.Range(-SPAWN_POSITION_RANGE, SPAWN_POSITION_RANGE), Random.Range(-SPAWN_VELOCITY_RANGE, SPAWN_VELOCITY_RANGE));
            m_controller.transform.position = controllerPos;
            m_controller.GetComponent<OrientedActor>().initialVelocity = Random.onUnitSphere * Random.Range(0.0f, m_controller.GetComponent<OrientedActor>().TheoryMaxSpeed);
            m_controller.transform.parent = transform;
            m_Ticking = true;
        }

        public void Reset()
        {
            m_controller.GetComponent<OrientedActor>().Reset();
            m_controller.gameObject.SetActive(false);
            EntityManager.Reset();
            m_inputManager.EnableEdit = true;
            ui.SetActive(true);
            m_Ticking = false;
        }

        public void BuildBehaviourTree()
        {
            m_blackboard = new BT_BlackBoard();
            m_tree = new BT_BehaviourTree();

        }

        
    }
}