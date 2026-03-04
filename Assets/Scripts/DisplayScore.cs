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
        scoreText.text = "Scores:\n";

        BirdScript[] birds = FindObjectsOfType<BirdScript>();

        foreach (var bird in birds)
        {
            Score score = bird.GetComponent<Score>();
            if (score != null) 
            {
                scoreText.text += $"P {bird.OwnerClientId}: {score.score.Value}\n";
            }
        }

    }
}
