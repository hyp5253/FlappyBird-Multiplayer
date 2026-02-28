using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : NetworkBehaviour
{
    // create a network variable to hold score for each player
    public NetworkVariable<int> score = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    
    public void AddScore(int points)
    {
        if (!IsServer) return;
        score.Value += points;
    }

}
