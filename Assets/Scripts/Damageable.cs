using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour {

    public enum EntityType {
        Player,
        Enemy,
        Object
    }
    [SerializeField] protected EntityType entityType;
    [SerializeField] public float health;

    public abstract void TakeDamage(float damage);

    protected abstract void Death();
}