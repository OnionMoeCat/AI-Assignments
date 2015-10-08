using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AISandbox {
    public class Pathfollowing : MonoBehaviour {
        public Grid grid;
        public int numRow = 30;
        public int numColumn = 30;
        public PathfollowingController pathfollowingController;
        public GameObject ui;
        private const float SPAWN_RANGE = 10f;
        private List<PathfollowingController> m_actors = new List<PathfollowingController>();

        private PathfollowingController m_controller;

        public List<PathfollowingController> Actors
        {
            get { return m_actors; }
        }

        private void Start() {
            // Create and center the grid
            grid.Create(numRow, numColumn);
            Vector2 gridSize = grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            grid.transform.position = gridPos;

            m_controller = Instantiate<PathfollowingController>(pathfollowingController);            
        }

        public void Launch() {
            m_controller.gameObject.SetActive(true);
            Vector2 controllerPos = new Vector2(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), Random.Range(-SPAWN_RANGE, SPAWN_RANGE));
            m_controller.transform.position = controllerPos;
            m_controller.GetComponent<OrientedActor>().initialVelocity = Random.onUnitSphere * Random.Range(0.0f, m_controller.GetComponent<OrientedActor>().TheoryMaxSpeed);
            m_controller.transform.parent = transform;            
            m_actors.Add(m_controller);
        }

        public void Reset()
        {
            m_actors.Remove(m_controller);
            m_controller.CleanUpInventory();
            m_controller.gameObject.SetActive(false);
            EntityManager.Reset();
            ui.SetActive(true);
        }
    }
}