using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class PipeScoreTrigger : NetworkBehaviour
{
    private bool hasScored = false; // To prevent multiple scoring for the same trigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        var score = collision.GetComponent<Score>();
        if (score != null && !hasScored)
        {
            score.AddScore(1); // Increment score by 1
            hasScored = true; // Prevent double-scoring
        }
    }

}
