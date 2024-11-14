using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;  
    [SerializeField] private string requiredKeyName1;  
    [SerializeField] private string requiredKeyName2;  
    [SerializeField] private string animation1 = "Door_Hinge_Open";  
    [SerializeField] private string animation2 = "Door_Hinge_Open2"; 
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
        // Verificar si el jugador está dentro del rango de detección
        if (Vector3.Distance(transform.position, PlayerController.PlayerSingleton.transform.position) < detectionRange)
        {
            // Verificar si se presiona la tecla 'E' y si el jugador tiene la primera o la segunda llave
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (PlayerController.PlayerSingleton.inventory.HasKey(requiredKeyName1))
                {
                    OpenDoor(animation1); 
                    PlayerController.PlayerSingleton.inventory.UseKey(requiredKeyName1);
                }
                else if (PlayerController.PlayerSingleton.inventory.HasKey(requiredKeyName2))
                {
                    OpenDoor(animation2);
                    PlayerController.PlayerSingleton.inventory.UseKey(requiredKeyName2);
                }
            }
        }
    }

    private void OpenDoor(string animationName)
    {
        _SoundControl.PlaySound(DoorSound);
        doorAnimator.SetTrigger(animationName);
    }
}