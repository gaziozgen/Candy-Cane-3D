using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public class ConfettiManager : MonoBehaviour
    {
        private static ConfettiManager instance = null;
        public static ConfettiManager Instance { get => instance; }
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
        public Transform CreateConfettiBlast(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Transform confetti = ObjectPooler.Instance.SpawnFromPool("ConfettiBlast", position, Quaternion.Euler(rotation)).transform;
            confetti.localScale = scale;
            return confetti;
        }
        public Transform CreateConfettiDirectional(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Transform confetti = ObjectPooler.Instance.SpawnFromPool("ConfettiDirectional", position, Quaternion.Euler(rotation)).transform;
            confetti.localScale = scale;
            return confetti;
        }
    }
}

