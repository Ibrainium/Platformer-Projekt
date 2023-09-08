using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class SimpleController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravity = 9.81f;
    [Tooltip("What happens when the robot dies?")]
    public UnityEvent OnDeath;
    private CharacterController controller;
    private bool isGrounded = false;

    private Vector3 moveDirection = Vector3.zero;

    // Moving platforms
    private Transform platform = null;
    private Vector3 platformOffset;
    public LayerMask platformLayer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we're on the ground
        isGrounded = GroundControl();

        // Get user input (old input system)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool wantJump = Input.GetButtonDown("Jump");

        // Moving platform support
        Vector3 baseDirection = Vector3.zero;
        if (platform)
        {
            baseDirection = platform.TransformPoint(platformOffset) - transform.position;
        }

        // Reset y velocity when we hit the ground
        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = 0;
        }

        // Handle movement on the ground
        if (isGrounded)
        {
            moveDirection = new Vector3(-v, moveDirection.y, h).normalized * moveSpeed;

            // Face in the move direction
            if (h != 0 || v != 0)
            {
                transform.forward = new Vector3(-v, 0f, h);
            }
        }

        // Handle jumping
        if (isGrounded && wantJump)
        {
            moveDirection.y = Mathf.Sqrt(2f * gravity * jumpHeight);
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move
        controller.Move(baseDirection + (moveDirection * Time.deltaTime));

        // Parent under platform
        FindPlatform();
    }

    // Keep track of which platform we are standing on
    // - used for moving along with moving platforms
    private void FindPlatform()
    {
        // Make a ray that points down
        Ray downRay = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;

        // Check if it hit a platform
        if (Physics.Raycast(downRay, out hit, 10f, platformLayer))
        {
            Debug.Log("on platform");
            platform = hit.transform;
            platformOffset = platform.InverseTransformPoint(transform.position);
        }
        else
        {
            Debug.Log("off platform");
            platform = null;
        }
    }

    // Built-in ground check is bad, so use raycast instead
    private bool GroundControl()
    {
        return Physics.Raycast(
            transform.position + controller.center,                     // from the middle of the controller...
            Vector3.down,                                               // ...pointing downwards...
            controller.bounds.extents.y + controller.skinWidth + 0.2f); // ... to the bottom of the controller.
    }

}
