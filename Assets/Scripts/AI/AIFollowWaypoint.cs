using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace Drone.AI {
    public class AIFollowWaypoint : AIControllerBase {
        [SerializeField] public Transform raceTarget = null;
        public bool RaceStarted { get; set; } = false;

        protected override void HandleMission() {
            if (RaceStarted) {
                target = raceTarget;
            } else {
                target = null;
            }
        }       

        public void ResetTracker() {
            GetComponent<WaypointProgressTracker>().Reset();
        }
    }
}


