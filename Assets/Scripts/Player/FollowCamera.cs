using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Camera References
    [SerializeField] private Transform target;
    [SerializeField] private float cameraSpeed;

    private void FixedUpdate()
    {
        FollowTarget();
    }

    // Moves the camera towards the follow target
    private void FollowTarget()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, cameraSpeed);
    }
}
