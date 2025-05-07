using UnityEngine;
using UnityEngine.InputSystem;

public class PainterInputHandler : MonoBehaviour {

    //Structure copied from Unity Starter Assets
    [Header("Input Values")]
    public Vector2 moveDirection;

    public void OnMove(InputValue inputValue) {
        MoveInput(inputValue.Get<Vector2>());
    }

    private void MoveInput(Vector2 newMoveDirection) {
        moveDirection = newMoveDirection;
    }
}