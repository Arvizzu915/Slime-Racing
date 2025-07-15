using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public class LobbyData
    {
        public NetworkConnectionToClient conn;
        public string playerName;
        // Add other lobby fields here (e.g., team, color)
    }

    public List<LobbyData> connectedPlayers = new List<LobbyData>();

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

        foreach (var lobbyData in connectedPlayers)
        {
            // Destroy the LobbyPlayer if it exists
            if (lobbyData.conn.identity != null)
            {
                NetworkServer.Destroy(lobbyData.conn.identity.gameObject);
            }

            // Create and spawn the actual player prefab
            Transform startPos = GetStartPosition();
            GameObject gamePlayer = Instantiate(playerPrefab, startPos.position, startPos.rotation);

            // Assign data to the new player
            var playerComponent = gamePlayer.GetComponent<PlayerGeneral>();
            playerComponent.playerName = lobbyData.playerName;

            NetworkServer.AddPlayerForConnection(lobbyData.conn, gamePlayer);
        }

        // Optional: clear lobby list now
        connectedPlayers.Clear();
    }

}
