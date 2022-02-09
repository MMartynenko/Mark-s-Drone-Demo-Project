using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Control {
    public class AIDroneInput : MonoBehaviour, IDroneInput {
        private float _pitch;
        private float _roll;
        private float _yaw;
        private float _throttle;
        private bool _autoHover = true;
        private bool _controlDisabled = false;

        public float Pitch {
            get { return _pitch; }
            set { _pitch = value; }
        }
        public float Roll {
            get { return _roll; }
            set { _roll = value; }
        }
        public float Yaw {
            get { return _yaw; }
            set { _yaw = value; }
        }
        public float Throttle {
            get { return _throttle; }
            set { _throttle = value; }
        }
        public bool AutoHover => _autoHover;
        public bool ControlDisabled { get => _controlDisabled; set => _controlDisabled = value; }

        public void StopAndDisable() {
            _pitch = 0;
            _roll = 0;
            _yaw = 0;
            _throttle = 0;
            _controlDisabled = true;
        }
    }
}


