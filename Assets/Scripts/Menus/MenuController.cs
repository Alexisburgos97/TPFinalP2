using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private UIDocument _doc;
    private Button _playButton;
    private Button _exitButton;
    private Button _optionsButton;
    private Button _creditsButton;

    private VisualElement _buttonsWrapper;
    [SerializeField] private VisualTreeAsset _settingsButtonsTemplate;
    private VisualElement _optionsButtons;



    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _playButton = _doc.rootVisualElement.Q<Button>("play-button");
        _playButton.clicked += PlayButtonOnClicked;

        _exitButton = _doc.rootVisualElement.Q<Button>("ExitButton");
        _exitButton.clicked += ExitButtonOnClicked;

        _creditsButton = _doc.rootVisualElement.Q<Button>("credits-button");
        _creditsButton.clicked += CreditsButtonOnClicked;

        _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");
        _optionsButton = _doc.rootVisualElement.Q<Button>("OptionsButton");
        _optionsButton.clicked += OptionsButtonOnClicked;

        _optionsButtons = _settingsButtonsTemplate.CloneTree();
        var backButton = _optionsButtons.Q<Button>("back-Button");
        backButton.clicked += BackButtonOnClicked;
    }

    private void PlayButtonOnClicked()
    {
        SceneManager.LoadScene("Game");
    }

    private void ExitButtonOnClicked()
    {
        Application.Quit();
    }

    private void CreditsButtonOnClicked()
    {
        SceneManager.LoadScene("Credits");
    }

    private void OptionsButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_optionsButtons);
    }

    private void BackButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_playButton);
        _buttonsWrapper.Add(_exitButton);
        _buttonsWrapper.Add(_optionsButton);
        _buttonsWrapper.Add(_creditsButton);
    }

}