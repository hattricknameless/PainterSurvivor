using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterStateMachine : MonoBehaviour {

    private Coroutine currentCoroutine;
    private IEnumerator loadedSpell;
    private SpellCaster spellCaster;
    private DrawLine drawLine;
    public enum PainterState {
        Casting,
        Drawing,
        MovementLocked,
        DrawingLocked
    }
    public PainterState state = PainterState.Drawing;

    private void Start() {
        Time.timeScale = 1;
        spellCaster = FindObjectOfType<SpellCaster>();
        drawLine = FindObjectOfType<DrawLine>();
    }

    //This structure is inspired by chatGPT
    private void Update() {
        if (state == PainterState.DrawingLocked) {return;}
        if (state == PainterState.MovementLocked) {return;}
        if (Input.GetMouseButtonDown(0)) {
            StartCurrentCoroutine();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopCurrentCoroutine();
            if (state == PainterState.Drawing) {drawLine.EndDraw();}
            SwitchState();
        }
    }

    private void StartCurrentCoroutine() {
        if (currentCoroutine != null) {
            return;
        }
        if (state == PainterState.Drawing) {
            currentCoroutine = StartCoroutine(drawLine.DrawInput());
        }
        else if (state == PainterState.Casting) {
            currentCoroutine = StartCoroutine(spellCaster.currentCasting);
        }
    }
    private void StopCurrentCoroutine() {
        if (currentCoroutine != null) {StopCoroutine(currentCoroutine);}
        currentCoroutine = null;
    }
    private void SwitchState() {
        if (!drawLine.isAcceptable) {return;}
        if (state == PainterState.Casting) {
            state = PainterState.Drawing;
        }
        else if (state == PainterState.Drawing) {
            state = PainterState.Casting;
        }
    }
}