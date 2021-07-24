using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroidCrasher
{
   public enum GameState
   {
        OnPause,
        Running,
        MainMenu
   }

    public class GameCycleManager : MonoBehaviour
    {
        //в теории можно было бы описать интерфейс IConroller и реализовать его в этих контроллерах и при старте в список поиском записать,
        //но я не уверен что это точно хорошая практика

        [SerializeField]
        int _initAsteroidsCount;

        [SerializeField]
        int _currentAsteroidsCount;


        public event GameEvent GameStateChanged = delegate { };

        private GameState _currentGameState;
        public GameState CurrentGameState 
        {
            get 
            {
                return _currentGameState;
            }
            private set
            {
                _currentGameState = value;
                GameStateChanged();
                
            }
        }

        [SerializeField]
        GameObject _playerGameObject;
        [SerializeField]
        AsteroidManager _asteroidManager;
        [SerializeField]
        HealthController _playerHealthController;
        [SerializeField]
        ShipController _shipController;
        [SerializeField]
        UFOSpawner _uFOSpawner;
        [SerializeField]
        Menu _mainMenu;


        void SpawnNewAsteroids()
        {
            _currentAsteroidsCount++;
            StartCoroutine(SpawnCorutine());
        }

        IEnumerator SpawnCorutine()
        {
            yield return new WaitForSeconds(2f);
            _asteroidManager.SpawnAsteroidsInRandomPos(_currentAsteroidsCount, AsteroidType.Big);
        }
        

        public void StartNewGame()
        {
            _currentAsteroidsCount = _initAsteroidsCount;
            StopAllCoroutines();
            _playerGameObject.SetActive(true);
            Time.timeScale = 1;
            CurrentGameState = GameState.Running;
            _asteroidManager.ResetManager();
            _playerHealthController.ResetContoller();
            PointCounter.ResetPoints();
            _shipController.ResetContoller();
            _uFOSpawner.ResetSpawner();

            _asteroidManager.SpawnAsteroidsInRandomPos(_currentAsteroidsCount, AsteroidType.Big);

        }

        public void PauseGame()
        {
            CurrentGameState = GameState.OnPause;
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            CurrentGameState = GameState.Running;
            Time.timeScale = 1;
        }

        void EndGame()
        {
            _playerGameObject.SetActive(false);
            Time.timeScale = 0;
            CurrentGameState = GameState.MainMenu;
            
        }

        void Start()
        {
            CurrentGameState = GameState.MainMenu;
            _playerHealthController.Die += EndGame;
            _asteroidManager.AllAsteroidsDestroyed += SpawnNewAsteroids;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
