using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FateGames;
public class MainLevelManager : LevelManager
{
    public static MainLevelManager Instance { get => (MainLevelManager)_Instance; }
    private Road road = null;
    private Player player = null;
    [SerializeField] private Text coinText;
    private int coin = 0;
    public Text CoinText { get => coinText; }
    public int Coin { get => coin; }
    public Road Road { get => road; }
    public Player Player { get => player; }

    private new void Awake()
    {
        base.Awake();
        road = FindObjectOfType<Road>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        coinText.text = PlayerProgression.COIN.ToString();
    }

    public override void StartLevel()
    {
        player.animator.SetTrigger("walk");
    }

    public override void FinishLevel(bool success)
    {
        if (GameManager.Instance.State != GameManager.GameState.FINISHED)
        {
            GameManager.Instance.State = GameManager.GameState.FINISHED;
            // CODE HERE ********





            // ******************
            GameManager.Instance.FinishLevel(success);
            if (success) PlayerProgression.COIN += coin;
        }
    }

    public void AddCoin(int number)
    {
        coin += number;
    }


}
