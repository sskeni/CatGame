using System.Xml.Serialization;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    // Enemy Stats
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject laserProjectilePrefab;
    [SerializeField] private GameObject projectileOrigin;
    [SerializeField] private AudioClip laserShootSoundClip;
    [SerializeField] private float laserShootVolume;
    [SerializeField] private float laserShootPitchRange;

    // Private References
    private bool lookAtPlayer = false;
    private GameObject playerRef;
    private float attackCooldownTimer;

    private void Start()
    {
        attackCooldownTimer = Random.Range(0f, attackSpeed);
    }

    private void FixedUpdate()
    {
        RotateToPlayer();
        CountCooldownTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerRef = collision.gameObject;
            lookAtPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerRef = null;
            lookAtPlayer = false;
        }
    }

    // Rotate towards the player
    private void RotateToPlayer()
    {
        if (lookAtPlayer && playerRef != null)
        {
            Quaternion rotationTarget = Quaternion.LookRotation(Vector3.forward, playerRef.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, rotateSpeed * Time.deltaTime);
            Attack();
        }
    }

    // Count the attack timer
    private void CountCooldownTimer()
    {
        attackCooldownTimer += Time.deltaTime;
    }

    // Shoot a laser
    private void Attack()
    {
        if(attackCooldownTimer >= attackSpeed)
        {
            GameObject laser = Instantiate(laserProjectilePrefab);
            laser.transform.position = projectileOrigin.transform.position;
            laser.transform.rotation = projectileOrigin.transform.rotation;
            laser.transform.SetParent(null);
            attackCooldownTimer = 0;
            SoundFXHandler.Instance.PlaySoundFXClip(laserShootSoundClip, transform, laserShootVolume, laserShootPitchRange, laserShootPitchRange);
        }
    }
}
