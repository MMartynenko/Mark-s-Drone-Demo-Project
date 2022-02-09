using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Game {
    public class WaypointMarker : MonoBehaviour {
        public event Action onEnterWaypoint;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                onEnterWaypoint?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}

