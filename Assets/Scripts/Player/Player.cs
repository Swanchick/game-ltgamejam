using UnityEngine;
using UnityEngine.Playables;

public enum PlayerState
{
    Walk,
    Run,
    Crouch
}

public class Player : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float cameraSensetivity = 0.7f;
    [SerializeField] private Transform playerHead;

    [Header("Player Movement")]
    [SerializeField] private float playerSpeedWalk = 6f;
    [SerializeField] private float playerSpeedRun = 10f;
    [SerializeField] private float playerSpeedAir = 5f;
    [SerializeField] private float playerGroundAcceleration = 0.1f;
    [SerializeField] private float playerAirAcceleration = 0.5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform playerFoot;
    [SerializeField] private float groundCheckDistance = 0.25f;
    [SerializeField] private float playerJumpImpulse = 10f;
    [SerializeField] private float playerJumpCooldown = 0.2f;

    [Header("Tag")]
    [SerializeField] private float tagRadius = 1f;
    [SerializeField] private LayerMask tagMask;

    private CharacterController playerController;

    private float xRotation = 0f;
    private Vector3 movement = Vector3.zero;
    private Vector3 referenceVelocity = Vector3.zero;
    private Vector3 downDirection = Vector3.down;
    private float gravity;
    private Vector3 gMovement;
    private bool playerJumped = false;

    private float currentSpeed = 0f;
    private float currentAcceleration = 0f;
    private PlayerState playerState = PlayerState.Walk;

    private BaseTag currentTag;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();

        gravity = Physics.gravity.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraRotation();
        Movement();
        Falling();
        FindTag();
        OnTagStay();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = transform.forward * vertical + transform.right * horizontal;
        direction.Normalize();
        direction *= currentSpeed;

        movement = Vector3.SmoothDamp(movement, direction, ref referenceVelocity, currentAcceleration);
        Vector3 horizontalDirection = Vector3Utils.ReverseVector(-downDirection);
        movement = Vector3.Scale(movement, horizontalDirection);

        Falling();

        playerController.Move(movement * Time.deltaTime);
    }

    public void ChangeState(PlayerState newPlayerState)
    {
        playerState = newPlayerState;
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(playerFoot.position, downDirection * groundCheckDistance, Color.red);

        return Physics.Raycast(playerFoot.position, downDirection, groundCheckDistance, groundMask);
    }

    private void Falling()
    {
        currentAcceleration = playerAirAcceleration;

        if (IsGrounded() && !playerJumped)
        {
            
            switch (playerState)
            {
                case PlayerState.Walk:
                    currentAcceleration = playerGroundAcceleration;
                    currentSpeed = playerSpeedWalk;
                    break;
                case PlayerState.Run:
                    currentSpeed = playerSpeedRun;
                    break;
            }
            
            gMovement = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        } 
        else
        {
            gMovement += downDirection * -gravity * Time.deltaTime;
        }

        movement += gMovement;
    }

    public void Jump()
    {
        Jump(-downDirection);
    }

    public void Jump(Vector3 direction)
    {
        if (playerJumped) return;

        gMovement = Vector3.zero;
        gMovement = direction * playerJumpImpulse;

        playerJumped = true;

        Invoke(nameof(ResetJump), playerJumpCooldown);
    }

    private void ResetJump()
    {
        playerJumped = false;
    }

    private void CameraRotation()
    {
        Vector2 mouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            );

        mouseDelta = Vector2.Lerp(mouseDelta, Vector2.zero, Time.deltaTime);
        mouseDelta *= cameraSensetivity;

        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Vector3 playerRotation = transform.rotation.eulerAngles;
        float finalRotation = playerRotation.y + mouseDelta.x;

        playerHead.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, finalRotation, 0f);
    }

    private void FindTag()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, tagRadius, tagMask);

        if (colliders.Length == 0)
        {
            if (currentTag)
            {
                OnTagExit(currentTag);
                currentTag = null;
            }
            
            return;
        }

        Collider tagCollider = colliders[0];
        if (!tagCollider) return;

        BaseTag tag = tagCollider.GetComponent<BaseTag>();
        string tagName = tag.GetTagName();

        if (currentTag)
        {
            if (currentTag.GetTagName() != tagName)
            {
                OnTagExit(currentTag);
                OnTagEnter(tag);
                currentTag = tag;
            }
        }
        else
        {
            currentTag = tag;
            OnTagEnter(tag);
        }
    }

    private void OnTagEnter(BaseTag newTag)
    {
        newTag.OnEnter(this);
    }

    private void OnTagStay()
    {
        if (!currentTag) return;
        
        currentTag.OnStay(this);
    }

    private void OnTagExit(BaseTag oldTag)
    {
        oldTag.OnExit(this);
    }
}
