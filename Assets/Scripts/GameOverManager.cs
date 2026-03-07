using Unity.Netcode;
using UnityEngine;

public class GameOverManager : NetworkBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public GameObject gameOverScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    // Every time a bird dies, we check if the game is over by counting how many birds are still alive
    public void CheckGameOver()
    {
        if (!IsServer) return;

        BirdScript[] birds = FindObjectsByType<BirdScript>(FindObjectsSortMode.None);
        int numberOfBirdsAlive = 0;

        foreach (var bird in birds)
            if (bird.isAlive.Value) numberOfBirdsAlive++;
        if (numberOfBirdsAlive == 0)
            ShowGameOverClientRpc();
    }

    // Server --> Client
    // The server calls this function to show the game over screen on all clients
    [ClientRpc]
    private void ShowGameOverClientRpc()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
    }
}