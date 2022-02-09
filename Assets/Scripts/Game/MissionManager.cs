using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Game {
    public class MissionManager : MonoBehaviour {
        [SerializeField] protected GameObject missionTriggerObject = null;
        protected IMissionTrigger missionTrigger = null;

        private void Awake() {
            missionTrigger = missionTriggerObject.GetComponent<IMissionTrigger>();
        }

        protected virtual void OnEnable() {            
            missionTrigger.onMissionStarted += StartMission;
        }

        protected virtual void OnDisable() {
            missionTrigger.onMissionStarted -= StartMission;
        }

        protected virtual void StartMission() {            
        }        
    }
}


