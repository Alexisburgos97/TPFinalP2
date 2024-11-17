using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDoor : MonoBehaviour
{
    [SerializeField] private GameObject doorMessage; 

    private void Start()
    {
        if (doorMessage != null)
        {
            doorMessage.SetActive(false); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (doorMessage != null)
            {
                doorMessage.SetActive(true); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (doorMessage != null)
            {
                doorMessage.SetActive(false); 
            }
        }
    }
}
