using Drone.AI;
using Drone.Control;
using Drone.Physics;
using Drone.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Game {
    public class RaceManager : MissionManager {
        [SerializeField] GameObject playerDrone;
        [SerializeField] GameObject[] racingDrones;
        [SerializeField] Transform playerPosition;
        [SerializeField] Transform[] opponentPositions;
        [SerializeField] RacingEffects racingEffects;
        [SerializeField] GameObject[] waypointMarkers;
        [SerializeField] int laps = 3;

        private int currentWPMarker = -1;
        private int currentLap = 0;

        void Start() {
            DisableRacingDrones();
            foreach (GameObject marker in waypointMarkers) {
                marker.SetActive(false);
            }
        }

        protected override void OnEnable() {
            base.OnEnable();
            foreach (GameObject marker in waypointMarkers) {
                var wpscript = marker.GetComponent<WaypointMarker>();
                wpscript.onEnterWaypoint += AdvanceMarker;
            }            
        }

        protected override void OnDisable() {
            base.OnDisable();
            foreach (GameObject marker in waypointMarkers) {
                var wpscript = marker.GetComponent<WaypointMarker>();
                wpscript.onEnterWaypoint -= AdvanceMarker;
            }           
        }

        protected override void StartMission() {            
            StartCoroutine(RaceCountdown());
        }

        private IEnumerator RaceCountdown() {            
            //Disable player controls
            var controller = playerDrone.GetComponent<DroneController>();
            controller.Halt();
            controller.SetYaw(playerPosition.rotation.eulerAngles.y);
            playerDrone.GetComponent<PlayerDroneInput>().StopAndDisable();

            //Fade out
            yield return racingEffects.FadeOut();

            //Place drones in position
            playerDrone.transform.position = playerPosition.position;
            playerDrone.transform.rotation = playerPosition.rotation;

            for (int i = 0; i < racingDrones.Length; i++) {
                controller = racingDrones[i].GetComponent<DroneController>();
                controller.Halt();
                controller.SetYaw(opponentPositions[i].rotation.eulerAngles.y);
                racingDrones[i].GetComponent<AIDroneInput>().StopAndDisable();
                racingDrones[i].transform.position = opponentPositions[i].position;
                racingDrones[i].SetActive(true);
            }
            AdvanceMarker();

            //Fade in
            yield return racingEffects.FadeIn();

            //Countdown and return control
            yield return racingEffects.CountdownEffects();
            for (int i = 0; i < racingDrones.Length; i++) {
                racingDrones[i].GetComponent<AIFollowWaypoint>().RaceStarted = true;
                racingDrones[i].GetComponent<AIDroneInput>().ControlDisabled = false;
            }
            playerDrone.GetComponent<PlayerDroneInput>().ControlDisabled = false;
        }

        private void AdvanceMarker() {
            currentWPMarker++;
            if (currentWPMarker < waypointMarkers.Length) {
                waypointMarkers[currentWPMarker].SetActive(true);
            } else {
                currentLap++;
                if (currentLap < laps) {
                    currentWPMarker = 0;
                    waypointMarkers[currentWPMarker].SetActive(true);
                } else {
                    currentWPMarker = -1;
                    laps = 0;
                    DisableRacingDrones();
                    missionTriggerObject.SetActive(true);
                }       
            }
        }

        private void DisableRacingDrones() {
            foreach (GameObject drone in racingDrones) {
                drone.GetComponent<AIFollowWaypoint>().RaceStarted = false;
                drone.GetComponent<AIFollowWaypoint>().ResetTracker();
                drone.SetActive(false);
            }
        }
    }
}


