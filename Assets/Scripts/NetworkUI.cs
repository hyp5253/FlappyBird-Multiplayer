using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using TMPro;

public class NetworkUI : MonoBehaviour
{
    public GameObject connectionPanel;
    public TMP_InputField ipInputField;

    public void OnHostClicked()
    {
        NetworkManager.Singleton.StartHost();
        HideConnectionPanel();
    }

    public void OnClientClicked()
    {
        string ip = ipInputField.text;
        if (string.IsNullOrEmpty(ip))
            ip = "127.0.0.1";

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, 7778);
        NetworkManager.Singleton.StartClient();
        HideConnectionPanel();
    }

    private void HideConnectionPanel()
    {
        if (connectionPanel != null)
            connectionPanel.SetActive(false);
    }
}