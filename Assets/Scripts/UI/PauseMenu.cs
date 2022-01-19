using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drone.Core;
using System;

namespace Drone.UI {
    public class PauseMenu : MonoBehaviour {

        PauseControl pauseControl;
        Canvas pauseMenuCanvas;

        bool showPauseMenu = false;

        void Awake() {
            pauseControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseControl>();
            pauseMenuCanvas = GetComponent<Canvas>();
            pauseMenuCanvas.enabled = false;
        }
              
        void Update() {
            if (showPauseMenu != PauseControl.gameIsPaused) {
                ShowOrHideCanvas(PauseControl.gameIsPaused);
                showPauseMenu = PauseControl.gameIsPaused;
            }                          
        }

        private void ShowOrHideCanvas(bool toggle) {
            pauseMenuCanvas.enabled = toggle;
        }

        public void OnClickResume() {
            pauseControl.UnpauseGame();
        }

        public void OnClickExit() {
            Application.Quit();
        }
    }
}


