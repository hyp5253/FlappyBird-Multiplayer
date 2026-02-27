using Unity.Netcode;
using UnityEngine;

public class BirdScript : NetworkBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public float FlapForce = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Flap! input (owner)");
            FlapServerRpc();
        }
    }

    [ServerRpc]
    void FlapServerRpc()
    {
        Debug.Log("Flap! applied (server)");
        Rigidbody2D.linearVelocity = Vector2.up * FlapForce;
    }
}
