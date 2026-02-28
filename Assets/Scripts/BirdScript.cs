using Unity.Netcode;
using UnityEngine;

public class BirdScript : NetworkBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public float FlapForce = 10f;

    private SpriteRenderer spriteRenderer;
    public NetworkVariable<bool> isAlive = new NetworkVariable<bool>(true);

    // Predefined color palette for players
    private static readonly Color[] PlayerColors = new Color[]
    {
        Color.yellow,      // Player 1
        Color.cyan,        // Player 2
        Color.magenta,     // Player 3
        Color.green,       // Player 4
        new Color(1f, 0.5f, 0f), // Orange
        Color.red
    };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // When the player spawns on the network, assign them a color and set their sorting order
    // this overrides the basic nsb 
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Assign color based on owner client ID
        int colorIndex = (int)(OwnerClientId) % PlayerColors.Length;
        spriteRenderer.color = PlayerColors[colorIndex];

        // Set sorting order - owned player on top
        spriteRenderer.sortingOrder = (IsOwner) ? 10 : 0;
    }

    // The player makes a request to the server to flap
    // ex "hey server I want to flap" and the server will validate and execute that request
    void Update()
    {
        if (!IsOwner) return;
        if (!isAlive) return;
        {
            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlapServerRpc();
        }
    }

    // function executed on server side
    [ServerRpc]
    void FlapServerRpc()
    {
        Rigidbody2D.linearVelocity = Vector2.up * FlapForce;
    }
}
