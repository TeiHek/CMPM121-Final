using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private bool dead;
    private Collider playerDetector;
    private Animator anim;
    private MonsterMovement movement;

    private void Awake() {
        currentHealth = maxHealth;
        dead = false;
        playerDetector = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        movement = GetComponent<MonsterMovement>();
    }

    public void damage(float dmgAmount) {
        // remove health from monster and kill if health is fully depleted
        currentHealth -= dmgAmount;
        if(currentHealth <= 0)
            die();
    }

    private void die() {
        // stop movement and play monster death anim
        dead = true;
        movement.stop();
        movement.enabled = false;
        anim.Play("MonsterDie");
    }

    private void OnTriggerEnter(Collider other) {
        // if not dead and colliding with player
        if(!dead && other.tag == "Player") {
            // stop movement and bring up lose screen
            movement.stop();
            movement.enabled = false;
            //TODO: call function in some game manager singleton to end game LOSE after a few seconds
        }
    }

    private void endGame() {
        // bring up win screen and destroy game object
        //TODO: call function in some game manager singleton to end game WIN after a few seconds
        Destroy(gameObject);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.O))
            damage(5);
    }
}
