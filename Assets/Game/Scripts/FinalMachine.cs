using System.Collections;
using System.Collections.Generic;
using FateGames;
using UnityEngine;

public class FinalMachine : MonoBehaviour
{
    public static FinalMachine Instance;

    [SerializeField] private TMPro.TextMeshProUGUI screen = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private Animator weightAnimator = null;
    [SerializeField] private Transform pistonToDown = null;
    [SerializeField] private Transform target = null;
    [SerializeField] private Transform weight = null;
    [SerializeField] private ParticleSystem spark = null;
    [SerializeField] private int targetWidth = 4;
    [SerializeField] private int targetLength = 4;

    private float pistonTarget;
    private int money = 0;
    private int counter = 0;

    private void Awake()
    {
        Instance = this;
        screen.text = "0 $";
        pistonTarget = pistonToDown.localPosition.z;
    }

    private void Update()
    {
        /*Vector3 target = Vector3.zero;
        target.z = pistonTarget;
        pistonToDown.localPosition = Vector3.Lerp(pistonToDown.localPosition, target, Time.deltaTime * 10);*/
    }

    private Vector3 GetNextPosition()
    {
        int x = counter % targetWidth;
        int y = counter / (targetWidth * targetLength);
        int z = (counter % (targetWidth * targetLength)) / targetWidth;

        counter++;
        return target.position + new Vector3(x - (targetWidth / 2f) + 0.5f, y + 0.2f, (targetLength / 2f) - z - 1);
    }

    public void PlaceCandyBit(Transform candyBit)
    {
        candyBit.LeanMove(GetNextPosition(), 0.2f);
        candyBit.LeanRotate(Vector3.forward, 0.2f).setOnComplete(() =>
        {
            candyBit.parent = target;
            //WeightDown();
        });
    }

    private void UpdateUI()
    {
        screen.text = money + " $";
    }

    public void AddCandyMoney(int money)
    {
        this.money += money;
        MainLevelManager.Instance.AddCoin(money);
        UpdateUI();
    }

    private void WeightDown()
    {
        if (pistonTarget > 0)
            pistonTarget -= 0.2f;
    }

    public void CandyToMoney(SouceMachine.Souce souce, bool sprinkleHeart, bool sprinkleStar)
    {
        spark.Play();
        animator.SetTrigger("cut");
        int money = 1;

        if (souce != SouceMachine.Souce.Empty)
        {
            money += 1;
        }

        if (sprinkleHeart)
        {
            money += 1;
        }

        if (sprinkleStar)
        {
            money += 1;
        }

        AddCandyMoney(money);
    }

    public void SaveCandyBit(Transform candyBit)
    {
        candyBit.position = GetNextPosition();
        candyBit.parent = target;
    }

    public void FinalShow()
    {
        FocusCamera();
        LeanTween.delayedCall(2f, () =>
        {
            weightAnimator.SetTrigger("Cover");
            LeanTween.delayedCall(2f, () =>
            {
                MainLevelManager.Instance.FinishLevel(true);
            });
        });
    }

    public void FocusCamera()
    {
        Vector3 pos = new Vector3(0, weight.position.y + 16, weight.position.z -16);
        Camera.main.transform.LeanMove(pos, 1f);
        Camera.main.transform.LeanRotateX(30f, 1f);
    }

}
