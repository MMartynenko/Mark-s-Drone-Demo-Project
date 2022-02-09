using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Game {
    public class RaceTrigger : MonoBehaviour, IMissionTrigger {
        public event Action onMissionStarted;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {                
                onMissionStarted?.Invoke();
                gameObject.SetActive(false);
            }
        }

    }
}


