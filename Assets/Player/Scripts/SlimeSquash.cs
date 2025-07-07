using UnityEngine;

public class SlimeSquash : MonoBehaviour
{
    [SerializeField] private float squashFactor = 0.7f;
    [SerializeField] private float stretchFactor = 1.3f;
    [SerializeField] private float animationSpeed = 5f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        UpdateSquashAndStretch();
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, animationSpeed * Time.deltaTime);
    }

    private void UpdateSquashAndStretch()
    {
        float velocityY = rb.linearVelocity.y;

        if (Mathf.Abs(velocityY) < 0.1f) // Near zero (landing or idle)
        {
            targetScale = originalScale;
        }
        else if (velocityY > 0) // Going up (launch/stretch)
        {
            targetScale = new Vector3(originalScale.x * 0.9f, originalScale.y * stretchFactor, 1f);
        }
        else // Falling or bouncing down (squash)
        {
            targetScale = new Vector3(originalScale.x * squashFactor, originalScale.y * 1.2f, 1f);
        }
    }
}
