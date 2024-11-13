using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private CharacterController characterController;
    private CameraController cameraController;
    private GatherInput gatherInput;
    private Animator animator;

    private float moveSpeed;
    private float rotationSpeed;
    private float jumpForce;
    private float ySpeed;
    private float groundCheckRadius;
    private Vector3 groundCheckOffset;
    private LayerMask groundLayer;

    public PlayerMovement(CharacterController characterController, CameraController cameraController, GatherInput gatherInput, Animator animator,
                          float moveSpeed, float rotationSpeed, float jumpForce, float groundCheckRadius, Vector3 groundCheckOffset, LayerMask groundLayer)
    {
        this.characterController = characterController;
        this.cameraController = cameraController;
        this.gatherInput = gatherInput;
        this.animator = animator;
        this.moveSpeed = moveSpeed;
        this.rotationSpeed = rotationSpeed;
        this.jumpForce = jumpForce;
        this.groundCheckRadius = groundCheckRadius;
        this.groundCheckOffset = groundCheckOffset;
        this.groundLayer = groundLayer;
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
            characterController.transform.rotation = Quaternion.RotateTowards(characterController.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
        return Physics.CheckSphere(characterController.transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }
}
