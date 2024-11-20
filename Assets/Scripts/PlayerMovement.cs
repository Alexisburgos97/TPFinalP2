using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour { 

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500;
    [SerializeField] private float jumpForce = 4f;
    private float ySpeed;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private GatherInput gatherInput;

    private CameraController cameraController;
    private CharacterController characterController;
    private Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();

        Application.targetFrameRate = 60;
    }

    public void HandleMovement()
    {
        Vector2 direction = gatherInput.smoothedDirection;
        Vector3 moveInput = new Vector3(direction.x, 0, direction.y);
        float moveAmount = Mathf.Clamp01(Mathf.Abs(direction.x) + Mathf.Abs(direction.y));

        var moveDir = cameraController.GetYRotation * moveInput;

        if (GroundCheck())
        {
            ySpeed = -1f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveInput.sqrMagnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        animator.SetFloat("WalkSpeed", moveAmount, 0.1f, Time.deltaTime);
    }

    public void HandleJump()
    {
        if (GroundCheck() && Input.GetButtonDown("Jump"))
        {
            ySpeed = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    private bool GroundCheck()
    {
        return Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    public void DisableMovement()
    {
        moveSpeed = 0;
        jumpForce = 0;
    }
}