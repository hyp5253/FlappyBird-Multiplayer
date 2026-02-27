using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

public class SpawnPipe : NetworkBehaviour
{
    public NetworkObject pipe;
    public float spawnRate = 2;
    private float timer = 0;
    public float heightOffset = 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        if (timer < spawnRate) timer += Time.deltaTime;
        else
        {
            spawnPipe();
            timer = 0;
        }
    }

    void spawnPipe() 
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;
        var pipeNetworkObject = Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);

        pipeNetworkObject.Spawn();
    }
}
