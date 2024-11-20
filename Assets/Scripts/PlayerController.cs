using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController PlayerSingleton;

    private Animator animator;

    //THIS IS SOLID
    private PlayerMovement playerMovement;
    private PlayerAttackHandler playerAttackHandler;
    private PlayerHealth playerHealth;
    private PlayerInventory playerInventory;

    /*THIS IS SONIDO*/
    ISoundController _SoundControl;

    [Header("Sounds")]
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
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        playerAttackHandler = GetComponent<PlayerAttackHandler>();
        playerHealth = GetComponent<PlayerHealth>();
        playerInventory = GetComponent<PlayerInventory>();

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        playerMovement.HandleMovement();
        playerMovement.HandleJump();
        playerAttackHandler.HandleAttack();
        HandleCure();
    }

    public void TakesDamage(float damage)
    {
        bool IsDead = playerHealth.TakesDamage(damage);

        if (IsDead){ PlayDeathAnimation(); }
    }

    private void HandleCure()
    {
        bool aux = false;
        if (Input.GetKeyDown(KeyCode.R))
        {
            aux = playerInventory.UsePotion();

            if (playerHealth.get_currentHealth() < 100 && aux)
            {
                playerHealth.Heal(10f);
                _SoundControl.PlaySound(BottleSound);
            }
        }
    }

    //USO Y MANEJO DE LAS LLAVES
    public bool HasKey(string key){ return playerInventory.HasKey(key); }
    public void UseKey(string key) { playerInventory.UseKey(key); }

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();

        if (item)
        {
            playerInventory.AddItem(item.item, 1);
            _SoundControl.PlaySound(ItemSound);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        playerInventory.ClearItems();
    }
    
    public void PlayDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death");
            playerMovement.DisableMovement();
            
            StartCoroutine(DestroyPlayerAfterDeath());
            
            GameManager.Instance.LoadLevel("DefeatScreen");
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
}