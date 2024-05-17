using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Transform spawnPoint;
    public Transform playerTarget;
    public int initialCount = 3;
    public float timeBetweenRounds = 5f;
    public int maxGhosts = 30; // Máximo total de fantasmas en el mapa

    private float roundTimer;
    private int currentRound = 0;
    private int currentGhostCount = 0; // Contador de fantasmas actuales

    private void Start()
    {
        roundTimer = timeBetweenRounds;
        SpawnGhosts(Mathf.Min(initialCount, maxGhosts)); // Asegura que no excedamos el máximo al inicio
    }

    private void Update()
    {
        roundTimer -= Time.deltaTime;
        if (roundTimer <= 0)
        {
            currentRound++;
            int ghostsToSpawn = Mathf.Min(initialCount + currentRound * 2, maxGhosts - currentGhostCount);
            if (ghostsToSpawn > 0)
            {
                SpawnGhosts(ghostsToSpawn);
            }
            roundTimer = timeBetweenRounds;
        }
    }

    void SpawnGhosts(int count)
    {
        float reductionFactor = Mathf.Max(0.5f, 1 - 0.05f * currentRound); // Reduce el factor de daño pero no menor a 50%
        for (int i = 0; i < count; i++)
        {
            GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, Quaternion.identity);
            GhostController ghostController = ghost.GetComponent<GhostController>();
            if (ghostController != null)
            {
                ghostController.playerTarget = playerTarget;
                ghostController.SetDamageReductionFactor(reductionFactor);
                ghostController.gameManager = this; // Pasamos esta instancia de GameManager al GhostController
            }
            currentGhostCount++; // Incrementa el contador de fantasmas
        }
    }

    // Llamar cuando un fantasma muere para actualizar el contador
    public void GhostDied()
    {
        currentGhostCount--;
    }
}
