using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("WaitToEnd", 15); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenuUI");
        }
    }

    public void WaitToEnd()
    {
        SceneManager.LoadScene("MainMenuUI");
    }
}
