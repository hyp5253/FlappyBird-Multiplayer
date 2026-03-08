using Unity.Netcode;
using UnityEngine;

public class SpawnPipe : NetworkBehaviour
{
    // reference to the pipe prefab we want to spawn handled by the network manager
    public NetworkObject pipe;

    // external tunables
    public float spawnRate = 6;
    private float timer = 0;
    public float heightOffset = 6;

    // overriding the behaviour of the network spawn to spawn a pipe immediately when the game starts
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        //spawnPipe();
    }

    // Check if we need to spawn a pipe every by tracking time in spawnRate
    void Update()
    {
        if (!IsServer) return;
        if (!GameOverManager.Instance.IsGameStarted()) return;
        if (timer < spawnRate) timer += Time.deltaTime;
        else
        {
            spawnPipe();
            timer = 0;
        }
    }

    // spawns a pipe at a random height within the height offset range
    void spawnPipe() 
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;
        var pipeNetworkObject = Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);

        pipeNetworkObject.Spawn();
    }
}
