using System;
using TMPro;
using UnityEngine;

public class PlayerStats : Damageable {

    [SerializeField] private TextMeshProUGUI endgameText;
    [SerializeField] private ReturnMainMenu returnButton;
    [SerializeField] private TextMeshProUGUI heathText;

    public override void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {Death();}
    }

    protected override void Death() {
        Time.timeScale = 0;
        endgameText.gameObject.SetActive(true);
        endgameText.text = "You're Dead";
        returnButton.gameObject.SetActive(true);
        Destroy(GetComponent<PainterMovement>());
    }

    private void Update() {
        heathText.text = $"Health: {health}";
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Enemy")) {
            TakeDamage(1);
            Enemy_Base enemy = other.GetComponent<Enemy_Base>();
            Vector3 knockbackDirection = collision.gameObject.transform.position - gameObject.transform.position;
            knockbackDirection *= 9;
            enemy.KnockBack(knockbackDirection, 7);
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Killzone")) {
            Death();
        }
    }
}