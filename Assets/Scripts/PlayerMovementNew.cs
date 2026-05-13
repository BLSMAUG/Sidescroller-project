using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public float coyoteTimer;
    public float lagJumpTimer;

    public GameObject bulleInterraction;

    public static PlayerMovementNew instance;

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Debug.Log(isGrounded);
        Physics();
    }

    #region Menu

    public GameObject SettingsMenuGO;
    public GameObject ExitConfirmGO;
    public void PauseMenu(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SettingsMenuGO.SetActive(true);
        }
    }

    public void OnResumeButton()
    {
        SettingsMenuGO.SetActive(false);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnMainMenuButton()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SceneManager.LoadScene(0);
        }
        else if (SceneManager.GetActiveScene().name == "Combat")
        {
            ExitConfirmGO.SetActive(true);
        }
    }

    public void OnExitConfirmButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitDenyButton()
    {
        ExitConfirmGO.SetActive(false);
    }

    #endregion

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        
        
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
        if(context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (context.performed && !isGrounded)
        {
            lagJumpTimer = 0.100f;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        
    }

    void Physics()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        float targetSpeed = horizontalMovement * moveSpeed;
        float acceleration = isGrounded ? groundAcceleration : airAcceleration;
        float newX = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);

        if (coyoteTimer > 0)
        {
            coyoteTimer -= Time.deltaTime;
            isGrounded = true;
            if (coyoteTimer <= 0)
            {
                isGrounded = false;
                coyoteTimer = 0;
            }
        }

        if (lagJumpTimer > 0)
        {
            lagJumpTimer -= Time.deltaTime;
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                lagJumpTimer = 0;
            }
            if (lagJumpTimer <= 0)
            {
                lagJumpTimer = 0;
            }
        }

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

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.fixedDeltaTime;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.linearVelocity.y < 0)
        {
            coyoteTimer = 0.075f;
        }
    }
}
