using UnityEngine;

public class TagBullet : MonoBehaviour
{
    [SerializeField] private float buleltSpeed = 30f;
    [SerializeField] private GameObject tagObject;

    private CharacterController bulletController;
    private float gravityMovement = 0f;
    private float gravity = 0f;

    private void Start()
    {
        gravity = Physics.gravity.y;

        bulletController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 movement = transform.forward * buleltSpeed;
        gravityMovement += gravity * Time.deltaTime * 0.5f;

        movement += gravityMovement * Vector3.up;

        bulletController.Move(movement * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Hitted");

        GameObject tag = Instantiate(tagObject, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

        Destroy(gameObject);
    }
}
