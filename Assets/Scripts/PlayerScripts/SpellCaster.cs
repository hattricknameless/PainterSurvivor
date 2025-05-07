using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpellCaster : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerDisplayerTransform;
    [SerializeField] private GameObject castPoint;
    [SerializeField] private Camera playerCamera;
    public IEnumerator currentCasting; //Next Spell Coroutine to cast
    public SpellAsset currentSpell; //Next Spell Asset to cast
    private Dictionary<string, SpellAsset> spellDict = new(); //Load in all SpellAsset and sort with symbolName

    [Header("Mouse Finder")]
    private Plane displayerPlane;
    private float rotationVelocity;
    [SerializeField] private float rotationSmoothTime;
    [SerializeField] private Vector3 mousePosition;

    private void Start() {
        playerDisplayerTransform = FindObjectOfType<GamePlayScreen>().transform;
        displayerPlane = new Plane(playerDisplayerTransform.forward, playerDisplayerTransform.position);
        SpellLoader();
    }

    private void Update() {
        //This part of the code is inspired by chatGPT, to make Input.mousePosition stick on the ground plane
        gameObject.transform.position = player.transform.position;

        //Calculate mouse position in world space on the displayer
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (displayerPlane.Raycast(ray, out float distance)) {
            Vector3 quadHitPoint = ray.GetPoint(distance);

            //Convert quad hit point to UV coordinates (normalized local position)
            Vector3 localPoint = playerDisplayerTransform.InverseTransformPoint(quadHitPoint);
            Vector2 uv = new Vector2(localPoint.x + 0.5f, localPoint.y + 0.5f); // Assuming quad is centered at origin

            //Use UV to calculate world space in playerCamera
            Ray cameraBRay = playerCamera.ViewportPointToRay(new Vector3(uv.x, uv.y, 0));
            if (Physics.Raycast(cameraBRay, out RaycastHit worldHit)) {
                mousePosition = worldHit.point;
                Vector3 hitDirection = mousePosition - gameObject.transform.position;
            
                //This part of the code is borrowed from PainterMovement to rotate the SpellCaster
                //normRotation is the normalized vector2 on the xz plane
                Vector2 normRotation = new Vector2(hitDirection.x, hitDirection.z).normalized;
                //targetRotation is the rotation target in degrees
                float targetRotation = Mathf.Atan2(normRotation.x, normRotation.y) * Mathf.Rad2Deg;

                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
                //Apply rotation to the player
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }
    }
    private void SpellLoader() {
        Object[] spellResources = Resources.LoadAll("SpellAssets", typeof(SpellAsset));
        foreach (SpellAsset s in spellResources) {
            spellDict.Add(s.symbolName, s);
            Debug.Log($"{s.symbolName}/symbol, {s.spellName}/name added");
        }
    }

    //Find the type from given symbolName
    public void SpellCastRouter(string symbolName) {
        currentSpell = spellDict[symbolName];
        switch (currentSpell.type) {
        case SpellAsset.castType.Projectile:
            currentCasting = ProjectileSpell();
            break;
        case SpellAsset.castType.Ray:
            currentCasting = RaySpell();
            break;
        case SpellAsset.castType.Cone:
        currentCasting = ConeSpell();
            break;
        case SpellAsset.castType.Area:
            break;
        }
    }
    public IEnumerator ProjectileSpell() {
        while (true) {
            //waiting for the spell to be placed
            if (Input.GetMouseButtonDown(0)) {
                Vector3 launchDirection = mousePosition - castPoint.transform.position;
                launchDirection.y = 0;
                GameObject projectile = Instantiate(currentSpell.projectile, castPoint.transform.position, currentSpell.projectile.transform.rotation);
                projectile.GetComponent<Spell_Base>().Spell_Launch(castPoint.transform.position, launchDirection);
                yield break;
            }
            yield return null;
        }
    }

    public IEnumerator RaySpell() {
        while (true) {
            if (Input.GetMouseButtonDown(0)) {
                Vector3 launchDirection = mousePosition - castPoint.transform.position;
                launchDirection.y = 0;
                GameObject ray = Instantiate(currentSpell.projectile, castPoint.transform.position + launchDirection.normalized * currentSpell.castRange / 2, castPoint.transform.rotation);
                ray.GetComponent<Spell_Base>().Spell_Launch(castPoint.transform.position, launchDirection);
                yield break;
            }
            yield return null;
        }
    }

    public IEnumerator ConeSpell() {
        while (true) {
            if (Input.GetMouseButtonDown(0)) {
                Vector3 launchDirection = mousePosition - castPoint.transform.position;
                launchDirection.y = 0;
                GameObject cone = Instantiate(currentSpell.projectile, castPoint.transform.position, quaternion.identity);
                cone.GetComponent<Spell_Base>().Spell_Launch(castPoint.transform.position, launchDirection);
                yield break;
            }
            yield return null;
        }
    }
}