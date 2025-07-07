using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using System;

public class PhysicsTrajectorySimulator : MonoBehaviour
{
    public static PhysicsTrajectorySimulator PhysicsSimulatorSingleton;

    [SerializeField] private GameObject playerGhostPrefab;
    [SerializeField] private int steps = 7;
    [SerializeField] private float timeStep = 0.1f;

    private Scene simulationScene;
    private PhysicsScene2D physicsScene;
    private GameObject ghostInstance;
    private List<Vector2> trajectoryPoints = new();

    private List<(Rigidbody2D real, Rigidbody2D sim)> Movings = new();

    private void Awake()
    {
        CreatePhysicsScene();

        PhysicsSimulatorSingleton = this;

        //SceneManager.LoadSceneAsync("StartZone", LoadSceneMode.Additive);

        DontDestroyOnLoad(gameObject);
    }

    public void PopulateSimulationScene()
    {
        foreach (GameObject obj in simulationScene.GetRootGameObjects())
        {
            if (obj != ghostInstance)
                Destroy(obj);
        }

        Movings.Clear();

        foreach (var staticObj in MapManager.singleton.staticObjectsInScenes)
        {
            if (staticObj == null) continue;

            GameObject staticClone = Instantiate(staticObj, staticObj.transform.position, staticObj.transform.rotation);
            SceneManager.MoveGameObjectToScene(staticClone, simulationScene);
        }

        foreach (var obj in MapManager.singleton.movingObjectsInScenes)
        {
            if (obj == null) continue;

            GameObject simClone = Instantiate(obj, obj.transform.position, obj.transform.rotation);
            SceneManager.MoveGameObjectToScene(simClone, simulationScene);

            IMovingObject simCloneScript = simClone.GetComponent<IMovingObject>();
            if (simCloneScript != null)
            {
                simCloneScript.DisableVisuals();
                simCloneScript.DisableScripts();
            }

            var realRb = obj.GetComponent<Rigidbody2D>();
            var simRb = simClone.GetComponent<Rigidbody2D>();

            if (realRb != null && simRb != null)
            {
                Movings.Add((realRb, simRb));
            }
        }
    }


    private void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("PhysicsSim", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        ghostInstance = Instantiate(playerGhostPrefab);
        ghostInstance.GetComponent<SpriteRenderer>().enabled = false; // hide it
        SceneManager.MoveGameObjectToScene(ghostInstance, simulationScene);
    }

    public void SyncAllMoving()
    {
        foreach (var (real, sim) in Movings)
        {
            SyncMovingToSimulation(real, sim);
        }
    }

    private void SyncMovingToSimulation(Rigidbody2D realMoving, Rigidbody2D simMoving)
    {
        simMoving.position = realMoving.position;
        simMoving.rotation = realMoving.rotation;
        simMoving.linearVelocity = realMoving.linearVelocity;
        simMoving.angularVelocity = realMoving.angularVelocity;

        simMoving.Sleep();
        simMoving.WakeUp();
    }

    public List<Vector2> SimulateTrajectory(Vector2 position, Vector2 velocity)
    {
        trajectoryPoints.Clear();

        Rigidbody2D rb = ghostInstance.GetComponent<Rigidbody2D>();
        ghostInstance.transform.position = position;
        rb.linearVelocity = velocity;
        rb.angularVelocity = 0;
        rb.rotation = 0;

        SyncAllMoving();


        for (int i = 0; i < steps; i++)
        {
            SyncAllMoving();
            physicsScene.Simulate(timeStep);



            trajectoryPoints.Add(ghostInstance.transform.position);
        }

        return trajectoryPoints;
    }
}
