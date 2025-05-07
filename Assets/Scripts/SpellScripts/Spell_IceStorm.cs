using System.Collections;
using UnityEngine;

public class Spell_IceStorm : Spell_Base {

    private ParticleSystem iceParticle;
    [SerializeField] private float animationTime;
    private void Start() {
        iceParticle = GetComponent<ParticleSystem>();
        FreezeParticleAdjustments();
        StartCoroutine(FreezeAnimation());
    }

    //Adjustments are from chatGPT
    private void FreezeParticleAdjustments() {
        //Adjust the particle direction align with launchDirection
        iceParticle.transform.rotation = Quaternion.LookRotation(launchDirection, Vector3.up);
        var particleShape = iceParticle.shape;
        particleShape.rotation = new Vector3(90, -spellAsset.coneAngle / 2, 0);
        particleShape.arc = spellAsset.coneAngle;

        //Adjust the particle reach and animation time
        var particleMain = iceParticle.main;
        particleMain.startSpeed = spellAsset.castRange / animationTime;
        particleMain.startLifetime = animationTime;
        particleMain.loop = false;
    }

    private IEnumerator FreezeAnimation() {
        iceParticle.Play();
        Collider[] colliders = Physics.OverlapSphere(castLocation, spellAsset.castRange, enemyLayerMask);

        foreach (Collider other in colliders) {
            if (!other.CompareTag(ENEMY)) {continue;}
            
            Vector3 directionToTarget = (other.transform.position - castLocation).normalized;

            // Calculate the angle between the sector direction and the target
            float angleToTarget = Vector3.Angle(launchDirection, directionToTarget);

            if (angleToTarget <= spellAsset.coneAngle / 2f) {
                Enemy_Base enemy = other.GetComponent<Enemy_Base>();
                enemy.Freezed(spellAsset.damage, spellAsset.freezeDuration);
            }
        }
        yield return new WaitForSeconds(animationTime + 0.1f);
        Destroy(gameObject);
    }

    //The not so successful particle collision
    private void OnParticleCollision(GameObject other) {
        if (!other.CompareTag(ENEMY)) {return;}
        Enemy_Base enemy = other.GetComponent<Enemy_Base>();
        enemy.Freezed(spellAsset.damage, spellAsset.freezeDuration);
    }
}
