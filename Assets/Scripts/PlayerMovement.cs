using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float moveSpeed = 4f;
    public float mouseSensitivity = 900f;
    public float cameraPitchLimit = 20f;

    // Private variables
    private Vector3 movementInput;
    private float yaw;                    // Player's Y-axis rotation mouse
    private float pitch;                  // Camera's X-axis rotation mouse
    private Transform cameraTransform;
    private Rigidbody playerRB;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        // Parent the camera to the player changed for better viewing.  This might work for 1st person if necessary
        //cameraTransform.SetParent(transform);

        // Set camera's local position 
        cameraTransform.localPosition = new Vector3(0f, 1.5f, 3f); // Height and distance behind the player

        // Initialize rotations
        yaw = transform.eulerAngles.y;
        pitch = cameraTransform.localEulerAngles.x;
    }

    void Update()
    {
        HandleMouseInput();
        HandleMovementInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // camera offset
        Vector3 offset = transform.forward * -3f + Vector3.up * 1.5f; // Adjust as needed

        // camera position
        cameraTransform.position = transform.position + offset;

        // Apply the pitch rotation
        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
    void HandleMouseInput()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // turn player based on x movement of mouse
        yaw += mouseX;
        transform.eulerAngles = new Vector3(0f, yaw, 0f);

        // follow mouse y movement for camera
        pitch -= mouseY; // Invert mouse Y 
        pitch = Mathf.Clamp(pitch, -cameraPitchLimit, cameraPitchLimit); // Limit the pitch
        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // keys for strafing, mouse turns
        float moveZ = Input.GetAxisRaw("Vertical");

        Debug.Log("Horiz: x: " + moveX + "Vert: Z: " + moveZ);

        // Store the movement input
        movementInput = new Vector3(moveX, 0f, moveZ).normalized;

        Debug.Log("X: " + moveX + ", " + moveZ);
    }

    void MovePlayer()
    {
        // was having issues with moving because of box collider attachment only apply movement if there is some input
        if (movementInput != Vector3.zero)
        {
            // Calculate movement direction relative to the player's orientation
            Vector3 moveDirection = transform.TransformDirection(movementInput) * moveSpeed * Time.fixedDeltaTime;

            // Move the player using Rigidbody for physics interactions
            playerRB.MovePosition(playerRB.position + moveDirection);
        }
    }

}
