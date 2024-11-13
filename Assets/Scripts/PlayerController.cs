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

    private PlayerMovement playerMovement;

    /*THIS IS SONIDO*/
    ISoundController _SoundControl;

    [Header("Sounds")]
    [SerializeField] private AudioClip AttkSound1;
    [SerializeField] private AudioClip AttkSound2;
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
        if (barHealth != null)
        {
            barHealth.TakesDamage(damage);
        }
        else
        {
            Debug.LogWarning("BarHealth no está asignado.");
        }
    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            _SoundControl.PlaySound(AttkSound1);
            AttemptAttack(attackDamage);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Attack2");
            _SoundControl.PlaySound(AttkSound2);
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
                Debug.DrawRay(transform.position, (enemy.transform.position - transform.position).normalized * attackRange, Color.red);

                enemy.RecibirDaño(damage);
            }
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