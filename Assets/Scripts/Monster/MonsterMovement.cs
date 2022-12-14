using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MonsterMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float viewDistance;
    [SerializeField] private Volume ppfxVolume;
    [SerializeField, Range(0,1)] private float vignetteDecay;
    [SerializeField] AudioClip alertSound;
    private AudioSource audioSource;
    private Vignette playerVignette;
    private Camera viewCam;
    private bool playerInView;
    private bool alerted;

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
        ppfxVolume.profile.TryGet<Vignette>(out playerVignette);
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = patrolSpeed;
        viewCam = GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();

        nextPoint = 0;
        patrolling = true;
        playerInView = false;
        alerted = false;
    }

    private void Update() {
        // TODO: Make this also work if player fires their weapon
        if(inView() || GameManager.Instance.GetJustShot()) {
            if(patrolling)
                patrolling = false;
            GameManager.Instance.SetJustShot(false);

            if (!alerted)
            {
                alerted = true;
                audioSource.PlayOneShot(alertSound);
            }
            // start chasing player
            StopCoroutine("restartPatrol");
            navAgent.destination = player.position;
            navAgent.speed = chaseSpeed;
            if (playerInView)
            {
                setVignetteIntensity();
            }
        }
        else {
            // basic patrolling
            if(patrolling) {
                playerVignette.intensity.value = Mathf.Clamp(playerVignette.intensity.value - vignetteDecay * Time.deltaTime, 0f, 1f);
                if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
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
        if(points.Count == 0) return;
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
            playerInView = hit;
            return hit;
        }
        playerInView = false;
        return false;
    }

    public bool heardShot(bool state) {
        return state;
    }

    private IEnumerator restartPatrol() {
        // wait then restart patrol where monster left off
        yield return new WaitForSeconds(restartPatrolTime);
        alerted = false;
        navAgent.destination = nextPoint - 1 >= 0 ? points[(nextPoint - 1) % points.Count] : points[points.Count - 1];
        navAgent.speed = patrolSpeed;
        patrolling = true;
    }

    public void stop() {
        navAgent.isStopped = true;
        navAgent.speed = 0;
    }

    // may be unecessary
    private void restartMovement() {
        navAgent.isStopped = false;
        navAgent.speed = patrolSpeed;
    }

    private void setVignetteIntensity() {
        float distance = Vector3.Distance(transform.position, player.position);
        float intensity = (-(1 / viewDistance) * distance) + 1;
        playerVignette.intensity.value = Mathf.Clamp(intensity, 0, 1);
    }
}
