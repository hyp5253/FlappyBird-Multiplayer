using JetBrains.Annotations;
using Unity.Netcode;
using UnityEditor.Build.Content;
using UnityEngine;

public class BirdScript : NetworkBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public float FlapForce = 10f;

    private SpriteRenderer spriteRenderer;
    public NetworkVariable<bool> isAlive = new NetworkVariable<bool>(true);

    public GameObject GameOverScreen;

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
        if (isAlive.Value != true) return;
        if (Input.GetKeyDown(KeyCode.Space)) FlapServerRpc();
    }

    // function executed on server side
    [ServerRpc]
    void FlapServerRpc()
    {
        Rigidbody2D.linearVelocity = Vector2.up * FlapForce;
    }

    // if the player collides with a pipe they can't flap anymore aka isAlive is false
    // also we have to check asks the GameOverManager to check if the game is over because of this collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Pipe"))
        {
            isAlive.Value = false;

            if (GameOverManager.Instance != null)
            {
                GameOverManager.Instance.CheckGameOver();
            }
        }
    }


}
