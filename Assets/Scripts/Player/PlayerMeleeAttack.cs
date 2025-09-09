using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private float attackVelocity = 25;
    [SerializeField] private LayerMask attackLayer;

    // Component References
    private Rigidbody2D rb;
    private Animator anim;

    // Attack Variables
    public bool shouldBeDamaging { get; private set; } = false;
    private float attackCooldownTimer;
    private RaycastHit2D[] attackHits;
    private List<IDamageable> iDamageables = new List<IDamageable>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Allow attacking at the start
        attackCooldownTimer = PlayerStats.Instance.attackCooldown;

        // Set Attack Controls
        PlayerController.Instance.controls.Player.Attack.performed += ctx => Attack();
    }

    private void FixedUpdate()
    {
        CountCooldownTimer();
    }

    // Attack all damageables in range
    private void Attack()
    {
        if (attackCooldownTimer >= PlayerStats.Instance.attackCooldown)
        {
            // Run animation
            anim.SetTrigger("attack");

            // Apply physics
            Vector2 attackDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            attackDirection = attackDirection.normalized;
            rb.linearVelocity = attackDirection * attackVelocity;
            rb.gravityScale = 5;

            // Reset the timer
            attackCooldownTimer = 0f;
        }
    }

    // Coroutine to identify and attack all items in range
    public IEnumerator DamageWhileAnimationIsActive()
    {
        shouldBeDamaging = true;
        PlayerController.Instance.playerHealth.hasTakenDamage = true; // Player is invincible while attacking
        rb.gravityScale = 1; // Lower gravity while attacking

        while (shouldBeDamaging)
        {
            // Get all damageables in range
            attackHits = Physics2D.BoxCastAll(
                origin: transform.position,
                size: new Vector2(transform.lossyScale.x, transform.lossyScale.y), // Get size of player hitbox
                angle: 0,
                direction: transform.up,
                distance: 0,
                layerMask: attackLayer);

            // Damage all attackables found
            for (int i = 0; i < attackHits.Length; i++)
            {
                IDamageable iDamageable = attackHits[i].collider.gameObject.GetComponent<IDamageable>();

                if (iDamageable != null && !iDamageable.hasTakenDamage) // Check hasTakenDamage to make sure we don't attack one object multiple times
                {
                    Tuple<float, bool> damage = CalculateAttackDamage();

                    // Items
                    foreach (ItemList j in PlayerController.Instance.playerInventory.items)
                    {
                        j.item.OnHit(PlayerStats.Instance, iDamageable, j.stacks);
                    }

                    iDamageable.Damage(damage.Item1, damage.Item2); // Sets hasTakenDamage to true

                    iDamageables.Add(iDamageable);
                }
            }

            yield return null;
        }

        PlayerController.Instance.playerHealth.hasTakenDamage = false; // Make player attackable again
        rb.gravityScale = 5f; // Enable gravity again
        ReturnAttackablesToDamageable();
    }

    // Resets IDamageables so they cannot be attacked in the same frame
    private void ReturnAttackablesToDamageable()
    {
        foreach (IDamageable damageable in iDamageables)
        {
            damageable.hasTakenDamage = false;
        }

        iDamageables.Clear();
    }

    // Calculates attack damage
    private Tuple<float, bool> CalculateAttackDamage()
    {
        float damage = PlayerStats.Instance.attackDamage;
        bool wasCrit = false;

        // Calculate crit
        if (UnityEngine.Random.Range(1, 101) <= PlayerStats.Instance.critChance)
        {
            damage *= 1 + (PlayerStats.Instance.critDamage / 100);
            wasCrit = true;
        }

        return new Tuple<float, bool>(damage, wasCrit);
    }

    // Keeps attack cooldown timer updated
    private void CountCooldownTimer()
    {
        attackCooldownTimer += Time.deltaTime;
    }

    #region Animation Triggers

    public void ShouldBeDamagingToTrue()
    {
        shouldBeDamaging = true;
    }

    public void ShouldBeDamagingToFalse()
    {
        shouldBeDamaging = false;
    }

    #endregion
}
