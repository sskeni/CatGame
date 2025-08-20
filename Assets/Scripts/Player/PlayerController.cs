using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    // Singleton Variables
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }

    // Serialized References
    [SerializeField] private LayerMask grappleAndGroundLayer;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    [SerializeField] private float attackVelocity = 25;
    [SerializeField] private LayerMask attackLayer;

    // Player Stats
    public float speed = 35f;
    public float jumpForce = 650f;
    public float attackDamage = 1f;
    public float attackCooldown = 1f;
    public float critChance = 10f;
    public float critDamage = 50f;
    public int maxJumps = 1;

    public float baseAttackDamage { get; private set; }
    public float baseSpeed { get; private set; }
    public float baseAttackCooldown { get; private set; }
    public float baseCritChance { get; private set; }
    public float baseCritDamage { get; private set; }

    // Public References
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public PlayerLevel playerLevel;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerInventory playerInventory;

    // Private References
    private PlayerControls controls;
    private DistanceJoint2D joint;
    private LineRenderer line;
    private Animator anim;

    // Movement Variables
    private Vector3 grapplePoint;
    private Vector2 move;
    private int jumpsLeft;
    private PlatformEffector2D currentOneWayPlatform;

    // Attack Variables
    public bool shouldBeDamaging { get; private set; } = false;
    private float attackCooldownTimer;
    private RaycastHit2D[] attackHits;
    private List<IDamageable> iDamageables = new List<IDamageable>();

    private void Awake()
    {
        CheckSingleton();

        // Set up player
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        line = GetComponent<LineRenderer>();
        controls = new PlayerControls();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
        playerInventory = GetComponent<PlayerInventory>();

        // Allow attacking at the start
        attackCooldownTimer = attackCooldown;

        // Set Movement Controls
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        // Set Jump Controls
        controls.Player.Jump.performed += ctx => { Jump(); /*EndGrapple();*/ };
        controls.Player.Jump.canceled += ctx => EndJump();

        // Set Grapple Controls
        //controls.Player.Grapple.performed += ctx => Grapple();
        //controls.Player.Grapple.canceled += ctx => EndGrapple();

        // Set Attack Controls
        controls.Player.Attack.performed += ctx => Attack();

        // Debug Controls
        //controls.Player.Debug.performed += ctx => Damage(1f);
    }

    private void Start()
    {
        StartCoroutine(CallItemUpdate());

        baseAttackDamage = attackDamage;
        baseSpeed = speed;
        baseAttackCooldown = attackCooldown;
        baseCritChance = critChance;
        baseCritDamage = critDamage;
    }

    private void Update()
    {
        FallingAnimationHandling();
    }

    private void FixedUpdate()
    {
        Move();
        FlipX();
        BetterGravity();
        RenderLine();
        CountCooldownTimers();
    }

    private void OnEnable()
    {
        controls.Player.Enable();

    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        FallThroughPlatform(collision);
    }

    // Set up Singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Move the player
    private void Move()
    {
        // Apply movement
        Vector2 movement = new Vector2(move.x, 0.0f) * speed;
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

    // Flip sprite depending on movement or attack direction
    private void FlipX()
    {
        // Flip based on movement
        if (move.x > 0) // Moving Right
        {
            sprite.flipX = false;
        }
        else if (move.x < 0) // Moving Left
        {
            sprite.flipX = true;
        }

        // Override if attacking
        Vector2 attackDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (shouldBeDamaging && attackDirection.x > 0) // Attacking Right
        {
            sprite.flipX = false;
        }
        else if (shouldBeDamaging && attackDirection.x < 0) // Attacking Left
        {
            sprite.flipX = true;
        }
    }

    // Jump
    private void Jump()
    {
        if (IsGrounded() || jumpsLeft > 0 || joint.enabled) // Jump if the player is on the ground or grappling
        {
            rb.linearVelocityY = 0f;
            rb.AddForce(new Vector2(0, jumpForce));
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
            jumpsLeft = maxJumps;
            anim.SetBool("falling", false);
        }
    }

    // Ground check
    public bool IsGrounded()
    {
        if(Physics2D.BoxCast(
            origin: transform.position,
            size: boxSize,
            angle: 0,
            direction: -transform.up,
            distance: castDistance,
            layerMask: grappleAndGroundLayer))
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

    // Begin Grapple
    private void Grapple()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            origin: transform.position,
            direction: Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position,
            distance: Mathf.Infinity,
            layerMask: grappleAndGroundLayer
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

    // Attack all damageables in range
    private void Attack()
    {
        if (attackCooldownTimer >= attackCooldown)
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
        playerHealth.hasTakenDamage = true; // Player is invincible while attacking

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
                    foreach (ItemList j in playerInventory.items)
                    {
                        j.item.OnHit(this, iDamageable, j.stacks);
                    }

                    iDamageable.Damage(damage.Item1, damage.Item2); // Sets hasTakenDamage to true

                    iDamageables.Add(iDamageable);
                }
            }

            yield return null;
        }

        playerHealth.hasTakenDamage = false; // Make player attackable again
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
        float damage = attackDamage;
        bool wasCrit = false;

        // Calculate crit
        if (UnityEngine.Random.Range(1, 101) <= critChance)
        {
            damage *= 1 + (critDamage / 100);
            wasCrit = true;
        }
        
        return new Tuple<float, bool>(damage, wasCrit);
    }

    // Keeps attack cooldown timer updated
    private void CountCooldownTimers()
    {
        attackCooldownTimer += Time.deltaTime;
    }

    // Updates items that operate every specified amount of seconds
    private IEnumerator CallItemUpdate()
    {
        foreach (ItemList i in playerInventory.items)
        {
            i.item.Update(this, i.stacks);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(CallItemUpdate());
    }

    // Debug Gizmos
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
        //Gizmos.DrawWireCube(transform.position, new Vector2(transform.lossyScale.x, transform.lossyScale.y));
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
