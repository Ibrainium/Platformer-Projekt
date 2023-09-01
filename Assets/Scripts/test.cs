using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float moveSpeed = 2.0f;

    private Vector3 currentTarget;
    private bool movingToEnd = true;

    private void Start()
    {
        currentTarget = endPoint.position;
    }

    private void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);

        if (transform.position == currentTarget)
        {
            if (movingToEnd)
            {
                transform.position = startPoint.position; // Snap back to startPoint
            }
            else
            {
                currentTarget = endPoint.position;
            }

            movingToEnd = !movingToEnd;
        }


    }
}
