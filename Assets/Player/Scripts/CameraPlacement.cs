using UnityEngine;

public class CameraPlacement : MonoBehaviour
{
    public static CameraPlacement singleton;

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
