using System.Collections;
using System.Collections.Generic;
using Drone.Control;
using UnityEngine;

namespace Drone.Physics {

    [RequireComponent(typeof(BoxCollider))]
    public class DroneEngine : MonoBehaviour {        
        [SerializeField] private float maxThrottle = 4f;                
        [SerializeField] Animator fanAnimator;
        [SerializeField] private float maxFanRotSpeed = 2f;
        [SerializeField] private float minFanRotSpeed = 0.5f;

        public void UpdateEngine(Rigidbody rb, IDroneInput inputs) {
            Vector3 upVector = transform.up;
            upVector.x = 0;
            upVector.z = 0;
            float diff = 1 - upVector.magnitude;
            float finalDiff = diff * UnityEngine.Physics.gravity.magnitude;
            float inputThrottle = inputs.ControlDisabled ? 0 : inputs.Throttle;

            Vector3 engineForce = Vector3.zero;
            float autoHover = rb.mass * UnityEngine.Physics.gravity.magnitude + finalDiff;            
            if (!inputs.AutoHover) autoHover = 0;
            engineForce = transform.up * (autoHover + inputThrottle * maxThrottle) / 4;            

            rb.AddForce(engineForce, ForceMode.Force);
            HandlePropellers(inputThrottle);            
        }

        private void HandlePropellers(float throttle) {           
            if (!fanAnimator) {
                return;
            }

            float fanSpeed = 1f;
            if (throttle > 0) fanSpeed = maxFanRotSpeed;
            else if (throttle < 0) fanSpeed = minFanRotSpeed;
            fanAnimator.SetFloat("fanRotation", fanSpeed);
        }

       
    }
}


