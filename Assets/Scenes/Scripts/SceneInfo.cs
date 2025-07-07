using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInfo : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private string[] adjacentScenes;

    [SerializeField] private Vector3 cameraPlace;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MapManager.singleton.EnterZone(targetScene, adjacentScenes));
            CameraPlacement.singleton.transform.position = cameraPlace;
        }
    }
}
