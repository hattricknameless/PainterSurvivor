using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_LightningBeam : Spell_Base {

    [SerializeField] private float beamWidth;
    // [SerializeField] private float beamLength;

    [SerializeField] private float animationTime;

    //Fields for beam animation
    private float timer;
    private Vector3 initialScale;

    private void Start() {
        initialScale = transform.localScale;
        StartCoroutine(BeamCollision());
        StartCoroutine(BeamAnimation());
    }

    private IEnumerator BeamCollision() {
        Vector3 colliderSize = new Vector3(beamWidth / 2, 0.5f, spellAsset.castRange / 2);
        Collider[] colliders = Physics.OverlapBox(transform.position, colliderSize, Quaternion.LookRotation(launchDirection, Vector3.up), enemyLayerMask);
        foreach (Collider other in colliders) {
            if (!other.CompareTag(ENEMY)) {continue;}
            Enemy_Base enemy = other.GetComponent<Enemy_Base>();
            enemy.TakeDamage(spellAsset.damage);
        }
        
        yield return new WaitForSeconds(animationTime * 2);
        Destroy(gameObject);
        yield break;
    }

    //This part of animation is provided by chatGPT
    private IEnumerator BeamAnimation()
    {
        //Extend the bar to max length
        timer = 0f;
        while (timer < animationTime) {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / animationTime);
            transform.localScale = new Vector3(initialScale.x, initialScale.y, progress * spellAsset.castRange);
            transform.localPosition = castLocation + launchDirection * (transform.localScale.z / 2f);
            yield return null;
        }

        //Shrink the bar from the starting end
        timer = 0f;
        Vector3 endPosition = castLocation + launchDirection * spellAsset.castRange;

        while (timer < animationTime) {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / animationTime);
            float remainingLength = 1f - progress;

            // Adjust scale and position
            transform.localScale = new Vector3(initialScale.x, initialScale.y, remainingLength * spellAsset.castRange);
            transform.localPosition = endPosition - launchDirection * (transform.localScale.z / 2f);
            yield return null;
        }
    }
}