using Unity.Netcode;
using UnityEngine;

public class MovePipe : NetworkBehaviour
{
    public float moveSpeed = 5;
    public float destroyPipeZone = -45;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        transform.position += Vector3.left * Time.deltaTime * moveSpeed;

        if (transform.position.x < destroyPipeZone)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
