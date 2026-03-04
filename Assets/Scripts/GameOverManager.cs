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
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void CheckGameOver()
    {
        if (!IsServer) return;

        // FIXME don't use the depracated FindObjectsOfTyp
        BirdScript[] birds = FindObjectsOfType<BirdScript>();
        int numberOfBirdsAlive = 0;

        foreach (var bird in birds)
        {
            if (bird.isAlive.Value) numberOfBirdsAlive++;
        }

        if (numberOfBirdsAlive == 0)
        {
            ShowGameOverClientRpc();
        }
    }

    [ClientRpc]
    private void ShowGameOverClientRpc()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }
}