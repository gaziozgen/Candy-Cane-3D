using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMachine : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private ParticleSystem spark = null;
    [SerializeField] private ParticleSystem candyCut = null;
    [SerializeField] private Renderer[] renderersToFadeOut;

    public void CutCandy(SouceMachine.Souce souce, bool sprinkleHeart, bool sprinkleStar)
    {
        spark.Play();
        candyCut.Play();
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

        FinalMachine.Instance.AddCandyMoney(money);
    }

    public void FadeOut()
    {
        for (int i = 0; i < renderersToFadeOut.Length; i++)
        {
            Color color = renderersToFadeOut[i].material.color;
            color.a = 0.5f;
            renderersToFadeOut[i].material.color = color;
        }
    }

}
