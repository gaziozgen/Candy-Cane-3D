using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
[CustomEditor(typeof(Road))]
public class RoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Road road = (Road)target;
        if (DrawDefaultInspector())
        {
            road.GenerateRoad();
        }
    }
}