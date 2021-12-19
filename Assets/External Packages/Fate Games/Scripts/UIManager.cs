using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FateGames
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public Canvas UICanvas { get => GameObject.Find("UI").transform.Find("Canvas").GetComponent<Canvas>(); }
        public static UIManager Instance { get => instance; }
        [Header("Prefabs")]
        [SerializeField] private GameObject uiPrefab = null;
        [SerializeField] private GameObject uiLoadingScreenPrefab = null;
        [SerializeField] private GameObject uiCompleteScreenPrefab = null;
        [SerializeField] private GameObject uiLevelTextPrefab = null;
        [SerializeField] private GameObject uiStartTextPrefab = null;
        private void Awake()
        {
            if (!instance)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        public void CreateUILevelText()
        {
            Transform parent = UICanvas.transform;
            GameObject go = Instantiate(uiLevelTextPrefab, parent);
            TextMeshProUGUI levelText = go.GetComponentInChildren<TextMeshProUGUI>();
            levelText.text = GameManager.Instance.LevelName + " " + PlayerProgression.CurrentLevel;
        }

        public void CreateUIStartText()
        {
            Transform parent = UICanvas.transform;
            GameObject go = Instantiate(uiStartTextPrefab, parent);
            Text levelText = go.GetComponent<Text>();
            levelText.text = "TAP TO PLAY";
        }
        public void CreateUICompleteScreen(bool success)
        {
            Transform parent = UICanvas.transform;
            GameObject go = Instantiate(uiCompleteScreenPrefab, parent);
            UICompleteScreen uiCompleteScreen = go.GetComponent<UICompleteScreen>();
            uiCompleteScreen.SetScreen(success, PlayerProgression.CurrentLevel);
        }
        public UILoadingScreen CreateLoadingScreen()
        {
            UILoadingScreen uiLoadingScreen = FindObjectOfType<UILoadingScreen>();
            if (!uiLoadingScreen)
            {
                Transform parent = UICanvas.transform;
                GameObject go = Instantiate(uiLoadingScreenPrefab, parent);
                uiLoadingScreen = go.AddComponent<UILoadingScreen>();
            }
            return uiLoadingScreen;
        }
    }
}
