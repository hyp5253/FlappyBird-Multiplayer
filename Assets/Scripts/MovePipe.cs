using Unity.Netcode;
using UnityEngine;

public class MovePipe : NetworkBehaviour
{
    // external tunables 
    public float moveSpeed = 5;
    public float destroyPipeZone = -45;

    // move the pipe to the left every frame by moveSpeed and destroy it if it goes off screen
    void Update()
    {
        if (!IsServer) return;
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // if a pipe has gone off screen we don't need it anymore so destroy it
        if (transform.position.x < destroyPipeZone)
            GetComponent<NetworkObject>().Despawn();
    }
}
