using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class PipeScoreTrigger : NetworkBehaviour
{
    // use a hashset to check if a bird has already scored
    // allows for multiple players instead of a single global pass through flag
    private HashSet<ulong> alreadyScored = new HashSet<ulong>();

    // when a bird enters the trigger, check if it has already scored and if not, increment the score
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        var bird = collision.GetComponent<BirdScript>();
        if (bird != null)
        {
            var score = bird.GetComponent<Score>();
            if (score != null && !alreadyScored.Contains(bird.OwnerClientId))
            {
                score.score.Value += 1;
                alreadyScored.Add(bird.OwnerClientId);
            }
        }
    }

}
