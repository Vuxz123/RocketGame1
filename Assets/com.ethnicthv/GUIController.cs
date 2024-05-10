using UnityEngine;
using UnityEngine.UIElements;

namespace com.ethnicthv
{
    public class GUIController : MonoBehaviour
    {
        UIDocument _uiDocument;
        // Start is called before the first frame update
        
        Label _scoreLabel;
        VisualElement _colorBox;
        
        void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            var root = _uiDocument.rootVisualElement;
            _scoreLabel = root.Q<Label>("score");
            _scoreLabel.text = "0/10";
            
            _colorBox = root.Q<VisualElement>("color");
        }
        
        /// <summary>
        /// Update Score on UI
        /// </summary>
        /// <param name="score"></param>
        public void UpdateScore(int score)
        {
            _scoreLabel.text = $"{score}/10";
        }
        
        /// <summary>
        /// Update Color on UI
        /// </summary>
        /// <param name="color"></param>
        public void UpdateColor(Color color)
        {
            _colorBox.style.backgroundColor = color;
        }
    }
}
