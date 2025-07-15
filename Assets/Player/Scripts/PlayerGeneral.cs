using Mirror;
using UnityEngine;

public class PlayerGeneral : NetworkBehaviour
{
    public static PlayerGeneral singleton;

    [SyncVar]
    public string playerName;

    public override void OnStartClient()
    {
        Debug.Log("Player name is: " + playerName);
    }

    private void Awake()
    {
        singleton = this;
    }
}
