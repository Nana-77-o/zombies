using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace Perapera_Puroto
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField]
        private GameTimer _gameTimer = default;
        [SerializeField]
        private UnityEvent _startEvent = default;
        [SerializeField]
        GameObject _gameOver = default;
        [SerializeField]
        GameObject _gameClear = default;
        [SerializeField]
        private string _loadScene = "MergeScene";
        [SerializeField]
        private string _resultScene = "ResultSample";
        [SerializeField]
        private float _retryTime = 2f;
        private bool _isGameOver = false;
        private bool _isGameClear = false;
        private void Awake()
        {
            Instance = this;
        }
        private IEnumerator Start()
        {
            yield return null;
            _gameTimer.DelTimerStop += OnTimerStop;
            StartGame();
        }
        private void OnTimerStop()
        {
            if (_isGameOver)
            {
                return;
            }
            GameClear();
        }

        private void GameClear()
        {
            if (_isGameOver)
            {
                return;
            }
            _isGameOver = true;
            _isGameClear = true;
            _gameClear.SetActive(true);
            _gameTimer.StopTimer();
            StartCoroutine(LoadScene());
        }
        public void StartGame()
        {
            _gameTimer.StartTimer();
            _startEvent?.Invoke();
        }
        public void GameOver()
        {
            if (_isGameOver)
            {
                return;
            }
            _isGameOver = true;
            _gameOver.SetActive(true);
            _gameTimer.StopTimer();
            StartCoroutine(LoadScene());
        }
        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(_retryTime);
            if (_isGameClear == true)
            {
                SceneControl.ChangeTargetScene(_resultScene);
                yield break;
            }
            SceneControl.ChangeTargetScene(_loadScene);
        }
    }
}