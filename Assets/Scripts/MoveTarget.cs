using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public enum MovementType
    {
        Linear,
        Random
    }

    [Header("Movement Settings")]
    public MovementType movementType = MovementType.Linear;
    public float moveSpeed = 5f;

    [Header("Linear Movement")]
    public Vector3 linearDirection = Vector3.forward;

    [Header("Random Movement")]
    public float randomChangeInterval = 2f;
    public float randomRange = 5f;

    private Vector3 randomDirection;
    private float timer;

    void Start()
    {
        randomDirection = GetRandomDirection();
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.Linear:
                transform.position += linearDirection.normalized * moveSpeed * Time.deltaTime;
                break;

            case MovementType.Random:
                timer += Time.deltaTime;
                if (timer >= randomChangeInterval)
                {
                    randomDirection = GetRandomDirection();
                    timer = 0f;
                }
                transform.position += randomDirection * moveSpeed * Time.deltaTime;
                break;
        }
    }

    Vector3 GetRandomDirection()
    {
        return new Vector3(
            Random.Range(-randomRange, randomRange),
            Random.Range(-randomRange, randomRange),
            Random.Range(-randomRange, randomRange)
        ).normalized;
    }
}
