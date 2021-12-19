using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouceMachine : MonoBehaviour
{
    public Souce souce = Souce.Empty;
    public Material souceMaterial;

    [SerializeField] private Renderer[] renderersToFadeOut;

    public enum Souce { Empty, Chocolate, Strawberry, Banana };

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
