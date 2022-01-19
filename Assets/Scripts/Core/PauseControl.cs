using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Core {
    public class PauseControl : MonoBehaviour {

        public static bool gameIsPaused = false;

        public void PauseGame() {
            Time.timeScale = 0f;
            gameIsPaused = true;
        }

        public void UnpauseGame() {
            Time.timeScale = 1f;
            gameIsPaused = false;
        }

        public void TogglePause() { 
            if(!gameIsPaused) {
                PauseGame();
            } else {
                UnpauseGame();
            }
        }

        private void OnPause() {            
            TogglePause();
        }
    }
}


