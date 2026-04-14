using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementNew : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundAcceleration = 15f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float airAcceleration = 4f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash")]
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 5f;

    private Rigidbody2D rb;
    float horizontalMovement;
    private bool isGrounded;
    private bool jumpRequested;

    private bool isDashing;
    private bool dashRequested;
    private float dashTimer;
    private float cooldownTimer;

    public GameObject bulleInterraction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Physics();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (horizontalMovement > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            bulleInterraction.transform.localScale = new Vector3(1, 1, 1);
        }

        else if (horizontalMovement < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            bulleInterraction.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        
    }

    public void Dash(InputAction.CallbackContext context)
    {
        
    }

    void Physics()
    {
        float targetSpeed = horizontalMovement * moveSpeed;
        float acceleration = isGrounded ? groundAcceleration : airAcceleration;
        float newX = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);

        // dash
        if (dashRequested)
        {
            isDashing = true;
            dashTimer = dashDuration;
            cooldownTimer = dashCooldown;
            dashRequested = false;

            float dashDir = horizontalMovement != 0 ? horizontalMovement : transform.localScale.x;
            rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);
        }

        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
                isDashing = false;
            return;
        }

        // cooldown dash
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.fixedDeltaTime;
    }
}
