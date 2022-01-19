using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drone.Control;
using Cinemachine;

namespace Drone.Core {
    public class CameraControl : MonoBehaviour {
        [SerializeField] Transform player;        
        [SerializeField] PlayerDroneInput playerInput;
        [SerializeField] CinemachineVirtualCamera mainCamera;
        [SerializeField] CinemachineVirtualCamera backCamera;
        [SerializeField] CinemachineVirtualCamera rightCamera;
        [SerializeField] CinemachineVirtualCamera leftCamera;
        [SerializeField] CinemachineVirtualCamera bottomCamera;

        private void Awake() {
            transform.position = player.position;
            transform.rotation = player.rotation;
        }

        private void Update() { 
            if (!playerInput.Camera.Equals(Vector2.zero)) {
                mainCamera.enabled = false;
                if (playerInput.Camera.y < 0) {
                    backCamera.enabled = true;
                } else if (playerInput.Camera.y > 0) {
                    bottomCamera.enabled = true;
                } else if (playerInput.Camera.x > 0) {
                    leftCamera.enabled = true;
                } else if (playerInput.Camera.x < 0) {
                    rightCamera.enabled = true;
                }
            } else {
                mainCamera.enabled = true;
                backCamera.enabled = false;
                rightCamera.enabled = false;
                leftCamera.enabled = false;
                bottomCamera.enabled = false;
            }
        }

        private void LateUpdate() {
            float playerY = player.eulerAngles.y;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerY, transform.eulerAngles.z);
        }
    }
}


    
    
