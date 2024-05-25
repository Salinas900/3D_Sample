using UnityEngine;
using UnityEngine.UI;  // Importa el espacio de nombres para trabajar con elementos UI

public class GhostController : MonoBehaviour
{
    public Transform playerTarget;
    public float health = 100f;
    public float moveSpeed = 5f;
    private float currentHealth;
    private float damageReductionFactor = 1f;
    public GameManager gameManager;
    public Image healthBar;  // Referencia a la barra de salud en la UI

    private Rigidbody rb;

    private void Start()
    {
        currentHealth = health;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is not attached to the ghost.");
        }
        if (playerTarget == null)
        {
            Debug.LogError("Player target not set in GhostController.");
        }
        healthBar.fillAmount = currentHealth / health;  // Inicializa el valor de la barra de salud
    }

    private void Update()
    {
        if (playerTarget != null)
        {
            MoveTowardsPlayer();
            LookAtPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    private void LookAtPlayer()
    {
        Vector3 lookDirection = playerTarget.position - transform.position;
        lookDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(25);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount * damageReductionFactor;
        healthBar.fillAmount = currentHealth / health;  // Actualiza la barra de salud
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void KnockBack(Vector3 direction)
    {
        float knockBackStrength = 5f;
        rb.AddForce(direction * knockBackStrength, ForceMode.Impulse);
    }

    void Die()
    {
        if (gameManager != null)
        {
            gameManager.GhostDied();
        }
        Debug.Log("Ghost died.");
        Destroy(gameObject);
    }

    public void SetDamageReductionFactor(float factor)
    {
        damageReductionFactor = factor;
    }
}
