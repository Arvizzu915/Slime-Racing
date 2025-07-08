using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public TMP_InputField ipInput;
    public void OnHostClick()
    {
        ((CustomNetworkManager)NetworkManager.singleton).StartHostFromMenu();
    }

    public void OnJoinClick()
    {
        ((CustomNetworkManager)NetworkManager.singleton).StartClientFromMenu(ipInput.text);
    }
}
