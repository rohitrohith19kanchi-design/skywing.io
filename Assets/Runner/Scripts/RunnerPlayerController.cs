using UnityEngine;

public class RunnerPlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float holdJumpForce = 25f;
    [SerializeField] private float maxJumpHoldTime = 0.25f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isHoldingJump;
    private bool canPlay;
    private float jumpHoldTimer;

    private RunnerGameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<RunnerGameManager>();
    }

    private void Start()
    {
        canPlay = false;
    }

    private void Update()
    {
        if (!canPlay)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();

        if (Input.GetKey(KeyCode.Space) &&
            isHoldingJump &&
            jumpHoldTimer < maxJumpHoldTime &&
            rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity +=
                Vector2.up * holdJumpForce * Time.deltaTime;

            jumpHoldTimer += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
            isHoldingJump = false;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isGrounded = false;
        isHoldingJump = true;
        jumpHoldTimer = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (gameManager != null)
                gameManager.GameOver();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    public void EnablePlayer()
    {
        canPlay = true;
    }

    public void DisablePlayer()
    {
        canPlay = false;
        isHoldingJump = false;
    }

    public void ResetPlayer(Vector3 startPosition)
    {
        transform.position = startPosition;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Re-enable jumping after restart.
        isGrounded = true;
        isHoldingJump = false;
        jumpHoldTimer = 0f;
        canPlay = false;
    }
}