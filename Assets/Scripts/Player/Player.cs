using UnityEngine;

public enum PlayerState
{
    Walk,
    Run,
    Crouch,
    Air
}

public class Player : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float cameraSensetivity = 0.7f;
    [SerializeField] private Transform playerHead;

    [Header("Player Movement")]
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerGroundAcceleration = 0.1f;
    [SerializeField] private float playerAirAcceleration = 0.5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform playerFoot;
    [SerializeField] private float groundCheckDistance = 0.25f;

    private CharacterController playerController;

    private float xRotation = 0f;
    private Vector3 movement = Vector3.zero;
    private Vector3 referenceVelocity = Vector3.zero;
    private Vector3 downDirection = Vector3.down;
    private float gravity;
    private Vector3 gMovement;

    private float currentAcceleration = 0f;
    private PlayerState playerGroundState = PlayerState.Walk;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();

        gravity = Physics.gravity.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        currentAcceleration = playerGroundAcceleration;

        CameraRotation();
        Movement();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = transform.forward * vertical + transform.right * horizontal;
        direction.Normalize();
        direction *= playerSpeed;

        movement = Vector3.SmoothDamp(movement, direction, ref referenceVelocity, currentAcceleration);

        Falling();

        playerController.Move(movement * Time.deltaTime);
    }

    private void ChangeState(PlayerState playerState)
    {

    }

    private bool IsGrounded()
    {
        Debug.DrawRay(playerFoot.position, downDirection * groundCheckDistance, Color.red);

        return Physics.Raycast(playerFoot.position, downDirection, groundCheckDistance, groundMask);
    }

    private void Falling()
    {
        if (IsGrounded())
        {
            currentAcceleration = playerGroundAcceleration;
            gMovement = Vector3.zero;
        } else
        {
            gMovement += downDirection * -gravity * Time.deltaTime * 0.1f;
            currentAcceleration = playerAirAcceleration;
        }

        movement += gMovement;
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
}
