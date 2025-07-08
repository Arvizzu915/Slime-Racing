using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public void StartHostFromMenu()
    {
        StartHost();
    }

    public void StartClientFromMenu(string ipAddress)
    {
        networkAddress = ipAddress;
        StartClient();
    }

    public void GoToGameScene()
    {
        ServerChangeScene("Bootstrap");
    }

}
