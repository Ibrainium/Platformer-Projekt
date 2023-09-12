using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class SimpleController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravity = 9.81f;
    

    public UnityEvent OnDeath;
    private CharacterController controller;
    private bool isGrounded = false;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;
   
    

    // Moving platforms
    private Transform platform = null;
    private Vector3 platformOffset;
    public LayerMask platformLayer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    // Built-in ground check is bad, so use raycast instead
    private bool GroundControl()
    {
        Debug.DrawRay(transform.position + controller.center,
            Vector3.down, Color.cyan);
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + controller.center,
                                   Vector3.down),
                           out hit,
                           controller.bounds.extents.y + controller.skinWidth + 1f))
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.Log(hit.point);
            Debug.Log(transform.position + controller.center);
            if ((hit.point - (transform.position + controller.center)).magnitude > 1.1f + controller.skinWidth)
            {
                return false;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                platform = hit.transform;
                platformOffset = platform.InverseTransformPoint(transform.position);
            }
            else
            {
                platform = null;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {




        if(transform.position.y  < -40)
        {
            SceneManager.LoadScene("Gameover");
        }

        // Check if we're on the ground
        isGrounded = GroundControl();
        Debug.Log(isGrounded);

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
                animator.SetBool("IsMoving", true);
                transform.forward = new Vector3(-v, 0f, h);
            }
            else
            {
                animator.SetBool("IsMoving", false);
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
        //FindPlatform();
    }

}
