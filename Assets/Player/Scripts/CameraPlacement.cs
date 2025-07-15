using UnityEngine;
using Mirror;

public class CameraPlacement : MonoBehaviour
{
    public static CameraPlacement singleton;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 5f; // Speed at which camera moves
    [SerializeField] private float edgeSize = 10f; // Distance from screen edge to start scrolling

    [Header("Camera Movement Limits")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;

    public Vector3 posPivot, cameraOffset = new(20f, 8f, -10f);

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

    private void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (PlayerGeneral.singleton == null)
        {
            return;
        }

        transform.position = PlayerGeneral.singleton.transform.position + cameraOffset;
    }
}
