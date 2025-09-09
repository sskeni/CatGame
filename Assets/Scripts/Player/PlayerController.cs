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
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerLevel playerLevel;
    [HideInInspector] public PlayerInventory playerInventory;
    [HideInInspector] public PlayerMeleeAttack playerAttack;
    [HideInInspector] public PlayerMovement playerMovement;

    private void Awake()
    {
        CheckSingleton();

        // Set up player
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttack = GetComponent<PlayerMeleeAttack>();
        playerMovement = GetComponent<PlayerMovement>();
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
        if (playerMovement.move.x > 0) // Moving Right
        {
            sprite.flipX = false;
        }
        else if (playerMovement.move.x < 0) // Moving Left
        {
            sprite.flipX = true;
        }

        // Override if attacking
        Vector2 attackDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (playerAttack.shouldBeDamaging && attackDirection.x > 0) // Attacking Right
        {
            sprite.flipX = false;
        }
        else if (playerAttack.shouldBeDamaging && attackDirection.x < 0) // Attacking Left
        {
            sprite.flipX = true;
        }
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