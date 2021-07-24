using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroidCrasher;
using TMPro;

public class Menu : MonoBehaviour
{
    Dictionary<InputType,string> _inputTypeTextPairs;

    [SerializeField]
    GameCycleManager _gameCycleManager;
    [SerializeField]
    InputControll _inputControll;
    [SerializeField]
    GameObject _continueButton;
    [SerializeField]
    GameObject _changeButton;
    TMP_Text _changeButtonText;
    [SerializeField]
    GameObject _menu;

    int _switchCounter;

    void OnGameStateChanged()
    {
        switch (_gameCycleManager.CurrentGameState)
        {
            case GameState.OnPause:
                {
                    _continueButton.SetActive(true);
                    ShowMenu();
                    break;
                }
                
            case GameState.Running:
                {
                    HideMenu();
                    break;
                }
                
            case GameState.MainMenu:
                {
                    _continueButton.SetActive(false);
                    ShowMenu();
                    break;
                }
                
            default:
                break;
        }
    }

    public void ShowMenu()
    {
        _menu.SetActive(true);
    }
    
    public void HideMenu()
    {
        _menu.SetActive(false);
    }

    public void ChangeInput()
    {
        _switchCounter++;
        Debug.Log(_switchCounter);
        var types = System.Enum.GetValues(typeof(InputType));
        _switchCounter %= types.Length;
        var type = (InputType)types.GetValue(_switchCounter);
        _inputControll.SetInputType(type);
        _changeButtonText.text = _inputTypeTextPairs[type];
    }

    public void PauseGame()
    {
        _gameCycleManager.PauseGame();
    }

    public void ResumeGame()
    {
        _gameCycleManager.ResumeGame();
    }

    public void StartNewGame()
    {
        _gameCycleManager.StartNewGame();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

  

    // Start is called before the first frame update
    void Start()
    {
        _gameCycleManager.GameStateChanged += OnGameStateChanged;
        _inputTypeTextPairs = new Dictionary<InputType, string>();
        _inputTypeTextPairs.Add(InputType.Keyboard, "”правление: клавиатура");
        _inputTypeTextPairs.Add(InputType.KeyboardAndMouse, "”правление: клавиатура + мышь");
        _changeButtonText = _changeButton.GetComponentInChildren<TMP_Text>();
        _switchCounter = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_gameCycleManager.CurrentGameState == GameState.Running)
            {
                Debug.Log("pause");
                PauseGame();
                return;
            }
            

            if(_gameCycleManager.CurrentGameState == GameState.OnPause)
            {
                ResumeGame();
                Debug.Log("continue");
                return;
            }
                
                
        }
    }
}
