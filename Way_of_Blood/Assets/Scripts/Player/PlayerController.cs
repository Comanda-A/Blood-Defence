using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    Animator animator;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;
    public float sprintSpeed = 30.0f;
    public float rollSpeed = 40.0f;
    public float rollDuration = 0.5f;

    private bool isRolling = false;
    private float rollTime;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        // Flip sprite when moving right
        if (horizontal > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = false;
        }

        // Set AnimState for running
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetInteger("AnimState", 2);
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }

        // Attack with sword on left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        // Roll on spacebar press
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            StartRoll();
        }
    }

    void FixedUpdate()
    {
        if (isRolling)
        {
            if (rollTime > 0)
            {
                rollTime -= Time.fixedDeltaTime;
                body.linearVelocity = new Vector2(horizontal * rollSpeed, vertical * rollSpeed);
            }
            else
            {
                isRolling = false;
            }
        }
        else
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : runSpeed;

            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            body.linearVelocity = new Vector2(horizontal * currentSpeed, vertical * currentSpeed);
        }
    }

    void StartRoll()
    {
        isRolling = true;
        rollTime = rollDuration;
    }
}