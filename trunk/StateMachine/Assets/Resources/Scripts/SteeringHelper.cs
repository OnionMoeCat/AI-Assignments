using UnityEngine;
namespace AISandbox {
	public class SteeringHelper {
		public static Vector2 GetSeekSteering(IActor actor, Vector2 target) {
			Vector2 axis = Vector2.zero;
			MonoBehaviour actor_mono = actor as MonoBehaviour;
			if (actor_mono != null) {
				Vector2 diff = target - (Vector2)actor_mono.transform.position;
				Vector2 desiredVelocity = diff.normalized * actor.MaxSpeed;
                axis = (desiredVelocity - actor.Velocity).normalized;
			}
			return axis;
		}

        public static Vector2 GetArriveSteering(IActor actor, Vector2 target, float slowing_distance)
        {
            Vector2 axis = Vector2.zero;
            MonoBehaviour actor_mono = actor as MonoBehaviour;
            if (actor_mono != null)
            {
                Vector2 diff = target - (Vector2)actor_mono.transform.position;
                float distance = diff.magnitude;
                float ramped_speed = actor.MaxSpeed*(distance/slowing_distance);
                float clipped_speed = Mathf.Min(ramped_speed, actor.MaxSpeed);
                Vector2 desiredVelocity = (clipped_speed/distance)*diff;
                axis = (desiredVelocity - actor.Velocity).normalized;
            }
            return axis;
        }
    }
}

