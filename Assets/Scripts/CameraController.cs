using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float groundDrag = 6f;
    public float jumpForce = 5f;
    public float jumpCooldown = 1f;
    public float airMultiplier = 0.5f;
    private bool readyToJump = true;

    [Header("Ground Check Settings")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Key Bindings")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Player Orientation")]
    public Transform orientation;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;

        // Ensure the character has a collider
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning("No collider attached to the character! Adding a CapsuleCollider.");
            var collider = gameObject.AddComponent<CapsuleCollider>();
            collider.height = playerHeight;
            collider.radius = 0.5f; // Adjust radius as needed
        }
    }

    private void Update()
    {
        // Check if player is grounded using a Raycast
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * (playerHeight * 0.5f + 0.3f), grounded ? Color.green : Color.red);

        HandleInput();
        ControlSpeed();

        // Adjust drag based on whether the player is grounded
        rb.drag = grounded ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump logic
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // Calculate movement direction based on input and orientation
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        float forceMagnitude = moveSpeed * 10f * (grounded ? 1 : airMultiplier);

        // Apply force to move the player
        rb.AddForce(moveDirection.normalized * forceMagnitude, ForceMode.Force);
    }

    private void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity to prevent excessive speed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset vertical velocity and apply jump force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
