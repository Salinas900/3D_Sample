using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Asegúrate de incluir este namespace para trabajar con TextMeshPro

public class GameManager : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Transform spawnPoint;
    public Transform playerTarget;
    public int initialCount = 5;
    public float timeBetweenRounds = 5f;
    public TMP_Text scoreText;  // Componente TextMeshPro para mostrar la puntuación
    private int score = 0;  // Puntuación actual

    private int maxGhosts = 30;
    private float roundTimer;
    private int currentRound = 0;
    private int currentGhostCount = 0;
    private bool roundActive = false;

    private void Start()
    {
        scoreText.text = "Score: " + score;  // Inicializa el texto de puntuación
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
        int ghostsToSpawn = initialCount + 2 * (currentRound - 1);
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
        score += 100;  // Añade 100 puntos por cada fantasma derrotado
        scoreText.text = "Score: " + score;  // Actualiza el texto de puntuación

        if (currentGhostCount == 0)
        {
            roundActive = false;
            roundTimer = timeBetweenRounds;
        }
    }

    public void PlayerDied()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
