using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    // Serialized References
    [SerializeField] private LayerMask grappleLayer;

    // Private References
    private DistanceJoint2D joint;
    private LineRenderer line;
    private Vector3 grapplePoint;

    private void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        line = GetComponent<LineRenderer>();

        PlayerController.Instance.controls.Player.Grapple.performed += ctx => Grapple();
        PlayerController.Instance.controls.Player.Grapple.canceled += ctx => EndGrapple();
    }

    private void FixedUpdate()
    {
        RenderLine();
    }

    // Begin Grapple
    private void Grapple()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            origin: transform.position,
            direction: Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position,
            distance: Mathf.Infinity,
            layerMask: grappleLayer
            );

        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            grapplePoint.z = 0; // Ignore z
            joint.connectedAnchor = grapplePoint; // Set anchor point to hit position from raycast
            joint.enabled = true;
            joint.distance = Vector2.Distance(transform.position, hit.point); // Set distance equal to distance between anchor point and player

            // Set positions of line renderer and enable
            line.SetPosition(0, hit.point);
            line.SetPosition(1, transform.position);
            line.enabled = true;
        }
    }

    // End Grapple
    private void EndGrapple()
    {
        joint.enabled = false;
        line.enabled = false;
    }

    // Render line for grappling
    private void RenderLine()
    {
        line.SetPosition(1, transform.position);
    }
}
