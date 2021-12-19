using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleMachine : MonoBehaviour
{
    public Sprinkle sprinkle = Sprinkle.Heart;
    public enum Sprinkle { Heart, Star };

    [SerializeField] private Renderer[] renderersToFadeOut;

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
