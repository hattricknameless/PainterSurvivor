using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    private PainterStateMachine painterState;
    private SpellCaster spellCaster;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private GameObject exitPortal;
    [SerializeField] private List<string> dialogueInput;
    private int dialoguePointer = 0;

    private void Start() {
        spellCaster = FindObjectOfType<SpellCaster>();
        painterState = FindObjectOfType<PainterStateMachine>();
        painterState.state = PainterStateMachine.PainterState.MovementLocked;
        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial() {
        //Welcome to PainterSurvivor
        tutorialText.text = dialogueInput[dialoguePointer++]; //dialogue0

        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        //This is a game that you draw symbols and cast spells!!
        tutorialText.text = dialogueInput[dialoguePointer++]; //dialogue1
        yield return null;

        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        //WASD to move
        tutorialText.text = dialogueInput[dialoguePointer++]; //dialogue2
        painterState.state = PainterStateMachine.PainterState.DrawingLocked;
        yield return null;

        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        //Now let's try out our first spell, FireBall
        tutorialText.text = dialogueInput[dialoguePointer++]; //dialogue3
        yield return null;

        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        //Draw a circle, Then point to a direction to cast it
        tutorialText.text = dialogueInput[dialoguePointer++]; //dialogue4
        painterState.state = PainterStateMachine.PainterState.Drawing;
        yield return null;

        //Detect a FireBall has drawn
        while (true) {
            try {
                if (spellCaster.currentSpell.spellName == "FireBall") {
                    break;
                }
            }
            catch {}
            yield return null;
        }
        yield return null;
        for (int i = 6; i < 9; i++) {
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            //Well done!!
            tutorialText.text = dialogueInput[dialoguePointer++]; //dialogue6~8
            yield return null;
        }
        exitPortal.SetActive(true);
        Debug.Log("Tutorial End");
        yield break;
    }
}
