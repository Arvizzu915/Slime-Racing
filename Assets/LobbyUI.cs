using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public Button startGameButton;

    void Start()
    {
        startGameButton.gameObject.SetActive(NetworkServer.active && NetworkClient.isConnected);
        startGameButton.onClick.AddListener(() => {
            ((CustomNetworkManager)NetworkManager.singleton).GoToGameScene();
        });
    }
}
