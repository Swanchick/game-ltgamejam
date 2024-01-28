using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private float playerGroundAcceleration = 0.1f;
    [SerializeField] private float playerRunAcceleration = 0.3f;
    [SerializeField] private float playerAirAcceleration = 0.5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform playerFoot;
    [SerializeField] private float groundCheckDistance = 0.25f;
    [SerializeField] private float playerJumpCooldown = 0.2f;

    [Header("Tag")]
    [SerializeField] private float tagRadius = 1f;
    [SerializeField] private LayerMask tagMask;
    [SerializeField] private PlayerHand playerHandScript;
    [SerializeField] public Transform RespawnPoint;

    public static Player Instance;

    private CharacterController playerController;

    private float xRotation = 0f;
    private Vector3 movement = Vector3.zero;
    private Vector3 referenceVelocity = Vector3.zero;
    private Vector3 downDirection = Vector3.down;
    private float gravity;
    private Vector3 gMovement = Vector3.zero;
    private bool playerJumped = false;
    private Vector3 horizontalVelocity = Vector3.zero;

    private float currentSpeed = 0f;
    private float currentAcceleration = 0f;
    private PlayerState playerState = PlayerState.Walk;

    private BaseTag currentTag;

    public static UnityEvent<Player, Vector3> PlayerMove = new UnityEvent<Player, Vector3>();
    public static UnityEvent<Player, Vector2> PlayerMouseRotate = new UnityEvent<Player, Vector2>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        playerController = GetComponent<CharacterController>();

        gravity = Physics.gravity.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        transform.position = RespawnPoint.position;
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
        movement = Vector3.Scale(movement, GetHorizontal());

        playerController.Move(movement * Time.deltaTime);

        horizontalVelocity = playerController.velocity;

        PlayerMove.Invoke(this, horizontalVelocity);
    }

    private Vector3 GetHorizontal()
    {
        return Vector3Utils.ReverseVector(-downDirection);
    }

    public void ChangeState(PlayerState newPlayerState)
    {
        playerState = newPlayerState;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public Vector3 GetHorizontalVelocity()
    {
        return horizontalVelocity;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(playerFoot.position, downDirection, groundCheckDistance, groundMask);
    }

    private void Falling()
    {
        if (IsGrounded() && !playerJumped)
        {
            
            switch (playerState)
            {
                case PlayerState.Walk:
                    currentAcceleration = playerGroundAcceleration;
                    currentSpeed = playerSpeedWalk;
                    break;
                case PlayerState.Run:
                    currentAcceleration = playerRunAcceleration;
                    currentSpeed = playerSpeedRun;
                    break;
            }
            
            gMovement = Vector3.zero;
        } 
        else
        {
            currentAcceleration = playerAirAcceleration;
            
            gMovement += downDirection * -gravity * Time.deltaTime * 2f;
            currentSpeed = playerSpeedWalk;
        }
        
        playerController.Move(gMovement * Time.deltaTime);
    }

    public void Jump(float jumpImpulse)
    {
        Jump(-downDirection, jumpImpulse);
    }

    public void Jump(Vector3 direction, float jumpImpulse)
    {
        if (playerJumped) return;

        Vector3 horizontal = GetHorizontal();

        gMovement = Vector3.zero;
        gMovement = direction * jumpImpulse;

        Vector3 hMovement = Vector3.Scale(gMovement, GetHorizontal());
        gMovement = Vector3.Scale(gMovement, Vector3Utils.Abs(downDirection));

        playerJumped = true;

        movement += hMovement * 2f;

        Invoke(nameof(ResetJump), playerJumpCooldown);
    }

    private void ResetJump()
    {
        playerJumped = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 normal = hit.normal;
        if (Vector3Utils.IsNormalWall(normal))
        {
            movement = horizontalVelocity;
        } else
        {
            normal = Vector3.down;
            gMovement = Vector3.down * -gravity;
        }
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
        Collider[] colliders = Physics.OverlapCapsule(transform.position, playerHead.position, tagRadius, tagMask);

        Debug.DrawRay(playerFoot.position, downDirection * tagRadius);

        if (colliders.Length == 0)
        {
            ClearTag();
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

    private void ClearTag()
    {
        if (!currentTag) return;

        OnTagExit(currentTag);
        currentTag = null;
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

    public void AddTag(GameObject tagObject, GameObject tagImage)
    {
        playerHandScript.AddTag(tagObject, tagImage);
    }

    public void SetCheckoutPoint(Transform NewRespawnPoint)
    {
        RespawnPoint = NewRespawnPoint;
    }

    public void Teleport()
    {
        playerController.enabled = false;
        transform.position = RespawnPoint.position;
        playerController.enabled = true;
    }
}
