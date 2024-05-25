using UnityEngine;
using UnityEngine.UI;  // Necesitas esta directiva para trabajar con elementos de UI

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float health = 100f;
    public float maxHealth = 100f;  // Añade una variable para el máximo de salud para facilitar cálculos
    public Image healthBar;  // Referencia a la barra de vida que es un componente Image

    private bool isGrounded;
    private float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        SetCursorState(true);
        healthBar.fillAmount = health / maxHealth;  // Inicializa el valor de la barra de salud
    }

    private void Update()
    {
        GroundCheck();
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
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
        RaycastHit hit;
        float range = 100f;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Ghost"))
            {
                GhostController ghost = hit.collider.gameObject.GetComponent<GhostController>();
                if (ghost != null)
                {
                    ghost.TakeDamage(health * 0.25f);
                    Vector3 knockBackDirection = (hit.collider.transform.position - transform.position). normalized;
                    ghost.KnockBack(knockBackDirection);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;  // Actualiza la barra de salud
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        SetCursorState(false);  // Unlock and show the cursor before changing scene
        SceneController.Instance.LoadScene("GameOver");
    }

    private void SetCursorState(bool cursorLocked)
    {
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
