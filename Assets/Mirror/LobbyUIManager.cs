using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mirror;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance;

    [Header("UI slots for players")]
    public GameObject[] playerSlots;

    void Awake()
    {
        Instance = this;
    }

    public void UpdatePlayerList()
    {
        // Disable all slots first
        foreach (var slot in playerSlots)
        {
            slot.SetActive(false);
        }

        int index = 0;
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity == null) continue;

            LobbyPlayer player = conn.identity.GetComponent<LobbyPlayer>();
            if (player == null) continue;

            if (index < playerSlots.Length)
            {
                GameObject slot = playerSlots[index];
                slot.SetActive(true);
                slot.GetComponentInChildren<TMP_Text>().text = player.playerName;
                index++;
            }
        }
    }
}
