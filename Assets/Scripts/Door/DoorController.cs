using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;  
    [SerializeField] private string requiredKeyName;   
    [SerializeField] private string doorOpeningAnimation;  
    [SerializeField] private float detectionRange = 2f;
    
    [Header("Sounds")] 
    ISoundController _SoundControl;
    [SerializeField] private AudioClip DoorSound;

    private void Awake()
    {
        _SoundControl = GetComponent<ISoundController>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.PlayerSingleton.transform.position) < detectionRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (PlayerController.PlayerSingleton.HasKey(requiredKeyName))
                {
                    OpenDoor(doorOpeningAnimation); 
                    PlayerController.PlayerSingleton.UseKey(requiredKeyName);
                }
            }
        }
    }

    private void OpenDoor(string animationName)
    {
        doorAnimator.SetTrigger(animationName);
        _SoundControl.PlaySound(DoorSound);
    }
    
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
    }
    
    private void HandlePlayerDeath()
    {
        enabled = false; 
    }
}