using Unity.Netcode;
using UnityEngine;

public class BirdScript : NetworkBehaviour
{
    public Rigidbody2D Rigidbody2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D.linearVelocity = Vector2.up * 10;
        }
    }
}
