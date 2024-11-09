using UnityEngine;

public class Player : MonoBehaviour
{
 
    [SerializeField] public float AttackDamage = 10f;
    [SerializeField] public float velocity;
    [SerializeField] private float jumpForce;
    [SerializeField] public float sensibilidadDelMouse;
    [SerializeField] private Animator animator;
    // [SerializeField] private float runningSpeedThreshold = 2.5f;
    private Rigidbody rb;
    
    /*public AudioSource audioSoundWalking;
    public AudioSource audioSoundSword;
    public AudioSource audioSoundBreathing;*/

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Congelar la rotación en los ejes X e Y para mantener al personaje erguido
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Rotación del personaje
        float rotacionHorizontal = Input.GetAxis("Mouse X") * sensibilidadDelMouse;
        transform.Rotate(0, rotacionHorizontal, 0);

        // Movimiento del personaje
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //var direction = new Vector3(vertical, 0, horizontal).normalized;
        //Vector3 movement = direction * speed;
        
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        Vector3 movement = direction * velocity;
        
        //Actualizar el animator con la velocidad actual
        var currentSpeed = movement.magnitude;
        animator.SetFloat("WalkSpeed", currentSpeed);
        
        transform.position += movement * Time.deltaTime;
        
        // Manejar el salto
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Attack2");
        }

        /*if (currentSpeed > 0 && IsGrounded())
        {
            if (!audioSoundWalking.isPlaying)
            {
                PlaySound(audioSoundWalking);
            }

            if (currentSpeed > runningSpeedThreshold)
            {
                audioSoundWalking.pitch = 1.5f;
            }
            else
            {
                audioSoundWalking.pitch = 1.0f;
            }

            PauseSound(audioSoundBreathing);
        }
        else
        {
            PauseSound(audioSoundWalking);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (!audioSoundBreathing.isPlaying)
                {
                    PlaySound(audioSoundBreathing);
                }
            }
            else
            {
                PauseSound(audioSoundBreathing);
            }
        }*/
    }
    
    void FixedUpdate()
    {
        // Forzar al jugador a mantenerse erguido
        Vector3 uprightRotation = new Vector3(0, transform.eulerAngles.y, 0);
        rb.MoveRotation(Quaternion.Euler(uprightRotation));
    }
    
    // Verificar si el personaje está en el suelo
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
    
    /*public void DealDamageToEnemy()
    {
        
        PlaySound(audioSoundSword);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.0f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyHeraklios enemyHealth = hitCollider.GetComponent<EnemyHeraklios>();
                if (enemyHealth != null && !enemyHealth.IsDead())
                {
                    enemyHealth.TakesDamage(AttackDamage);
                }
            }
            
            if (hitCollider.CompareTag("EnemyFinal"))
            {
                EnemyMaw enemyMawHealth = hitCollider.GetComponent<EnemyMaw>();
                if (enemyMawHealth != null && !enemyMawHealth.IsDead())
                {
                    enemyMawHealth.TakesDamage(AttackDamage);
                }
            }
        }
    }
    
    public void DealDamageToEnemy2()
    {
        
        PlaySound(audioSoundSword);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.0f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyHeraklios enemyHealth = hitCollider.GetComponent<EnemyHeraklios>();
                if (enemyHealth != null && !enemyHealth.IsDead())
                {
                    enemyHealth.TakesDamage(5f);
                }
            }
            
            if (hitCollider.CompareTag("EnemyFinal"))
            {
                EnemyMaw enemyMawHealth = hitCollider.GetComponent<EnemyMaw>();
                if (enemyMawHealth != null && !enemyMawHealth.IsDead())
                {
                    enemyMawHealth.TakesDamage(5f);
                }
            }
        }
    }/*
    
    /*private void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            
            if (!audioSource.gameObject.activeSelf)
            {
                audioSource.gameObject.SetActive(true);
            }

            // Force-enable the AudioSource component
            audioSource.enabled = true;

            audioSource.Play();
        }
    }*/


    /*private void PauseSound(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        
        if (audioSource.gameObject.activeSelf)
        {
            audioSource.gameObject.SetActive(false);
        }
    }*/
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            Rigidbody boxRb = collision.gameObject.GetComponent<Rigidbody>();
            if (boxRb != null)
            {
                Vector3 force = collision.contacts[0].normal * -1 * velocity;
                boxRb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
    
}
