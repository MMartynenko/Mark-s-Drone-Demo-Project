using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Drone.Control;

namespace Drone.Physics {
    public class DroneController : RigidBodyManager {
        [SerializeField] private float minMaxPitch = 30;
        [SerializeField] private float minMaxRoll = 30;
        [SerializeField] private float yawPower = 4;
        [SerializeField] private float lerpSpeed = 2;
        
        private IDroneInput input;
        private List<DroneEngine> engines = new List<DroneEngine>();

        private float finalPitch;
        private float finalRoll;
        private float finalYaw;
        private float yaw;
        

        void Start() {
            input = GetComponent<IDroneInput>();
            engines = GetComponentsInChildren<DroneEngine>().ToList();            
        }

        protected override void HandlePhysics() {
            HandleEngines();           
            HandleControls();      
        }

        private void HandleControls() {
            float pitch = input.Pitch * minMaxPitch; 
            float roll = -input.Roll * minMaxRoll;
            yaw += input.Yaw * yawPower;            

            finalPitch = Mathf.Lerp(finalPitch, pitch, lerpSpeed * Time.deltaTime);
            finalRoll = Mathf.Lerp(finalRoll, roll, lerpSpeed * Time.deltaTime);
            finalYaw = Mathf.Lerp(finalYaw, yaw, lerpSpeed * Time.deltaTime);

            Quaternion rot = Quaternion.Euler(finalPitch, finalYaw, finalRoll);
            rb.MoveRotation(rot);
        }

        private void HandleEngines() {
            foreach (DroneEngine engine in engines) {
                engine.UpdateEngine(rb, input);
            }
        }

        public float GetAltitude() {
            return transform.position.y;
        }

        public float GetVelocity() {
            return rb.velocity.magnitude;
        }        
    }
}


