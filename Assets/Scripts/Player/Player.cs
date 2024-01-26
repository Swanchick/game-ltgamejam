using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float cameraSensetivity = 0.7f;
    [SerializeField] private Transform playerHead;

    [Header("Player Movement")]
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerGroundAcceleration = 0.1f;
    [SerializeField] private float playerAirAcceleration = 0.5f;
    
    private CharacterController playerController;

    private float xRotation = 0f;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraRotation();
        Movement();
    }

    private void Movement()
    {

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
