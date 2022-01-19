using Drone.Physics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Drone.HUD {
    public class HUDScript : MonoBehaviour {
        [SerializeField] DroneController playerDrone;
        [SerializeField] TextMeshProUGUI altitudeText;
        [SerializeField] TextMeshProUGUI velocityText;

        void Update() {
            altitudeText.text = string.Format("Altitude: {0:f2}", playerDrone.GetAltitude());
            velocityText.text = string.Format("Velocity: {0:f2}", playerDrone.GetVelocity());
        }
    }
}

