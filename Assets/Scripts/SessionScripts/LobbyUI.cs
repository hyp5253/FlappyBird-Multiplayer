using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    [Header("Main Menu Panel")]
    public GameObject mainMenuPanel;
    public Button hostButton;
    public Button joinButton;
    public TMP_InputField ipInputField;

    [Header("Lobby Panel")]
    public GameObject lobbyPanel;
    public TMP_Text playerListText;
    public Button startGameButton;
    public Button leaveLobbyButton;

    // Awake is called when the script instance is being loaded, before any Start methods
    // this one attaches buttons to event listeners
    private void Awake()
    {
        Instance = this;

        hostButton.onClick.AddListener(OnHostClicked);
        joinButton.onClick.AddListener(OnJoinClicked);
        startGameButton.onClick.AddListener(OnStartGameClicked);
        leaveLobbyButton.onClick.AddListener(OnLeaveLobbyClicked);

        ShowMainMenu();
    }

    // become host and show lobby ui
    private void OnHostClicked()
    {
        ConnectionManager.Instance.StartHost();
        ShowLobby();
        startGameButton.interactable = false; // Will enable when enough players join
    }

    // become client and show lobby ui
    private void OnJoinClicked()
    {
        string ipAddress = string.IsNullOrEmpty(ipInputField.text) ? "127.0.0.1" : ipInputField.text;
        ConnectionManager.Instance.StartClient(ipAddress);
        ShowLobby();
        startGameButton.gameObject.SetActive(false); // Only host can start
    }

    // only host can start the game, this will call the lobby manager to start the game
    private void OnStartGameClicked()
    {
        if (LobbyManager.Instance != null)
            LobbyManager.Instance.StartGame();
    }

    // disconnect from the lobby and return to main menu
    private void OnLeaveLobbyClicked()
    {
        ConnectionManager.Instance.Disconnect();
        ShowMainMenu();
    }

    // update the player list text with the current players in the lobby, also enable start button if host and enough players
    public void UpdatePlayerList(NetworkList<ulong> players)
    {
        playerListText.text = $"Players ({players.Count}):\n";
        for (int i = 0; i < players.Count; i++)
        {
            playerListText.text += $"Player {players[i]}\n";
        }

        // Enable start button if host and enough players
        if (NetworkManager.Singleton.IsHost && LobbyManager.Instance != null)
        {
            startGameButton.interactable = LobbyManager.Instance.CanStartGame();
        }
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    private void ShowLobby()
    {
        mainMenuPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }
}