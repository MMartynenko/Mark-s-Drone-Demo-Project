using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.AI {
    public class AIPathfindWaypoint : AIControllerBase {
        [SerializeField] GameObject wpManager;
        private float accuracy = 1.0f;
        private GameObject[] wps;
        private GameObject currentNode;
        private int currentWP = 0;
        private Graph g;
        private float savedMinSpeed;

        protected override void Start() {
            base.Start();
            wps = wpManager.GetComponent<WarehouseManager>().waypoints;
            g = wpManager.GetComponent<WarehouseManager>().graph;
            currentNode = wps[0];            
            savedMinSpeed = minSpeed;
        }

        private void OnEnable() {
            wpManager.GetComponent<WarehouseManager>().onChangeTarget += PathfindToWaypoint;
        }        

        private void OnDisable() {
            wpManager.GetComponent<WarehouseManager>().onChangeTarget -= PathfindToWaypoint;
        }

        private void PathfindToWaypoint(int index) {
            g.AStar(currentNode, wps[index]);
            currentWP = 0;
        }

        protected override void HandleMission() {
            if (g.getPathLength() == 0 || currentWP == g.getPathLength()) {
                minSpeed = 0;
                return;
            }

            minSpeed = savedMinSpeed;

            //the node we are closest to at this moment
            currentNode = g.getPathPoint(currentWP);

            //if we are close enough to the current waypoint move to next
            if (Vector3.Distance(
                g.getPathPoint(currentWP).transform.position,
                transform.position) < accuracy) {
                currentWP++;
            }

            //if we are not at the end of the path
            if (currentWP < g.getPathLength()) {
                target = g.getPathPoint(currentWP).transform;                
            }
        }
    }
}
    
