using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TIMNaviArrowManager))]
public class TIMNaviArrowManagerEditor : Editor
{
    TIMNaviArrowManager manager;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(manager == null) manager = (TIMNaviArrowManager)target;

        if (GUILayout.Button("Update"))
        {
            manager.UpdateStep();
        }
    }
}

