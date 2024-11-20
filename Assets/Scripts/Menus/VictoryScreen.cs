using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    ISoundController _SoundControl;

    //BUTTONS
    [SerializeField] private Button BtnReplay;
    [SerializeField] private Button BtnQuitDS;

    //SCREENS
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject optionScreen;

    //SOUND
    [SerializeField] private AudioClip ClickSound;

    //SLIDER
    [SerializeField] private Slider MasterVolumeSlider;

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

        mainScreen.SetActive(true);
        optionScreen.SetActive(false);
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
