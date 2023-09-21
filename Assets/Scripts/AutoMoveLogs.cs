using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoMoveLogs : MonoBehaviour
{
    [SerializeField] private Transform StartPoint;
    [SerializeField] private Transform EndPoint;
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = EndPoint.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {

            transform.position = StartPoint.position; // Snap back to startPoint
        }

        else
        {
            targetPosition = EndPoint.position;
        }
        


    }
}
