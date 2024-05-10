using System.Collections;
using System.Collections.Generic;
using com.ethnicthv;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverController : MonoBehaviour
{
    UIDocument _uiDocument;
    
    Button _toMenuButton;
    Button _restartButton;
    Label _scoreLabel;
    
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        _toMenuButton = root.Q<Button>("toMenu");
        _toMenuButton.clicked += ToMenu;
        
        _restartButton = root.Q<Button>("restart");
        _restartButton.clicked += RestartGame;
        
        _scoreLabel = root.Q<Label>("score");
        _scoreLabel.text = GamePlay.IsLost ? "Game Over" : "Win" + $"\nScore: {GamePlay.Score}";
    }

    private static void ToMenu()
    {
        Debug.Log("To Menu");
        SceneManager.LoadScene(UsefulConstant.MainMenuScene);
    }

    private static void RestartGame()
    {
        Debug.Log("Restart Game");
        SceneManager.LoadScene(UsefulConstant.GamePlayScene);
    }
}
