using UnityEditor;
using UnityEngine;

public class PainterMovement : MonoBehaviour {

    //References
    private PainterInputHandler inputHandler; //The script handles input
    private CharacterController controller; //Charater controller component
    private PainterStateMachine stateMachine;

    [SerializeField] private float moveSpeed = 10.0f; //MoveSpeed of the player
    private float rotationVelocity; //Auto-checked by unity using in smoothing
    [SerializeField] private float rotationSmoothTime; //How long it takes to reach the target angle
    [SerializeField] private float speedChangeRate = 10.0f; //How fast to reach the full movespeed
    [SerializeField] private float currentSpeed; //The current speed of the player
    private float targetRotation; //The target rotation the player character wants to reach
    [SerializeField] private float gravity;
    [SerializeField] private float terminalVelocity;
    [SerializeField] private float gravityVelocity;
    private bool isGrounded; //If player is on the ground

    private void Start() {
        //References of the components
        inputHandler = GetComponent<PainterInputHandler>();
        controller = GetComponent<CharacterController>();
        stateMachine = FindObjectOfType<PainterStateMachine>();
        isGrounded = controller.isGrounded;
    }

    private void Update() {
        if (stateMachine.state == PainterStateMachine.PainterState.MovementLocked) {return;}
        Move();
    }
    /*
    The Move() function is refactored from ThirdPersonController's Move() function and removed most of the
    unnecessary part I don't need, I only need the move on ground part.
    I used chatGPT to figue out what I do or don't need.
    */
    private void Move() {
        float targetSpeed = moveSpeed;
        //When the there is no input, set targetSpeed to 0, gradually slowing down
        if (inputHandler.moveDirection == Vector2.zero) {
            targetSpeed = 0.0f;
        }

        //Inputspeed detect how fast player is currently moving
        float inputSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
        float speedOffset = 0.1f;
        //If inputSpeed is slower or faster than the targetSpeed (with an offset), increase or decrease speed
        if (inputSpeed < targetSpeed - speedOffset || inputSpeed > targetSpeed + speedOffset) {
            //Lerp makes moves smoothly from currentSpeed to targetSpeed, use Time.deltatime*speedChangeRate as the argument
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
            //Round the speed incase of floating point error
            currentSpeed = Mathf.Round(currentSpeed * 1000f) / 1000f;
        }
        else {
            //If currentSpeed is within the offset, set it to targetSpeed
            currentSpeed = targetSpeed;
        }
        
        //normRotation is the normalized vector of the moveDirection from input
        Vector2 normRotation = new Vector2(inputHandler.moveDirection.x, inputHandler.moveDirection.y).normalized;
        
        //If the moveDirection is not zero(player inputs moving)
        if (inputHandler.moveDirection != Vector2.zero) {
            //targetRotation is the target angle in degrees (Atan2() turns into radians and Rad2Deg turns into degrees)
            targetRotation = Mathf.Atan2(normRotation.x, normRotation.y) * Mathf.Rad2Deg;
            //rotation is the next rotation player will do turn from transform.y to targetRotation, rotationSmoothTime as the argument
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                rotationSmoothTime);
            //Apply rotation to the player
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        
        //targetDirection is the player moving towards the moveDirection
        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        //Horizontalmovement is currentSpeed*Time.deltaTime with direction of targetDirection
        Vector3 horizontalMovement = currentSpeed * Time.deltaTime * targetDirection.normalized;

        if (isGrounded && gravityVelocity < 0) {
            gravityVelocity = -2;
        }
        else {
            gravityVelocity = gravity * Time.deltaTime;
            gravityVelocity = Mathf.Min(gravityVelocity, terminalVelocity);
        }
        //VerticalMovement applies gravity
        Vector3 verticalMovement = new Vector3(0, gravityVelocity, 0);

        //Player makes the move to horizontal + vertical movement
        controller.Move(horizontalMovement + verticalMovement);
    }
}