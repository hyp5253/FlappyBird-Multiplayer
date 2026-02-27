using Unity.Netcode;
using UnityEngine;

public class Score : NetworkBehaviour
{
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
