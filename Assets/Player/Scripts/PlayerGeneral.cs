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
        LobbyUIManager.Instance?.UpdatePlayerList();
    }
}