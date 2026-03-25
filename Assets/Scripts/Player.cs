using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravityForce = 30f;
    [SerializeField] private float cornerGravityForce = 50f;
    [SerializeField] private float groundCheckDistance = 0.25f;

    [Header("References")] [SerializeField]
    private Collider2D platformCollider;

    private Rigidbody2D rb;
    private float moveInput;
    private bool jumpInput;
    private bool isGrounded;
    private Vector2 currentGravityDirection;
    private Vector2 currentTangentDirection;
    private bool isNearCorner;
    private bool _uiMoveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
    }

    private void Update()
    {
        if (!_uiMoveInput) moveInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
        }
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        CheckGround();
        if (isGrounded) Move();
        if (jumpInput && isGrounded)
        {
            Jump();
        }

        jumpInput = false;
    }

    private void ApplyGravity()
    {
        if (platformCollider == null) return;

        Vector3 closestPoint = platformCollider.ClosestPoint(transform.position);
        Vector3 toPlatform = closestPoint - transform.position;
        float distance = toPlatform.magnitude;

        if (distance > 0.01f)
        {
            currentGravityDirection = toPlatform.normalized;
        }

        isNearCorner = Mathf.Abs(currentGravityDirection.x) > 0.4f && Mathf.Abs(currentGravityDirection.y) > 0.4f;

        float force = isNearCorner ? cornerGravityForce : gravityForce;
        rb.AddForce(currentGravityDirection * force);

        currentTangentDirection = new Vector2(-currentGravityDirection.y, currentGravityDirection.x);
    }

    private void Move()
    {
        if (moveInput == 0f) return;

        Vector2 moveForce = currentTangentDirection * moveInput * moveSpeed * 10f;
        rb.AddForce(moveForce);
    }

    private void Jump()
    {
        Vector2 jumpDirection = -currentGravityDirection;
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void CheckGround()
    {
        Vector2 closestPoint = platformCollider.ClosestPoint(transform.position);
        float distance = Vector2.Distance(transform.position, closestPoint);
        isGrounded = distance <= groundCheckDistance;
    }


    public void SetMoveInput(float value)
    {
        moveInput = value;
        _uiMoveInput = value != 0;
    }

    public void OnJumpButtonPressed()
    {
        jumpInput = true;
    }
}