using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour, IDamageable
{

    public static PlayerController PlayerSingleton;

    public BarHealth barHealth;

    public InventoryObject inventory;

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

    //THIS IS SOLID
    private PlayerMovement playerMovement;
    private PlayerAttackHandler playerAttackHandler;

    /*THIS IS SONIDO*/
    ISoundController _SoundControl;

    [Header("Sounds")]
    [SerializeField] private AudioClip attkSound1;
    [SerializeField] private AudioClip attkSound2;
    [SerializeField] private AudioClip BottleSound;
    [SerializeField] private AudioClip ItemSound;
    /*THIS IS SONIDO*/

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

        playerAttackHandler = new PlayerAttackHandler(animator, _SoundControl, attkSound1, attkSound2, attackDamage, attackRange, transform);

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        playerMovement.HandleMovement();
        playerMovement.HandleJump();
        playerAttackHandler.HandleAttack();
        HandleCure();
    }

    /*RECIBIR DAÑO - 13-11-2024*/
    public void TakesDamage(float damage)
    {
        if (barHealth != null)
        {
            barHealth.TakesDamage(damage);
        }
        else
        {
            Debug.LogWarning("BarHealth no está asignado.");
        }
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
        if (barHealth.Health < 100 && inventory.HasItemOfType(ItemType.Potion))
        {
            const float potionHealthRestore = 10f;

            // Calculamos cuánto se puede restaurar sin exceder el límite de salud
            float healthToRestore = Mathf.Min(potionHealthRestore, 100 - barHealth.Health);

            barHealth.recibeCure(healthToRestore);

            inventory.UsePotion();
            _SoundControl.PlaySound(BottleSound);

            if (!inventory.HasItemOfType(ItemType.Potion))
            {
                Debug.Log("Ya no hay más pociones.");
            }
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
}