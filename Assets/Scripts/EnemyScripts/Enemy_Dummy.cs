using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dummy : Enemy_Base {

    [SerializeField] Transform anchor;

    protected void Start() {
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyRenderer = GetComponent<MeshRenderer>();
        enemyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        StartCoroutine(OnActive());
    }

    IEnumerator OnActive() {
        //Keep the dummy in the center
        while (true) {
            transform.position = Vector3.MoveTowards(transform.position, anchor.position, originalSpeed * Time.deltaTime);
            yield return null;
        }
    }

    protected override void Death() {
        health = 100;
    }
}