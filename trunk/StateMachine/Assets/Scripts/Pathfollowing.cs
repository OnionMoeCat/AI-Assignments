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
        private const float SPAWN_RANGE = 10f;
        private void Start() {
            // Create and center the grid
            grid.Create(numRow, numColumn);
            Vector2 gridSize = grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            grid.transform.position = gridPos;

            PathfollowingController controller = Instantiate<PathfollowingController>(pathfollowingController);
            Vector2 controllerPos = new Vector2(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), Random.Range(-SPAWN_RANGE, SPAWN_RANGE));
            controller.transform.position = controllerPos;
            controller.GetComponent<OrientedActor>().initialVelocity = Random.onUnitSphere * Random.Range(0.0f, controller.GetComponent<OrientedActor>().TheoryMaxSpeed);
            controller.transform.parent = transform;
            controller.gameObject.SetActive(true);
        }
    }
}