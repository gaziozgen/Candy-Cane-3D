using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private float length = 10;
    [SerializeField] private float width = 8;

    public float Length { get => length; }
    public float Width { get => width; }

    public void GenerateRoad()
    {
        Vector3 localScale = transform.localScale;
        localScale.z = length;
        localScale.x = width;
        transform.localScale = localScale;
    }
}
