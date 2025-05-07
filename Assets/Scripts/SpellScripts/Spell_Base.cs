using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell_Base : MonoBehaviour {
    
    [SerializeField] protected SpellAsset spellAsset;
    [SerializeField] protected Vector3 launchDirection;
    [SerializeField] protected Vector3 castLocation;
    protected LayerMask enemyLayerMask;
    protected const string ENEMY = "Enemy";

    protected void Awake() {
        enemyLayerMask = LayerMask.GetMask(ENEMY);
    }
    
    public virtual void Spell_Launch(Vector3 startLocation, Vector3 direction) {
        castLocation = startLocation;
        launchDirection = Vector3.Normalize(direction);
    }
    public virtual void Spell_Hit() {}
}