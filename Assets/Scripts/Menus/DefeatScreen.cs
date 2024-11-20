using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatScreen : MonoBehaviour
{
    ISoundController _SoundControl;

    //BUTTONS
    [SerializeField] private Button BtnReplay;
    [SerializeField] private Button BtnQuitDS;

    //SOUND
    [SerializeField] private AudioClip ClickSound;

    //NOMBRE ESCENA A CARGAR
    [SerializeField] private string FirstScene;

    private void Start()
    {
        _SoundControl = GetComponent<ISoundController>();
    }

    private void Awake()
    {
        BtnReplay.onClick.AddListener(Replay);
        BtnQuitDS.onClick.AddListener(QuitGame);
    }

    private void Replay()
    {
        _SoundControl.PlaySound(ClickSound);
        GameManager.Instance.LoadLevel(FirstScene);
    }

    private void QuitGame()
    {
        _SoundControl.PlaySound(ClickSound);
        Application.Quit();
    }
}
