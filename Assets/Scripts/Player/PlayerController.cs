using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton Variables
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }

    // Public References
    [HideInInspector] public PlayerControls controls;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PlayerHealth health;
    [HideInInspector] public PlayerMeleeAttack attack;
    [HideInInspector] public PlayerMovement movement;

    private void Awake()
    {
        CheckSingleton();

        // Set up player
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
        attack = GetComponent<PlayerMeleeAttack>();
        movement = GetComponent<PlayerMovement>();

        EnablePlayControls();
        DisableUIControls();

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        // Start repeating coroutines
        StartCoroutine(CallItemUpdate());
    }

    private void FixedUpdate()
    {
        FlipX();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnDestroy()
    {
        controls.Dispose();
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

    // Flip sprite depending on movement or attack direction
    private void FlipX()
    {
        // Flip based on movement
        if (movement.move.x > 0) // Moving Right
        {
            sprite.flipX = false;
        }
        else if (movement.move.x < 0) // Moving Left
        {
            sprite.flipX = true;
        }

        // Override if attacking
        Vector2 attackDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (attack.shouldBeDamaging && attackDirection.x > 0) // Attacking Right
        {
            sprite.flipX = false;
        }
        else if (attack.shouldBeDamaging && attackDirection.x < 0) // Attacking Left
        {
            sprite.flipX = true;
        }
    }

    // Updates items that operate every specified amount of seconds
    private IEnumerator CallItemUpdate()
    {
        foreach (ItemList i in PlayerInventory.Instance.items)
        {
            i.item.Update(this, i.stacks);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(CallItemUpdate());
    }

    // Enable gameplay controls
    public void EnablePlayControls()
    {
        controls.Player.Enable();
    }

    // Disable gameplay controls
    public void DisablePlayControls()
    {
        controls.Player.Disable();
    }

    // Enable UI controls
    public void EnableUIControls()
    {
        controls.UI.Enable();
    }

    // Disables UI controls
    public void DisableUIControls()
    {
        controls.UI.Disable();
    }
}