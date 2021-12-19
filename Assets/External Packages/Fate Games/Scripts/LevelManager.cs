using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FateGames
{
    public abstract class LevelManager : MonoBehaviour
    {

        public static LevelManager _Instance;
        public enum LevelType { MAIN };

        protected LevelType type;
        public LevelType Type
        {
            get
            {
                return type;
            }
        }

        protected void Awake()
        {
            AvoidDuplication();
        }
        public abstract void StartLevel();
        public abstract void FinishLevel(bool success);


        private void AvoidDuplication()
        {
            if (!_Instance || _Instance.GetType() != this.GetType())
                _Instance = this;
            else
                Destroy(gameObject);
        }
    }
}