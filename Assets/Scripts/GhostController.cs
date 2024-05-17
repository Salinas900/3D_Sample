using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    public Transform playerTarget;
    public float health = 100f;  // Salud inicial
    private float currentHealth;
    private float damageReductionFactor = 1f;  // Factor de reducción de daño inicial
    public GameManager gameManager;  // Referencia al GameManager

    private NavMeshAgent agent;

    private void Start()
    {
        currentHealth = health;
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is not attached to the ghost.");
        }
        if (playerTarget == null)
        {
            Debug.LogError("Player target not set in GhostController.");
        }
    }

    private void Update()
    {
        if (playerTarget != null && agent != null)
        {
            agent.SetDestination(playerTarget.position);  // Configura el destino del agente al jugador
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(25); // El fantasma hace daño al jugador
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount * damageReductionFactor;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameManager != null)
        {
            gameManager.GhostDied();  // Notificar al GameManager que un fantasma ha muerto
        }
        Debug.Log("Ghost died.");
        Destroy(gameObject);
    }

    public void SetDamageReductionFactor(float factor)
    {
        damageReductionFactor = factor;
    }
}
