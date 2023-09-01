using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMove : MonoBehaviour
{
    [Tooltip("Object to follow")]
    public Transform follow;
    [Tooltip("Max speed")]
    public float speed;

    private Vector3 offset;
    private Vector3 target;
    private float currentSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // We are not right on top of our target - start by calculating an offset
        offset = transform.position - follow.position;
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    // Follow the player
    private void Follow()
    {
        // We do not move right on top of target, but to an offset position
        target = follow.position + offset;

        // Calculate speed based on distance to target - this makes the camera
        // stop smoothly when approaching its target position.
        float distance = (target - transform.position).magnitude;
        currentSpeed = Mathf.Lerp(0, speed, 2 * distance / speed);

        // Move smoothly towards target
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}
