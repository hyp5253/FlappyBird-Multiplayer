using UnityEngine;
using Unity.Netcode;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject GameOverScreen;

    // we get each player's score component and display it on the screen
    void Update()
    {
        // Mini score section title
        scoreText.text = "Scores:\n";

        BirdScript[] birds = FindObjectsByType<BirdScript>(FindObjectsSortMode.None);

        // display the score for each player
        foreach (var bird in birds)
        {
            Score score = bird.GetComponent<Score>();
            if (score != null) // guard case before players score any points
                scoreText.text += $"P{bird.OwnerClientId}: {score.score.Value}\n";
        }

    }
}
