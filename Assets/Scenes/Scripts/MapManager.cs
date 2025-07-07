using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor.SearchService;

public class MapManager : MonoBehaviour
{
    public static MapManager singleton;

    public HashSet<string> loadedScenesStrings = new();
    public List<UnityEngine.SceneManagement.Scene> loadedScenes = new();

    public List<GameObject> staticObjectsInScenes = new();
    public List<GameObject> movingObjectsInScenes = new();

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




    public IEnumerator EnterZone(string mainScene, string[] adjacentScenes)
    {
        movingObjectsInScenes.Clear();
        staticObjectsInScenes.Clear();

        // Scenes to keep
        HashSet<string> keepScenes = new() { mainScene };
        keepScenes.UnionWith(adjacentScenes);

        // Load main scene
        yield return LoadIfNotLoaded(mainScene);

        // Load adjacent scenes
        foreach (string scene in adjacentScenes)
        {
            yield return LoadIfNotLoaded(scene);
        }

        // Collect scenes to unload
        var toUnload = new List<string>();
        foreach (var scene in loadedScenes)
        {
            if (!keepScenes.Contains(scene.name))
            {
                toUnload.Add(scene.name);
            }
        }

        // Prepare new list of valid loaded scenes
        var stillLoaded = new List<UnityEngine.SceneManagement.Scene>();
        foreach (var scene in loadedScenes)
        {
            if (!toUnload.Contains(scene.name))
            {
                stillLoaded.Add(scene);
            }
        }

        // Update scene tracking before unloading
        loadedScenes = stillLoaded;
        foreach (string sceneName in toUnload)
        {
            loadedScenesStrings.Remove(sceneName);
        }

        // Unload scenes
        foreach (string sceneName in toUnload)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
        }

        // Collect moving and static objects from loaded scenes
        foreach (UnityEngine.SceneManagement.Scene scene in loadedScenes)
        {
            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.TryGetComponent<MovingObjects>(out var movingObjectsList))
                {
                    foreach (GameObject obj in movingObjectsList.movingObjects)
                    {
                        if (obj != null)
                            movingObjectsInScenes.Add(obj);
                    }
                    break; // If only one MovingObjects per scene
                }
            }

            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.TryGetComponent<StaticObjects>(out var staticObjects))
                {
                    foreach (GameObject obj in staticObjects.staticObjects)
                    {
                        if (obj != null)
                            staticObjectsInScenes.Add(obj);
                    }
                    break; // If only one StaticObjects per scene
                }
            }
        }

        // Populate physics simulation
        PhysicsTrajectorySimulator.PhysicsSimulatorSingleton.PopulateSimulationScene();
    }



    private IEnumerator LoadIfNotLoaded(string sceneName)
    {
        if (!loadedScenesStrings.Contains(sceneName))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);



            yield return asyncLoad;

            UnityEngine.SceneManagement.Scene loadedScene = SceneManager.GetSceneByName(sceneName);
            if (loadedScene.IsValid())
            {
                loadedScenes.Add(loadedScene);
                loadedScenesStrings.Add(sceneName);
            }
        }
    }
}
