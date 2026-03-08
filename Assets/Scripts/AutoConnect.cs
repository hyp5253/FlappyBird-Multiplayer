using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class AutoConnect : MonoBehaviour
{

    [Header("Set this before building")]
    public bool isHost = true;

    [Header("Client settings")]
    public string hostip = "127.0.0.1";
    public ushort port = 7778;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(hostip, port);
            NetworkManager.Singleton.StartClient();

        }
    }
}
