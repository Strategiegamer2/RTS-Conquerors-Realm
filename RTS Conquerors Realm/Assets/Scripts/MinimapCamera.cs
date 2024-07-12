using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform playerTransform;
    public float height = 50f;

    void LateUpdate()
    {
        Vector3 newPosition = playerTransform.position;
        newPosition.y = height;
        transform.position = newPosition;
    }
}
