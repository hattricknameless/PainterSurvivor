using System;
using System.Collections;
using UnityEngine;

public class GroundDetector : MonoBehaviour {

    private Enemy_Base enemy;
    public bool isLanded = false;

    private void Start() {
        enemy = GetComponentInParent<Enemy_Base>();
    }
    private void OnTriggerEnter(Collider other) {
        //Starts the landed trigger if an enemy landed
        if (other.CompareTag("Floor")) {
            isLanded = true;
        }
        //Kill the enemy if an enemy falls out of the arena
        else if (other.CompareTag("Killzone")) {
            enemy.HitKillzone();
        }
    }
    private void Update() {
        
    }
}