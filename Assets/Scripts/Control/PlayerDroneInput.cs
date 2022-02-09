using UnityEngine;
using UnityEngine.InputSystem;

namespace Drone.Control {

    [RequireComponent(typeof(PlayerInput))]
    public class PlayerDroneInput : MonoBehaviour, IDroneInput {
        private float _pitch;
        private float _roll;
        private float _yaw;
        private float _throttle;
        private bool _autoHover = true;
        private Vector2 _camera;
        private bool _controlDisabled = false;

        public float Pitch => _pitch;
        public float Roll => _roll;
        public float Yaw => _yaw;
        public float Throttle => _throttle;
        public bool AutoHover => _autoHover;
        public Vector2 Camera => _camera;
        public bool ControlDisabled { get => _controlDisabled; set => _controlDisabled = value; }

        private void OnPitch(InputValue value) {
            _pitch = value.Get<float>();
        }

        private void OnRoll(InputValue value) {
            _roll = value.Get<float>();
        }

        private void OnYaw(InputValue value) {
            _yaw = value.Get<float>();
        }

        private void OnThrottle(InputValue value) {
            _throttle = value.Get<float>();
        }

        private void OnAutoHover(InputValue value) {
            //Remove entirely?
            //if (!PauseControl.gameIsPaused) {
            //    _autoHover = !_autoHover;
            //}            
        } 
        
        private void OnCamera(InputValue value) {
            _camera = value.Get<Vector2>();
        }

        public void StopAndDisable() {
            _pitch = 0;
            _roll = 0;
            _yaw = 0;
            _throttle = 0;
            _controlDisabled = true;
        }
    }
}


