using UnityEngine;

public class EnemyAutoJump : MonoBehaviour
{
    // Component References
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Enemy enemy;

    // Jump Stats
    [SerializeField] private float jumpForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemy.isGrounded()) Jump();
    }

    // Make the enemy jump
    private void Jump()
    {
        rb.linearVelocityY = 0;
        rb.AddForceY(jumpForce);
    }
}
