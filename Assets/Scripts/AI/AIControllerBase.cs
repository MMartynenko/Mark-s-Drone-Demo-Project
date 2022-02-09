using System;
using Drone.Control;
using UnityEngine;

namespace Drone.AI {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AIDroneInput))]
    public class AIControllerBase : MonoBehaviour {
        [SerializeField] protected Transform target = null;

        [SerializeField] protected float maxSpeed = 20f;
        [SerializeField] protected float minSpeed = 0f;
        [SerializeField] [Range(0, 1)] protected float maxSpeedFactor = 0.5f;        
        [SerializeField] protected float maxVerticalSpeed = 10f;         
        [SerializeField] protected float maxYawSensitivity = 0.01f; // how sensitively the AI uses steering input to turn to the desired direction
        [SerializeField] [Range(1, 90)] protected float angleThreshold = 90f; // threshold at which AI begins to slow down yaw
        protected float angleDeadZone = 1f; //threshold to which yaw course correction is performed
        protected float angleDistanceDeadZone = 0.1f; //how close are we to target to ignore course correction        
        [SerializeField] protected float cautiousMaxDistanceVertical = 5f; // distance at which distance-based cautiousness begins
        [SerializeField] protected float accelSensitivity = 0.5f; // How sensitively the AI uses the accelerator to reach the current desired speed
        [SerializeField] protected float brakeSensitivity = 1f; // How sensitively the AI uses the brake to reach the current desired speed
        [SerializeField] protected float horizontalWanderDistance = 0f; //How far the AI can randomly wander from target horizontally
        [SerializeField] protected float verticalWanderDistance = 0f; //How far the AI can randomly wander from target vertically
        protected float wanderSpeed = 0.1f; // How fast the wandering noise will fluctuate
        [SerializeField] protected float verticalEvasionDistance = 1f; //How far the drone will move vertically to avoid other drones

        protected Rigidbody rb;
        protected AIDroneInput controls;
        private float randomPerlin; // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)

        private float evasionDistance = 0;
        private float evasionTime = 0;

        private void Start() {
            rb = GetComponent<Rigidbody>();
            controls = GetComponent<AIDroneInput>();
            randomPerlin = UnityEngine.Random.value * 100;
        }

        private void Update() {
            HandleMission();
            if (!target) {
                return;
            }            
            HandleControls();
        }

        protected virtual void HandleMission() {            
        }

        private void HandleControls() {
            HandleYaw();
            HandlePitchAndRoll();
            HandleAltitude();
        }        

        private void HandleYaw() {
            // calculate the local-relative position of the target, to steer towards
            Vector3 localTarget = transform.InverseTransformPoint(target.position);

            // if too close to target, ignore yaw
            if (localTarget.magnitude < angleDistanceDeadZone) {
                SetYaw(0);
                return;
            }
            // work out the local angle towards the target
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            float angleCautiousFactor = Mathf.InverseLerp(angleThreshold, angleDeadZone, Mathf.Abs(targetAngle));
            float yawSensitivity = Mathf.Lerp(maxYawSensitivity, maxYawSensitivity * 0.1f, angleCautiousFactor);
            
            float steer = Mathf.Clamp(targetAngle * yawSensitivity, -1, 1);
            SetYaw(steer);            
        }

        private void HandlePitchAndRoll() {
            Vector3 localTarget = transform.InverseTransformPoint(target.position);
            Vector2 targetPos = new Vector2(localTarget.z, localTarget.x);
            Vector3 localVelocity = transform.InverseTransformVector(rb.velocity);
            Vector2 velocity2D = new Vector2(localVelocity.z, localVelocity.x);

            // Add horizontal wander
            Vector2 wanderVector = new Vector2(Mathf.PerlinNoise(Time.time * wanderSpeed, randomPerlin) * 2 - 1, Mathf.PerlinNoise(Time.time * wanderSpeed * 2, randomPerlin) * 2 - 1) * horizontalWanderDistance;
            targetPos += wanderVector;

            // Get desired speed based on distance to target. Clamp based on magnitude.
            Vector2 desiredSpeed = targetPos * maxSpeedFactor;            
            if (desiredSpeed.magnitude > maxSpeed) {
                desiredSpeed = desiredSpeed.normalized * maxSpeed;
            } else if (velocity2D.magnitude < minSpeed) {
                desiredSpeed = desiredSpeed.normalized * minSpeed;
            }

            // use different sensitivity depending on whether accelerating or braking: - do this separately for pitch and roll
            float accelBrakeSensitivityPitch = (Mathf.Sign(desiredSpeed.x) == Mathf.Sign(velocity2D.x) && desiredSpeed.x > velocity2D.x) ? accelSensitivity : brakeSensitivity;
            float accelBrakeSensitivityRoll = (Mathf.Sign(desiredSpeed.y) == Mathf.Sign(velocity2D.y) && desiredSpeed.y > velocity2D.y) ? accelSensitivity : brakeSensitivity;

            // decide the actual amount of accel input to achieve desired speed.             
            float accelPitch = Mathf.Clamp((desiredSpeed.x - velocity2D.x) * accelBrakeSensitivityPitch, -1, 1);
            float accelRoll = Mathf.Clamp((desiredSpeed.y - velocity2D.y) * accelBrakeSensitivityRoll, -1, 1);
            SetPitch(accelPitch);
            SetRoll(accelRoll);          
            
        }

        private void HandleAltitude() {
            float desiredSpeed = maxVerticalSpeed;

            // add vertical wander
            float targetPosition = target.position.y + (Mathf.PerlinNoise(Time.time * wanderSpeed, randomPerlin) * 2 - 1) * verticalWanderDistance;

            // Add evasion if necessary
            if (Time.time < evasionTime) {
                targetPosition += evasionDistance;
            } else {
                evasionDistance = 0;
            }

            // brake based on distance
            float delta = targetPosition - transform.position.y;            
            float distanceCautiousFactor = Mathf.InverseLerp(cautiousMaxDistanceVertical, 0, Mathf.Abs(delta));
            desiredSpeed = Mathf.Lerp(maxVerticalSpeed, 0, distanceCautiousFactor) * Mathf.Sign(delta);

            // use different sensitivity depending on whether accelerating or braking:
            float accelBrakeSensitivity = (Mathf.Sign(desiredSpeed) == Mathf.Sign(rb.velocity.y) && desiredSpeed > rb.velocity.y) ? accelSensitivity : brakeSensitivity;

            // decide the actual amount of accel input to achieve desired speed.
            float accel = Mathf.Clamp((desiredSpeed - rb.velocity.y) * accelBrakeSensitivity, -1, 1);            
            SetThrottle(accel);
        }        

        //Enable avoidance of other drones through collision detection
        private void OnTriggerStay(Collider col) {
            var rb = col.GetComponent<Rigidbody>();
            if (rb != null) {
                var otherDrone = rb.GetComponent<IDroneInput>();
                if (otherDrone != null) {
                    //Determine if the other drone is in front
                    if (Vector3.Angle(transform.forward, rb.transform.position - transform.position) < 90) {
                        //Evade for 1 second
                        evasionTime = Time.time + 1;                        
                        evasionDistance = verticalEvasionDistance * Mathf.Sign(transform.position.y - rb.transform.position.y);                        
                    }                        
                }
            }
        }

        protected void SetPitch (float value) {
            controls.Pitch = Mathf.Clamp(value, -1, 1);
        }

        protected void SetRoll(float value) {
            controls.Roll = Mathf.Clamp(value, -1, 1);
        }

        protected void SetYaw(float value) {
            controls.Yaw = Mathf.Clamp(value, -1, 1);
        }

        protected void SetThrottle(float value) {
            controls.Throttle = Mathf.Clamp(value, -1, 1);
        }
    }
}


