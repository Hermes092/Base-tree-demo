using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 9f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("Sprint Settings")]
    public float sprintDuration = 3.5f;
    public float sprintCooldown = 3.5f;

    private bool isDashing = false;
    private float dashTime;
    private float nextDashTime;
    private Vector3 dashDirection;

    private bool isSprinting = false;
    private float sprintTimeLeft;
    private float nextSprintTime;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        float currentSpeed = moveSpeed;

        // Sprint input
        if (Input.GetKey(KeyCode.LeftShift) && Time.time >= nextSprintTime && sprintTimeLeft > 0f)
        {
            isSprinting = true;
            sprintTimeLeft -= Time.deltaTime;
            currentSpeed = sprintSpeed;

            if (sprintTimeLeft <= 0f)
            {
                isSprinting = false;
                nextSprintTime = Time.time + sprintCooldown;
            }
        }
        else
        {
            isSprinting = false;

            // Regenerate sprint when not sprinting and cooldown finished
            if (Time.time >= nextSprintTime)
            {
                sprintTimeLeft = Mathf.Min(sprintTimeLeft + Time.deltaTime, sprintDuration);
            }
        }

        // Normal movement (disabled while dashing)
        if (!isDashing)
        {
            transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);

            // Start dash
            if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextDashTime && moveDirection != Vector3.zero)
            {
                isDashing = true;
                dashTime = dashDuration;
                dashDirection = moveDirection;
                nextDashTime = Time.time + dashCooldown;
            }
        }

        // Dash movement
        if (isDashing)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);

            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false;
            }
        }
    }

    void Start()
    {
        sprintTimeLeft = sprintDuration;
    }
}