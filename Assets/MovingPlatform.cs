using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IMovingObject
{
    [SerializeField] private float speed;
    [SerializeField] private float time;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private MovingPlatform script;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isMoving = true;

    private void FixedUpdate()
    {
        Vector2 newPosition = rb.position + speed * Time.fixedDeltaTime * Vector2.right;
        rb.MovePosition(newPosition);

        if (isMoving)
        {
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        isMoving = false;
        yield return new WaitForSeconds(time);
        speed *= -1;
        isMoving = true;
    }

    public void DisableScripts()
    {
         script.enabled = false;
    }

    public void DisableVisuals()
    {
        spriteRenderer.enabled = false;
    }
}


public interface IMovingObject
{
    public void DisableScripts();
    public void DisableVisuals();
}