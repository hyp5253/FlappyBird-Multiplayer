using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance { get; private set; }

    // Using a NetworkList to keep track of connected players in a networked environment
    private NetworkList<ulong> connectedPlayers;
    private const int MIN_PLAYERS = 2;
    private const int MAX_PLAYERS = 4;

    // initializes the singleton instance and sets up the NetworkList for tracking connected players
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        connectedPlayers = new NetworkList<ulong>();
    }

    // This method is called by the NetworkManager when a client attempts to connect. It approves all connections and prevents
    // automatic player object creation, allowing the lobby manager to handle player instantiation manually.
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, 
                               NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = false;
        response.Pending = false;
    }

    // When the network spawns, the lobby manager sets up callbacks for client connections and disconnections, and listens
    // for changes to the player list
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        connectedPlayers.OnListChanged += OnPlayerListChanged;
    }

    // When the network despawns, the lobby manager cleans up all callbacks and clears the player list to ensure a
    // fresh state for the next session
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = null;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;

            // Clear the player list when network despawns
            connectedPlayers.Clear();
        }

        connectedPlayers.OnListChanged -= OnPlayerListChanged;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!IsServer) return;

        if (connectedPlayers.Count < MAX_PLAYERS)
        {
            connectedPlayers.Add(clientId);
        }
    }

    // When a client disconnects, the lobby manager removes them from the connected players list, ensuring that the lobby state remains
    // accurate and up-to-date for all remaining clients.
    private void OnClientDisconnected(ulong clientId)
    {
        if (!IsServer) return;

        for (int i = 0; i < connectedPlayers.Count; i++)
        {
            if (connectedPlayers[i] == clientId)
            {
                connectedPlayers.RemoveAt(i);
                break;
            }
        }

    }

    private void OnPlayerListChanged(NetworkListEvent<ulong> changeEvent)
    {
        LobbyUI.Instance?.UpdatePlayerList(connectedPlayers);
    }

    // This method is called by the host when they click the "Start Game" button. It checks if there are
    // enough players to start the game
    public void StartGame()
    {
        if (!IsServer) return;

        if (connectedPlayers.Count >= MIN_PLAYERS)
        {
            StartGameClientRpc();
        }
    }

    // This ClientRpc is responsible for spawning player objects for each connected client and transitioning to the game scene.
    [ClientRpc]
    private void StartGameClientRpc()
    {
        if (IsServer)
        {
            foreach (var clientId in connectedPlayers)
            {
                var playerObject = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab);
                var networkObject = playerObject.GetComponent<NetworkObject>();
                networkObject.SpawnAsPlayerObject(clientId);
            }

            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    // Utility methods to get player count and check if the game can start based on the number of connected players
    public int GetPlayerCount() => connectedPlayers.Count;
    public bool CanStartGame() => connectedPlayers.Count >= MIN_PLAYERS;
}