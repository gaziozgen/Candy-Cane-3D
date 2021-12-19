using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Facebook.Unity;
using GameAnalyticsSDK;

namespace FateGames
{
    public class GameManager : MonoBehaviour
    {
        #region Properties
        private int levelCount { get => UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1; }
        private bool locked = false;


        public GameState State = GameState.LOADING_SCREEN;
        private static GameManager instance;
        private UIStartText uiStartText = null;
        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            if (!instance)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            GameAnalytics.Initialize();
            if (FB.IsInitialized)
                FB.ActivateApp();
            else
            {
                FB.Init(() =>
                {
                    FB.ActivateApp();
                    AdjustCurrentLevel();
                });
            }
        }
        private void Update()
        {
            if (!locked)
                CheckInput();
        }

        #endregion

        #region Singleton
        public static GameManager Instance
        {
            get
            {
                GameManager instance = GameManager.instance;
                if (!instance)
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
                return instance;
            }
        }

        #endregion

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.S))
                TakeScreenshot();
            if (Input.GetKeyDown(KeyCode.X) && State == GameState.IN_GAME)
                FinishLevel(true);
            else if (Input.GetKeyDown(KeyCode.C) && State == GameState.IN_GAME)
                FinishLevel(false);
            else if (Input.GetMouseButtonDown(0) && State == GameState.START_SCREEN)
                StartLevel();
        }
        private void TakeScreenshot()
        {
            string folderPath = Directory.GetCurrentDirectory() + "/Screenshots/";

            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            var screenshotName =
                                    "Screenshot_" +
                                    System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                                    ".png";
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName));
            Debug.Log(folderPath + screenshotName);
        }

        #region Level management



        [SerializeField] private string levelName = "Level";
        public string LevelName { get => levelName; }

        private void AdjustCurrentLevel()
        {
            if (PlayerProgression.CurrentLevel == 0)
                PlayerProgression.CurrentLevel = 1;
            int targetLevel = PlayerProgression.CurrentLevel % levelCount;
            if (targetLevel == 0)
                targetLevel = levelCount;
            if (SceneManager.GetActiveScene().buildIndex == 0) // no level is loaded
                LoadLevel(targetLevel);
        }

        public void LoadCurrentLevel()
        {
            if (PlayerProgression.CurrentLevel == 0)
                PlayerProgression.CurrentLevel = 1;
            int targetLevel = PlayerProgression.CurrentLevel % levelCount;
            if (targetLevel == 0)
                targetLevel = levelCount;
            LoadLevel(targetLevel);
        }
        public void LoadLevel(int level)
        {
            LeanTween.cancelAll();
            InputManager.Clear();
            StartCoroutine(LoadLevelAsynchronously(level));
        }

        private IEnumerator LoadLevelAsynchronously(int level)
        {
            locked = true;
            UILoadingScreen loadingScreen = UIManager.Instance.CreateLoadingScreen();
            AsyncOperation operation = SceneManager.LoadSceneAsync(level);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                yield return null;
            }
            if (loadingScreen)
                loadingScreen.Hide();
            UIManager.Instance.CreateUILevelText();
            UIManager.Instance.CreateUIStartText();
            locked = false;
            State = GameState.START_SCREEN;
        }

        public void StartLevel()
        {
            print(levelName + " " + PlayerProgression.CurrentLevel + " started.");
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level" + PlayerProgression.CurrentLevel);
            State = GameState.IN_GAME;
            if (UIStartText.Instance)
                UIStartText.Instance.Hide();
            LevelManager._Instance.StartLevel();
        }
        public void FinishLevel(bool success)
        {
            GameAnalytics.NewProgressionEvent(success ? GAProgressionStatus.Complete : GAProgressionStatus.Fail, "level" + PlayerProgression.CurrentLevel);
            State = GameState.COMPLETE_SCREEN;
            UIManager.Instance.CreateUICompleteScreen(success);
            if (success)
            {
                PlayerProgression.CurrentLevel += 1;
                AdjustCurrentLevel();
            }
        }
        #endregion

        #region Enumerators
        public enum GameState { LOADING_SCREEN, START_SCREEN, IN_GAME, PAUSE_SCREEN, FINISHED, COMPLETE_SCREEN }
        #endregion
    }
}