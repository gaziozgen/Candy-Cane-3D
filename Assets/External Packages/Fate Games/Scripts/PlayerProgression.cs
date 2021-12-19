using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public class PlayerProgression : MonoBehaviour
    {
        /*private static PlayerProgression instance;
        public static PlayerProgression Instance { get => instance; }
        private void Awake()
        {
            if (!instance)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }*/
        public static int CurrentLevel { get => PlayerPrefs.GetInt("CurrentLevel"); set => PlayerPrefs.SetInt("CurrentLevel", value); }
        public static int MONEY { get => PlayerPrefs.GetInt("MONEY"); set => PlayerPrefs.SetInt("MONEY", value); }
        public static int COIN { get => PlayerPrefs.GetInt("COIN"); set => PlayerPrefs.SetInt("COIN", value); }
        public static int GEM { get => PlayerPrefs.GetInt("GEM"); set => PlayerPrefs.SetInt("GEM", value); }
        public static int KEY { get => PlayerPrefs.GetInt("KEY"); set => PlayerPrefs.SetInt("KEY", value); }
        public static int GOLD { get => PlayerPrefs.GetInt("GOLD"); set => PlayerPrefs.SetInt("GOLD", value); }
        public static int DIAMOND { get => PlayerPrefs.GetInt("DIAMOND"); set => PlayerPrefs.SetInt("DIAMOND", value); }
    }

}
