using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.ethnicthv
{
    public class GamePlay: MonoBehaviour
    {
        public MapBehaviour map;
        public GUIController guiController;
        
        //State
        private static int _score;
        private int _currentColor;

        public static int Score => _score;
        public static bool IsLost { get; private set; }

        public Color CurrentColor => _colorMap[_currentColor];
        //
        
        private const float _collectableRespawnTime = UsefulConstant.CollectableRespawnTime;
        
        private float _deltaTime;

        private Color[] _colorMap = {
            Color.green,
            Color.red,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Color.magenta,
            Color.white,
        };
        
        private void Start()
        {
            IsLost = false;
            
            _deltaTime = _collectableRespawnTime;
            _score = 0;
            _currentColor = 0;
            
            _currentColor = Random.Range(0, _colorMap.Length);
            var x = Random.Range(0, UsefulConstant.MapSizeX);
            var y = Random.Range(0, UsefulConstant.MapSizeY);
            map.AddCollectable(x, y, _currentColor);
            guiController.UpdateColor(_colorMap[_currentColor]);
        }

        private void Update()
        {
            _deltaTime -= Time.deltaTime;
            if (!(_deltaTime <= 0)) return;
            _deltaTime = _collectableRespawnTime;
            var color = Random.Range(0, _colorMap.Length);
            var x = Random.Range(0, UsefulConstant.MapSizeX);
            var y = Random.Range(0, UsefulConstant.MapSizeY);
            map.AddCollectable(x, y, color);
        }

        /// <summary>
        /// Update Game Play State when player collect a color
        /// </summary>
        /// <param name="color"> The color Player collected </param>
        public void UpdatePlayerOnCollect(int color)
        {
            if (color == _currentColor)
            {
                _score++;
                var t = map.CollectableColors.Count;
                _currentColor = Random.Range(0, t - 1);
                
                guiController.UpdateScore(_score);
                guiController.UpdateColor(_colorMap[_currentColor]);
            }
            else
            {
                GameOver();
            }
            
            if (_score == 10)
            {
                SceneManager.LoadScene(UsefulConstant.MainMenuScene);
            }
        }
        
        /// <summary>
        /// Return Color by index
        /// </summary>
        /// <param name="index"> Index </param>
        /// <returns> Color </returns>
        public Color GetColor(int index)
        {
            return _colorMap[index];
        }

        private void GameOver()
        {
            IsLost = true;
            SceneManager.LoadScene(UsefulConstant.GameOverScene);
        }

    }
}