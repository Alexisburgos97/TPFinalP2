using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController PlayerSingleton;

    [Header("HUD Elements")]
    public BarHealth barHealth;

    [Header("Inventory")]
    public InventoryObject inventory;

    [SerializeField] private GatherInput gatherInput;

    [Header("Player Stats")]
    [SerializeField] private float moveSpeed = 5f, rotationSpeed = 500;
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float strongAttackRange = 2.1f;
    [SerializeField] private float strongAttackDamage = 30f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private LayerMask groundLayer;

    private CameraController cameraController;
    private CharacterController characterController;
    private Animator animator;

    //THIS IS SOLID
    private PlayerMovement playerMovement;
    private PlayerAttackHandler playerAttackHandler;
    private PlayerHealth playerHealth;

    /*THIS IS SONIDO*/
    ISoundController _SoundControl;

    [Header("Sounds")]
    [SerializeField] private AudioClip attkSound1;
    [SerializeField] private AudioClip attkSound2;
    [SerializeField] private AudioClip BottleSound;
    [SerializeField] private AudioClip ItemSound;
    /*THIS IS SONIDO*/
    
    public delegate void PlayerDeathHandler();
    public static event PlayerDeathHandler OnPlayerDeath;

    private void Awake()
    {
        if (PlayerSingleton == null)
        {
            PlayerSingleton = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        _SoundControl = GetComponent<ISoundController>();
        characterController = GetComponent<CharacterController>();
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();

        playerMovement = new PlayerMovement(
            characterController, cameraController, gatherInput, animator,
            moveSpeed, rotationSpeed, jumpForce, groundCheckRadius, groundCheckOffset, groundLayer
        );

        playerAttackHandler = new PlayerAttackHandler(animator, _SoundControl, attkSound1, attkSound2, attackDamage, 
                                                        attackRange, strongAttackRange, strongAttackDamage, transform);
        playerHealth = new PlayerHealth(100, barHealth);

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        playerMovement.HandleMovement();
        playerMovement.HandleJump();
        HandleAttack();
        HandleCure();
    }

    /*RECIBIR DAÑO - 13-11-2024*/
    public void TakesDamage(float damage)
    {
        bool IsDead = playerHealth.TakesDamage(damage);

        if (IsDead){ PlayDeathAnimation(); }
    }

    public void HandleAttack()
    {
        playerAttackHandler.HandleAttack();
    }

    private void HandleCure()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UsePotion();
        }
    }
    public void UsePotion()
    {
        if (playerHealth.get_currentHealth() < 100 && inventory.HasItemOfType(ItemType.Potion))
        {
            playerHealth.Heal(10f);

            /*barHealth.recibeCure(healthToRestore);*/

            inventory.UsePotion();
            _SoundControl.PlaySound(BottleSound);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();

        if (item)
        {
            inventory.AddItem(item.item, 1);
            _SoundControl.PlaySound(ItemSound);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Items.Clear();
    }
    
    public void PlayDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death"); 
            DisableMovement();
            
            StartCoroutine(DestroyPlayerAfterDeath());
            
            // SceneManager.LoadScene("GameOver");
        }
    }
    
    IEnumerator DestroyPlayerAfterDeath()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float deathAnimationDuration = stateInfo.length;

        yield return new WaitForSeconds(deathAnimationDuration);

        // Notifica a otros scripts
        OnPlayerDeath?.Invoke();

        Destroy(gameObject);
    }
    
    public void DisableMovement()
    {
        moveSpeed = 0;               
        jumpForce = 0;              
    }
}