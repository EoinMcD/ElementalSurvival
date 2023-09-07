using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region  Variables
    [Header("Movement")]
    [SerializeField] float walkSpeed=7f;    
    [SerializeField] float sprintSpeed=14f;
    [SerializeField] float groundDrag=8f;
    [SerializeField] float slideSpeed=14f;
    float moveSpeed;                                //Speed that player is moving . Is dynamically set to different speeds
    
    [Space]
    [SerializeField] public Transform orientation;  //Direction the player is facing : orientation.forward

    [Header("Jumping")]
    [SerializeField] float jumpForce=14f;
    [SerializeField] float jumpCooldown=0.3f;
    [SerializeField] float airMultiplier=0.4f;
    [SerializeField] float airDrag=.66f;
    [SerializeField] int maxJumps=1;
    const float playerGravity =3.5f;
    float gravity;
    bool useGravity;
    float numJumps;
    bool readyToJump;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed=3.5f;
    [SerializeField] float crouchYScale=.5f;
    float startYScale;

    [Header("Ground Check")]
    [SerializeField] float playerHeight=2;
    bool grounded;
    
    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle=40f;
    RaycastHit slopeHit;
    bool exitingSlope;
 
    [Header("Ability Use")]
    bool isAbility;
    float abilitySpeed;

    [Header("References")]    
    MovementState moveState;
    PlayerSliding ps;
    PlayerStats pStats;
    Rigidbody rb;

    public bool sliding;
    bool canInput = true;
    float horizontalInput;
    float verticalInput;
    public Vector3 moveDir;
    public bool applyDrag = true;
    bool minimiseVelocity=true;

    public enum MovementState {
        ability,
        walking,
        sprinting,
        air,
        sliding,
        crouching
    }
    #endregion

    #region  InBuiltFunctions
    private void Start() {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<PlayerSliding>();
        pStats = GetComponent<PlayerStats>();
        rb.useGravity=false;
        rb.freezeRotation = true;
        gravity=playerGravity;
        useGravity=true;

        
        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        StateHandler();
        IsGrounded();
        if(useGravity){
            DoGravity();
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }
    #endregion

    #region  Input and Movement
    void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKeyDown(sprintKey)) {
            pStats.StartSprinting();
        }
        if(Input.GetKeyUp(sprintKey)) {
            pStats.StopSprinting();
        }
        if(((Input.GetKey(jumpKey)&&grounded) || (Input.GetKeyDown(jumpKey) && moveState==MovementState.air)) && numJumps>=1 && readyToJump) {
            
            Jump();

            
        }

        if(Input.GetKeyDown(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if(Input.GetKeyUp(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            
        }
    }

    void MovePlayer() {
        if(!canInput) {return;}
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(OnSlope() && !exitingSlope) {
            rb.AddForce(GetSlopeMoveDirection(moveDir) * moveSpeed * 20, ForceMode.Force);

            if(rb.velocity.y > 0) {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        
        else if(grounded) {
            rb.AddForce(moveDir.normalized * moveSpeed *10, ForceMode.Force);
            
        } 
        else if(!grounded) {
            rb.AddForce(moveDir.normalized * moveSpeed  * airMultiplier*10, ForceMode.Force);
        }

        UseGravity(!OnSlope());
    }

    public void StopInput(bool stopInput) {
        canInput = !stopInput;
    }

    public bool IsPlayerMoving(){
        if(horizontalInput!=0 || verticalInput!=0 ) {
            return true;
        } else{return false;}
    }

    void SpeedControl() {
        if(OnSlope() && !exitingSlope && minimiseVelocity) {
            if(rb.velocity.magnitude > moveSpeed) {
                rb.velocity= rb.velocity.normalized * moveSpeed;
            }
        }
        else if(minimiseVelocity) {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if(flatVel.magnitude > moveSpeed) {
                Vector3 limitVel= flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitVel.x,rb.velocity.y,limitVel.z);
            }
        }
    }

    public void ResetVelocity() {
        rb.velocity = new Vector3(0f, 0f, 0f);
    }

    public float GetMoveSpeed(){
        return moveSpeed;
    }

    public void DecreaseMoveSpeed(float amountToDecrease){
        moveSpeed-=amountToDecrease;
    }
    #endregion

    #region  GroundCheck and Jumping
    bool IsGrounded() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .1f);
        if(grounded && applyDrag) {
            rb.drag = groundDrag;
        }
        else { rb.drag = airDrag; }
        if(grounded && readyToJump) {
            numJumps = maxJumps;
        }
        return grounded;
    }
    public bool isPlayerGrounded(){
        return grounded;
    }

    void UseGravity(bool useGravity){
        this.useGravity=useGravity;
    }

    void DoGravity(){
        rb.AddForce(Vector3.down *gravity, ForceMode.Acceleration);
    }

    public void SetGravityForce(float gravity=playerGravity) {
        this.gravity=gravity;
    }
    void Jump() {
        readyToJump = false;
        numJumps--;
        exitingSlope = true;
        Debug.Log("JUMPING");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(sliding) {
            rb.AddForce(transform.up * jumpForce*1.5f, ForceMode.Impulse);
            rb.AddForce(orientation.forward *50f,ForceMode.Impulse);
        }
        else {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        Invoke(nameof(ResetJump),jumpCooldown);
    }

    void ResetJump() {
        readyToJump =true;
        exitingSlope = false;
    }
    #endregion

    #region  Slopes
     public bool OnSlope() {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit , playerHeight * .5f +.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <maxSlopeAngle && angle !=0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction) {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;

    }
    #endregion

    #region  Ability
    public void SetUseAbility(bool isAbility, bool doesAffectVelocity){
        this.isAbility = isAbility;
        minimiseVelocity=!doesAffectVelocity;

    } 
    #endregion

    #region  StateHandling
     void StateHandler() {
        if(isAbility) {
            moveState = MovementState.ability;   
        }
        else if(sliding) {
            moveState = MovementState.sliding;
            if(!ps.decreasingSlide) {
                moveSpeed = slideSpeed;
            }
        }
        else if(Input.GetKey(crouchKey)) {
            moveState = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if(grounded && Input.GetKey(sprintKey)) {
            moveState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if(grounded) {
            moveState = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else {
            moveState = MovementState.air;
        }
    }
    #endregion
}
