using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float health = 100f;  // Salud inicial del jugador

    private bool isGrounded;
    private float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        GroundCheck();
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))  // Detecta clic izquierdo para atacar
        {
            Attack();
        }
    }

    private void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float moveHorizontal = Input.GetAxis("Horizontal") * speed;
        float moveVertical = Input.GetAxis("Vertical") * speed;

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        rb.MovePosition(rb.position + movement * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.Impulse);
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, groundCheckDistance + 0.1f, groundLayer);
    }

    private void Attack()
    {
        // Implementa el raycast o la lógica para detectar y dañar a los fantasmas frente al jugador
        Debug.Log("Attack initiated!");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Player health: {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Implementa la lógica de la muerte del jugador, como reiniciar el nivel o mostrar un menú de game over
    }
}
