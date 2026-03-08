using Unity.Netcode;
using UnityEngine;

public class GameOverManager : NetworkBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public GameObject gameOverScreen;
    public GameObject startButton;

    // let us know if the game has started or not across all clients by using a network variable
    private NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false);
    //private bool gameStartedLocal = false;

    private void Awake()
    {
        Instance = this;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    // can only start the game from the server aka click the start button
    public void StartGame()
    {
        if (!IsServer) return;
        gameStarted.Value = true;
        //gameStartedLocal = true;
        StartGameClientRpc();
    }

    // Server --> Client
    // The server calls this function to hide the start button on all clients
    [ClientRpc]
    private void StartGameClientRpc()
    {
        //gameStartedLocal = true;
        startButton.SetActive(false);
    }

    // arrow function to check if the game has started or not
    public bool IsGameStarted() => gameStarted.Value;

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