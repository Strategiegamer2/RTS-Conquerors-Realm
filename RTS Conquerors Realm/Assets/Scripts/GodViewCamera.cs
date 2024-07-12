using UnityEngine;

public class GodViewCamera : MonoBehaviour
{
    public float panSpeed = 20f; // Speed of camera movement
    public float scrollSpeed = 20f; // Speed of zooming
    public float rotationSpeed = 100f; // Speed of rotation

    public float minY = 20f; // Minimum zoom level
    public float maxY = 120f; // Maximum zoom level

    public float minRotationX = 40f; // Minimum X rotation angle
    public float maxRotationX = 50f; // Maximum X rotation angle

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float currentRotationY = 0f;
    private float maxRotationY = 90f; // Maximum Y rotation angle
    private float minRotationY = -90f; // Minimum Y rotation angle

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 pos = transform.position;
        Vector3 forwardMovement = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 rightMovement = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        // Keyboard input for panning
        if (Input.GetKey("w"))
        {
            pos += forwardMovement * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos -= forwardMovement * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos -= rightMovement * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos += rightMovement * panSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Adjust X rotation based on zoom level
        float t = (pos.y - minY) / (maxY - minY);
        float rotationX = Mathf.Lerp(minRotationX, maxRotationX, t);
        transform.rotation = Quaternion.Euler(rotationX, currentRotationY, 0);

        transform.position = pos;
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Delete))
        {
            currentRotationY += rotationSpeed * Time.deltaTime;
            currentRotationY = Mathf.Clamp(currentRotationY, minRotationY, maxRotationY);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentRotationY, 0);
        }
        else if (Input.GetKey(KeyCode.Insert))
        {
            currentRotationY -= rotationSpeed * Time.deltaTime;
            currentRotationY = Mathf.Clamp(currentRotationY, minRotationY, maxRotationY);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentRotationY, 0);
        }
    }

    public void ResetCamera()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentRotationY = initialRotation.eulerAngles.y;
    }
}
