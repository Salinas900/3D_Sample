using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Transform spawnPoint;
    public Transform playerTarget;
    public int initialCount = 5; // Comenzar con 5 fantasmas
    public float timeBetweenRounds = 5f; // 5 segundos entre rondas
    private int maxGhosts = 30; // Máximo total de fantasmas para controlar la dificultad

    private float roundTimer;
    private int currentRound = 0;
    private int currentGhostCount = 0;
    private bool roundActive = false;

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (currentGhostCount == 0 && !roundActive)
        {
            roundTimer -= Time.deltaTime;
            if (roundTimer <= 0)
            {
                StartRound();
            }
        }
    }

    void StartRound()
    {
        roundActive = true;
        currentRound++;
        int ghostsToSpawn = initialCount + 2 * (currentRound - 1); // Añadir 2 fantasmas más cada ronda
        if (ghostsToSpawn > maxGhosts)
            ghostsToSpawn = maxGhosts;

        SpawnGhosts(ghostsToSpawn);
        roundTimer = timeBetweenRounds;
    }

    void SpawnGhosts(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (ghostPrefab == null)
            {
                Debug.LogError("Ghost prefab is missing or destroyed");
                return;
            }
            GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, Quaternion.identity);
            GhostController ghostController = ghost.GetComponent<GhostController>();
            if (ghostController != null)
            {
                ghostController.playerTarget = playerTarget;
                ghostController.gameManager = this;
            }
            currentGhostCount++;
        }
    }

    public void GhostDied()
    {
        currentGhostCount--;
        if (currentGhostCount == 0)
        {
            roundActive = false; // Finaliza la ronda actual, inicia cuenta regresiva para la próxima ronda
            roundTimer = timeBetweenRounds; // Reinicia el temporizador para la siguiente ronda
        }
    }

    public void PlayerDied()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
