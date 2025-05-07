using System.Collections;
using UnityEngine;

public class Enemy_Base : Damageable {

    protected PainterMovement painter;
    protected Rigidbody enemyRigidbody;
    protected MeshRenderer enemyRenderer;
    protected GroundDetector groundDetector;
    [SerializeField] protected readonly float summonHeight = 53;
    [SerializeField] protected float summonForce;
    [SerializeField] protected float originalSpeed;
    protected float currentMoveSpeed;
    [SerializeField] protected float groundDelay;

    protected enum EnemyState {
        freezed, 
    }

    [Header("VisualComponents")]
    [SerializeField] protected float flashDuration;
    [SerializeField] protected Material originalMaterial;
    [SerializeField] protected Material flashRedMaterial;
    [SerializeField] protected Material freezeMaterial;

    protected const string SPELL = "Spell";

    private void Start() {
        painter = FindObjectOfType<PainterMovement>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyRenderer = GetComponent<MeshRenderer>();
        groundDetector = GetComponentInChildren<GroundDetector>();

        //Inspired by chatGPT, freezes the rotation so it won't spin
        enemyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        currentMoveSpeed = originalSpeed;
        StartCoroutine(OnSummon());
    }

    IEnumerator OnSummon() {
        gameObject.transform.position += new Vector3(0, summonHeight, 0);
        enemyRigidbody.AddForce(new Vector3(0, -summonForce, 0), ForceMode.Acceleration);
        while (!groundDetector.isLanded) {
            yield return null;
        }
        yield return new WaitForSeconds(groundDelay);
        StartCoroutine(OnActive());
        yield break;
    }

    IEnumerator OnActive() {
        while (true) {
            try {
                transform.position = Vector3.MoveTowards(transform.position, painter.transform.position, currentMoveSpeed * Time.deltaTime);
            }
            catch {
                //if cannot find player, stays at the same spot
                transform.position = transform.position;
            }
            yield return null;
        }
    }

    public override void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            Death();
        }
        else {
            StartCoroutine(OnTakeDamageVisual());
        }
    }

    public void Freezed(float damage, float duration) {
        health -= damage;
        if (health <= 0) {
            Death();
        }
        else {
            StartCoroutine(OnFreezeVisual(duration));
        }
    }

    protected override void Death() {
        // Debug.Log("Unit dead");
        Destroy(gameObject);
    }

    public void HitKillzone() {
        Death();
    }

    public void KnockBack(Vector3 direction, float strength) {
        direction.y = 0;
        enemyRigidbody.AddForce(direction + new Vector3(0, strength, 0), ForceMode.Impulse);
    }

    public void ExplosiveKnockBack(float force, Vector3 origin, float radius) {
        enemyRigidbody.AddExplosionForce(force, origin, radius);
    }
    
    //ChatGPT inspired me to change material
    protected IEnumerator OnTakeDamageVisual() {
        enemyRenderer.material = flashRedMaterial;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.material = originalMaterial;
    }

    protected IEnumerator OnFreezeVisual(float duration) {
        enemyRenderer.material = freezeMaterial;
        currentMoveSpeed = 0;
        yield return new WaitForSeconds(duration);
        currentMoveSpeed = originalSpeed;
        enemyRenderer.material = originalMaterial;
    }
}
