using UnityEngine;

public class PlayerGeneral : MonoBehaviour
{
    public static PlayerGeneral singleton;

    private void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(gameObject);
    }
}
