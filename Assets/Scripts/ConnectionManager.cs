using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of ConnectionManager exists globally
    public static ConnectionManager Instance { get; private set; }

    // Awake handles setup that doesn't depend on other objects initialization
    // Connection manager is a gameObject, and is either created or destroyed
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // StartHost initializes the server and client in one step, allowing the host to play immediately
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    // StartClient takes an optional IP address and port, defaulting to localhost and port 7777
    // It configures the transport layer with the provided connection data before starting the client
    public void StartClient(string ipAddress = "127.0.0.1", ushort port = 7777)
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipAddress, port);
        NetworkManager.Singleton.StartClient();
    }

    // Gracefully shuts down the connection 
    public void Disconnect()
    {
        if (NetworkManager.Singleton.IsHost)
            NetworkManager.Singleton.Shutdown();
        else if (NetworkManager.Singleton.IsClient)
            NetworkManager.Singleton.Shutdown();
    }
}