using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_FireBall : Spell_Base {
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float explodeAnimTime;
    [SerializeField] private float explosionForce;
    private Rigidbody spellRigidbody;
    [SerializeField] private GameObject explodeFieldVisual;

    private void Start() {
        spellRigidbody = GetComponent<Rigidbody>();
        enemyLayerMask = LayerMask.GetMask("Enemy");
        explodeFieldVisual = gameObject.transform.GetChild(0).gameObject;
        explodeFieldVisual.SetActive(false);
        spellRigidbody.AddForce(launchDirection * projectileSpeed, ForceMode.Impulse);
    }

    private void Update() {
        if (Vector3.Distance(castLocation, transform.position) >= spellAsset.castRange) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag(ENEMY)) {return;}
        Enemy_Base enemy = collision.gameObject.GetComponent<Enemy_Base>();
        enemy.TakeDamage(spellAsset.damage);
        Destroy(spellRigidbody);
        StartCoroutine(Explosion());
    }

    private IEnumerator Explosion() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, spellAsset.damageRange, enemyLayerMask);
        foreach (Collider other in colliders) {
            if (!other.CompareTag(ENEMY)) {continue;}
            Enemy_Base enemy = other.GetComponent<Enemy_Base>();
            enemy.TakeDamage(spellAsset.areaDamage);
            enemy.ExplosiveKnockBack(explosionForce, transform.position, spellAsset.damageRange);
        }
        explodeFieldVisual.SetActive(true);
        explodeFieldVisual.transform.localScale = new Vector3(spellAsset.damageRange, spellAsset.damageRange, spellAsset.damageRange);
        yield return new WaitForSeconds(explodeAnimTime);
        Destroy(gameObject);
        yield break;
    }
}