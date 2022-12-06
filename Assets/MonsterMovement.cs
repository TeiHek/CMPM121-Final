using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float viewDistance;
    private Camera viewCam;

    [Header("Chase")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float restartPatrolTime;
    [SerializeField] private float chaseSpeed;

    [Header("Patrol")] 
    [SerializeField] private float patrolSpeed;
    [SerializeField] private List<Vector3> points;
    private int nextPoint;
    private bool patrolling;
    private NavMeshAgent navAgent;

    private void Awake() {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = patrolSpeed;
        viewCam = GetComponent<Camera>();

        nextPoint = 0;
        patrolling = true;
    }

    private void Update() {
        // TODO: Make this also work if player fires their weapon
        if(inView()) {
            if(patrolling)
                patrolling = false;

            // start chasing player
            StopCoroutine("restartPatrol");
            navAgent.destination = player.position;
            navAgent.speed = chaseSpeed;
        }
        else {
            // basic patrolling
            if(patrolling) {
                if(!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
                    getNextPoint();
            }
            else {
                // get to last known player position. when position reached, wait, then restart patrol
                if(!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
                    StartCoroutine("restartPatrol");
            } 
        }
    }

    private void getNextPoint() {
        //print("getting next point " + nextPoint);
        navAgent.destination = points[nextPoint];
        nextPoint = (nextPoint + 1) % points.Count;
    }

    private bool inView() {
        // normalize view fustrum and see if player is within it
        Vector3 screenPoint = viewCam.WorldToViewportPoint(player.position);
        bool xCorrect = screenPoint.x > 0 && screenPoint.x < 1;
        bool yCorrect = screenPoint.y > 0 && screenPoint.y < 1;
        bool zCorrect = screenPoint.z > 0;

        // if player is within fustrum, raycast to see if they are not behind an object
        if(xCorrect && yCorrect && zCorrect) {
            Vector3 direction = (player.position - transform.position).normalized;
            bool hit = Physics.Raycast(transform.position, direction, viewDistance, playerLayer);
            Debug.DrawLine (transform.position, transform.position + direction * viewDistance, Color.red, 2);
            return hit;
        }

        return false;
    }

    private IEnumerator restartPatrol() {
        // wait then restart patrol where monster left off
        yield return new WaitForSeconds(restartPatrolTime);
        navAgent.destination = nextPoint - 1 >= 0 ? points[(nextPoint - 1) % points.Count] : points[points.Count - 1];
        navAgent.speed = patrolSpeed;
        patrolling = true;
    }

    /*
    *  stop() and restartMovement() may be unneeded
    *  stop maybe for when player is caughts
    */
    public void stop() {
        navAgent.isStopped = true;
        navAgent.speed = 0;
    }

    private void restartMovement() {
        navAgent.isStopped = false;
        navAgent.speed = patrolSpeed;
    }
}
