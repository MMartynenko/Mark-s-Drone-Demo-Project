using Drone.Game;
using System;
using System.Linq;
using UnityEngine;

namespace Drone.AI {
    [System.Serializable]
    public struct Link {

        public enum direction { BI, UNI };
        public GameObject node1;
        public GameObject node2;
        public direction dir;
    }

    public class WarehouseManager : MonoBehaviour {

        [SerializeField] public GameObject[] waypoints;
        [SerializeField] public Link[] links;
        [SerializeField] public Graph graph = new Graph();

        [SerializeField] GameObject[] warehouseTriggers;
        public event Action<int> onChangeTarget;

        void Awake() {

            if (waypoints.Length > 0) {
                foreach (GameObject wp in waypoints) {

                    graph.AddNode(wp);
                    //Debug.Log(wp.name);
                }

                foreach (Link l in links) {

                    graph.AddEdge(l.node1, l.node2);
                    if (l.dir == Link.direction.BI) {

                        graph.AddEdge(l.node2, l.node1);
                    }
                }
            }
        }

        private void OnEnable() {            
            foreach (GameObject marker in warehouseTriggers) {
                var wpscript = marker.GetComponent<WarehouseMarker>();
                wpscript.onEnterMarker += ChangeTarget;
            }
        }        

        private void OnDisable() {            
            foreach (GameObject marker in warehouseTriggers) {
                var wpscript = marker.GetComponent<WarehouseMarker>();
                wpscript.onEnterMarker -= ChangeTarget;
            }
        }

        private void ChangeTarget(int index) {
            onChangeTarget?.Invoke(index);
        }

        void Update() {
            graph.debugDraw();
        }
    }
}

