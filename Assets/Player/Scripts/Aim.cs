using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    [SerializeField] private float throwForce = 0;
    [SerializeField] private Rigidbody2D rb;

    //Throw direction and force
    private Vector2 startPoint;
    private Vector2 currentMousePos;

    [SerializeField] private float maxForce = 15f;

    private bool isAiming = false;

    //Grounded
    private bool isGrounded;

    //simulation
    [SerializeField] private TrajectoryVisualizer visualizer;

    private Vector2 velocitySim;

    private void Update()
    {
        if (isAiming)
        {
            currentMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            Vector2 launchVector = startPoint - currentMousePos;
            Vector2 direction = launchVector.normalized;
            float force = Mathf.Clamp(launchVector.magnitude * throwForce, 0f, maxForce);
            velocitySim = direction * force;

            var points = PhysicsTrajectorySimulator.PhysicsSimulatorSingleton.SimulateTrajectory(transform.position, velocitySim);
            visualizer.ShowTrajectory(points);
        }
    }

    public void AimInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isAiming = true;
            startPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        if (ctx.canceled && isAiming)
        {
            isAiming = false;

            visualizer.ClearDots();

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(velocitySim, ForceMode2D.Impulse);
        }
    }
}
