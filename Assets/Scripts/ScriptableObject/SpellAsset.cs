using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell Asset", menuName = "Spell Asset", order = 2)]
public class SpellAsset : ScriptableObject {

    public string spellName;
    public string symbolName;
    public enum castType {
        Projectile,
        Ray,
        Cone,
        Area
    };
    public castType type;
    public float damage;
    public float cost;
    public float castRange;

    [Header("Type Specific")]
    public float damageRange;
    public float knockbackStrength;
    public float areaDamage;
    public float tickDamage;
    public float coneAngle;
    public float freezeDuration;

    [Header("Asset References")]
    public GameObject projectile;

    [Header("Game Logic Variables")]
    public bool isActivated = false;
}