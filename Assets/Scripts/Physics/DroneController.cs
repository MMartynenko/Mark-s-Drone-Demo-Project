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
            SetYaw(transform.rotation.eulerAngles.y);            
        }

        protected override void HandlePhysics() {
            HandleEngines();           
            HandleControls();      
        }

        private void HandleControls() {
            rb.isKinematic = input.ControlDisabled;

            float inputPitch = input.ControlDisabled ? 0 : input.Pitch;
            float inputRoll = input.ControlDisabled ? 0 : input.Roll;
            float inputYaw = input.ControlDisabled ? 0 : input.Yaw;            

            float pitch = inputPitch * minMaxPitch; 
            float roll = -inputRoll * minMaxRoll;
            yaw += inputYaw * yawPower;            

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
        
        public void SetYaw(float eulerY) {
            yaw = eulerY;
            finalYaw = yaw;
        }

        public void Halt() {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;                    
        }        
    }
}


