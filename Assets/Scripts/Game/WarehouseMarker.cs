using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Game {
    public class WarehouseMarker : MonoBehaviour {
        [SerializeField] int markerIndex = 0;
        public event Action<int> onEnterMarker;
        private float hideTime = 5f;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                onEnterMarker?.Invoke(markerIndex);
                StartCoroutine(HideMarker());
            }
        }

        private IEnumerator HideMarker() {
            var renderer = gameObject.GetComponent<MeshRenderer>();
            var collider = gameObject.GetComponent<BoxCollider>();
            renderer.enabled = false;
            collider.enabled = false;
            yield return new WaitForSeconds(hideTime);
            renderer.enabled = true;
            collider.enabled = true;
        }
    }
}

