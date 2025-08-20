using System.Xml.Serialization;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject laserProjectilePrefab;
    [SerializeField] private GameObject projectileOrigin;

    private bool lookAtPlayer = false;
    private GameObject playerRef;
    private float attackCooldownTimer;

    private void FixedUpdate()
    {
        RotateToPlayer();
        CountCooldownTimer();
    }

    private void RotateToPlayer()
    {
        if (lookAtPlayer && playerRef != null)
        {
            Quaternion rotationTarget = Quaternion.LookRotation(Vector3.forward, playerRef.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, rotateSpeed * Time.deltaTime);
            Attack();
        }
    }

    private void CountCooldownTimer()
    {
        attackCooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if(attackCooldownTimer >= attackSpeed)
        {
            GameObject laser = Instantiate(laserProjectilePrefab);
            laser.transform.position = projectileOrigin.transform.position;
            laser.transform.rotation = projectileOrigin.transform.rotation;
            laser.transform.SetParent(null);
            attackCooldownTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            playerRef = collision.gameObject;
            lookAtPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            playerRef = null;
            lookAtPlayer = false;
        }
    }
}
