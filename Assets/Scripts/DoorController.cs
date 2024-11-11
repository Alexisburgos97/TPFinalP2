using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;  
    [SerializeField] private string requiredKeyName1;  
    [SerializeField] private string requiredKeyName2;  
    [SerializeField] private string animation1 = "Door_Hinge_Open";  
    [SerializeField] private string animation2 = "Door_Hinge_Open2"; 
    [SerializeField] private float detectionRange = 2f; 

    private void Update()
    {
        // Verificar si el jugador está dentro del rango de detección
        if (Vector3.Distance(transform.position, PlayerController.singleton.transform.position) < detectionRange)
        {
            // Verificar si se presiona la tecla 'E' y si el jugador tiene la primera o la segunda llave
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (PlayerController.singleton.inventory.HasKey(requiredKeyName1))
                {
                    OpenDoor(animation1); 
                    PlayerController.singleton.inventory.UseKey(requiredKeyName1);
                }
                else if (PlayerController.singleton.inventory.HasKey(requiredKeyName2))
                {
                    OpenDoor(animation2);
                    PlayerController.singleton.inventory.UseKey(requiredKeyName2);
                }
            }
        }
    }

    private void OpenDoor(string animationName)
    {
        doorAnimator.SetTrigger(animationName);
    }
}