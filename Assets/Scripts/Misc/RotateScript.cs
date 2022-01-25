using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Misc {

    public class RotateScript : MonoBehaviour {
        [SerializeField] float rotateSpeed = 2f;

        void Update() {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }
}


