using Mirror;
using UnityEngine;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    public void SetPlayerName(string name)
    {
        if (isLocalPlayer)
        {
            CmdSetPlayerName(name);
        }
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        playerName = name;
    }

    void OnNameChanged(string oldName, string newName)
    {
        if (LobbyUIManager.Instance != null)
        {
            LobbyUIManager.Instance.UpdatePlayerList();
        }
    }

    public override void OnStartServer()
    {
        if (LobbyUIManager.Instance != null)
        {
            LobbyUIManager.Instance.UpdatePlayerList();
        }

        string name = playerName;
        ((CustomNetworkManager)NetworkManager.singleton).connectedPlayers.Add(new CustomNetworkManager.LobbyData
        {
            conn = connectionToClient,
            playerName = name
        });
    }
}