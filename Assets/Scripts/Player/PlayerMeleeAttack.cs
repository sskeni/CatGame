using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private int attacks;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Allow attacking at the start
        attacks = 1;

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
        if (attacks > 0 && !shouldBeDamaging)
        {
            attacks--;

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
        PlayerController.Instance.health.hasTakenDamage = true; // Player is invincible while attacking
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

                    float damageAmount = damage.Item1;

                    bool wasCrit = damage.Item2;

                    // Items
                    foreach (ItemList j in PlayerController.Instance.inventory.items)
                    {
                        j.item.OnHit(PlayerStats.Instance, iDamageable, damageAmount, j.stacks);
                    }

                    iDamageable.Damage(damageAmount, wasCrit); // Sets hasTakenDamage to true

                    iDamageables.Add(iDamageable);
                }
            }

            yield return null;
        }

        PlayerController.Instance.health.hasTakenDamage = false; // Make player attackable again
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
        float damage = PlayerStats.Instance.attackDamage * PlayerStats.Instance.attackMultiplier;
        bool wasCrit = false;

        float lowerDamageVariance = PlayerStats.Instance.lowerDamageVariance;
        float upperDamageVariance = PlayerStats.Instance.upperDamageVariance;
        damage *= Random.Range(lowerDamageVariance, upperDamageVariance);

        // Calculate crit
        if (Random.Range(0, 100) <= PlayerStats.Instance.critChance)
        {
            damage *= 1 + (PlayerStats.Instance.critDamage / 100);
            wasCrit = true;
        }

        return new Tuple<float, bool>(damage, wasCrit);
    }

    // Keeps attack cooldown timer updated
    private void CountCooldownTimer()
    {
        if (attacks < PlayerStats.Instance.maxAttacks)
        {
            attackCooldownTimer += Time.deltaTime;
        }

        if (attackCooldownTimer >= PlayerStats.Instance.attackCooldown)
        {
            attacks++;
            attackCooldownTimer = 0f;
        }
    }

    // Returns the percentage from 0 to 1 until the attack ready
    public float GetCooldownPercentage()
    {
        return attackCooldownTimer / PlayerStats.Instance.attackCooldown;
    }

    // Returns the current number of attacks
    public int GetAttacks()
    {
        return attacks;
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
