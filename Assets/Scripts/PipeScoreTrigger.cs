using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class PipeScoreTrigger : NetworkBehaviour
{
    private HashSet<ulong> alreadyScored = new HashSet<ulong>();

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
