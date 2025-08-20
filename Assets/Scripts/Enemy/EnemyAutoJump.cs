using UnityEngine;

public class EnemyAutoJump : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Jump();
    }

    private void Jump()
    {
        rb.linearVelocityY = 0;
        rb.AddForceY(jumpForce);
    }
}
