using UnityEngine;

namespace Drone.Physics {

    [RequireComponent(typeof(Rigidbody))]
    public class RigidBodyManager : MonoBehaviour {
        [SerializeField] private float weight = 1f;

        protected Rigidbody rb;
        protected float startDrag;
        protected float startAngularDrag;

        void Awake() {
            rb = GetComponent<Rigidbody>();
            if (rb) {
                rb.mass = weight;
                startDrag = rb.drag;
                startAngularDrag = rb.angularDrag;
            }
        }

        
        void FixedUpdate() {
            if (!rb) {
                return;
            }
            HandlePhysics();
        }

        protected virtual void HandlePhysics() { }
    }
}


