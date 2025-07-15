using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public Button startGameButton;

    public GameObject[] playerConnectedUI;

    void Start()
    {
        Debug.Log("Binding listener");
        startGameButton.onClick.AddListener(() => {
            Debug.Log("StartGameButton clicked");
            ((CustomNetworkManager)NetworkManager.singleton).GoToGameScene();
        });
    }
}
