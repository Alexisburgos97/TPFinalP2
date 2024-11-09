using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController singleton;
    
    public BarHealth barHealth;
    
    [SerializeField] private GatherInput gatherInput;
    
    [SerializeField] private float moveSpeed = 5f, rotationSpeed = 500;
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float attackDamage = 15f;
    
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private LayerMask groundLayer;

    private CameraController cameraController;
    private CharacterController characterController;
    private Animator animator;

    private Vector3 moveInput;
    
    private Quaternion targetRotation;
    
    private float ySpeed;
    private bool isGrounded;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            Debug.Log("PlayerController singleton asignado.");
        }
        else
        {
            Destroy(this.gameObject);
        }
    
        if (barHealth != null)
        {
            Debug.Log("barHealth asignado correctamente.");
        }
        else
        {
            Debug.LogWarning("barHealth no está asignado en PlayerController.");
        }

        cameraController = Camera.main.GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 60;
    }
    
    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    private void HandleMovement()
    {
        Vector2 direction = gatherInput.smoothedDirection;
        moveInput = new Vector3(direction.x, 0, direction.y);
        
        float moveAmount = Mathf.Clamp01(Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
        
        moveInput = new Vector3(gatherInput.direction.x, 0, gatherInput.direction.y);

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
            targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        animator.SetFloat("WalkSpeed", moveAmount, 0.1f, Time.deltaTime);
    }
    
    private void HandleJump()
    {
        isGrounded = GroundCheck();

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            ySpeed = jumpForce; 
            animator.SetTrigger("Jump");
        }
    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            AttemptAttack(attackDamage);
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Attack2");
            AttemptAttack(5f);
        }
    }
    
    private void AttemptAttack(float damage)
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();
            
            if (enemy != null && enemy.estaVivo)
            {
                
                Debug.Log($"Enemigo: {enemy}");
            
                Debug.DrawRay(transform.position, (enemy.transform.position - transform.position).normalized * attackRange, Color.red);
                
                Debug.Log($"Atacando a {enemy.gameObject.name}, Vida antes del ataque: {enemy.vida}");
                enemy.RecibirDaño(damage);
                Debug.Log($"Vida después del ataque: {enemy.vida}");
            }
        }
    }

    private bool GroundCheck()
    {
        bool isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
        
        return isGrounded;
    }
}
