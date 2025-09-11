using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Serialized References
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;

    // Private Variables
    public Vector2 move;
    public int jumpsLeft;
    private PlatformEffector2D currentOneWayPlatform;
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Set Movement Controls
        PlayerController.Instance.controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        PlayerController.Instance.controls.Player.Move.canceled += ctx => move = Vector2.zero;

        // Set Jump Controls
        PlayerController.Instance.controls.Player.Jump.performed += ctx => Jump();
        PlayerController.Instance.controls.Player.Jump.canceled += ctx => EndJump();
    }
    private void Update()
    {
        BetterGravity();
    }

    private void FixedUpdate()
    {
        Move();
        FallingAnimationHandling();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        FallThroughPlatform(collision);
    }
    
    // Move the player
    private void Move()
    {
        // Apply movement
        Vector2 movement = new Vector2(move.x, 0.0f) * PlayerStats.Instance.moveSpeed;
        rb.AddForce(movement);

        // Set animation
        if (move.x != 0)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
    }
    
    // Let player fall through one way platforms by ignoring the collision
    private void FallThroughPlatform(Collision2D collision)
    {

        if (move.y < -0.7f && collision.gameObject.GetComponent<PlatformEffector2D>() != null)
        {
            currentOneWayPlatform = collision.gameObject.GetComponent<PlatformEffector2D>(); // Store the platform so we don't lose it
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), currentOneWayPlatform.gameObject.GetComponent<CompositeCollider2D>(), true);
            StartCoroutine(ReEnablePlatform(collision));
        }
    }

    // Re enable one way platform collision once no long overlapping it
    private IEnumerator ReEnablePlatform(Collision2D collision)
    {
        // Get list of overlapping colliders
        List<Collider2D> results = new List<Collider2D>();
        while (true)
        {
            GetComponent<BoxCollider2D>().Overlap(results);
            bool found = false;
            // If one of them is a PlatformEffector2D, continue through while loop
            foreach (Collider2D collider in results)
            {
                if (collider.gameObject.GetComponent<PlatformEffector2D>() != null)
                {
                    found = true;
                }
            }
            yield return new WaitForEndOfFrame(); // Make sure the game doesn't hang
            if (found) continue;
            break;
        }
        Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), currentOneWayPlatform.gameObject.GetComponent<CompositeCollider2D>(), false); // Re-enable collision
        yield return null;
    }

    // Jump
    private void Jump()
    {
        if (IsGrounded() || jumpsLeft > 0) // Jump if the player is on the ground or grappling
        {
            rb.linearVelocityY = 0f;
            rb.AddForce(new Vector2(0, PlayerStats.Instance.jumpForce));
            rb.gravityScale = 1;
            jumpsLeft--;

            // do animation
            anim.SetBool("jumping", true);
        }
    }

    // End jump
    private void EndJump()
    {
        rb.gravityScale = 5;
    }

    // Ground check
    public bool IsGrounded()
    {
        if (Physics2D.BoxCast(
            origin: transform.position,
            size: boxSize,
            angle: 0,
            direction: -transform.up,
            distance: castDistance,
            layerMask: groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Apply more gravity when the player is falling
    private void BetterGravity()
    {
        if (rb.linearVelocityY < -0.5)
            rb.gravityScale = 5;
    }

    // Transition animation to falling
    private void FallingAnimationHandling()
    {
        // If we are in the air, we are jumping, and our vertical velocity approaches, then transition to falling
        if (!IsGrounded() && rb.linearVelocityY < 2)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }

        // Once we hit the ground and we were previously falling, transition out of falling
        if (IsGrounded() && anim.GetBool("falling"))
        {
            jumpsLeft = PlayerStats.Instance.maxJumps;
            anim.SetBool("falling", false);
        }
    }

    // Debug Gizmos
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
        //Gizmos.DrawWireCube(transform.position, new Vector2(transform.lossyScale.x, transform.lossyScale.y));
    }
}
