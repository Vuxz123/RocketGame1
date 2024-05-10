using com.ethnicthv;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    UIDocument _uiDocument;
    
    Button _startButton;
    
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        _startButton = root.Q<Button>("start");
        _startButton.clicked += StartGame;
    }
    
    void StartGame()
    {
        //Create a new game
        SceneManager.LoadScene(UsefulConstant.GamePlayScene);
    }
}
