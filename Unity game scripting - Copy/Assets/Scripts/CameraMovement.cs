using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 20f;  // Speed of camera movement
    public float rotationSpeed = 10f;  // Speed of camera rotation
    public float zoomSpeed = 20f;  // Speed of camera zoom
    public float verticalSpeed = 1f; // Speed of vertical movement (Q and E)
    public float mouseSensitivity = 200f; // Sensitivity of mouse movement

    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float vertical = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            vertical = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            vertical = 1;
        }

        Vector3 forwardMovement = transform.forward * v * moveSpeed * Time.deltaTime;
        Vector3 rightMovement = transform.right * h * moveSpeed * Time.deltaTime;
        Vector3 verticalMovement = transform.up * vertical * verticalSpeed * Time.deltaTime;

        transform.position += forwardMovement + rightMovement + verticalMovement;
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(2)) // Middle mouse button for rotation
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoom = scroll * zoomSpeed * transform.forward * Time.deltaTime;
        transform.Translate(zoom, Space.World);
    }
}
