using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AutoBoneCombine))]
public class AutoBoneCombineButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AutoBoneCombine auto = (AutoBoneCombine)target;
        if (GUILayout.Button("CombineBone"))
        {
            auto.AutoCombine();
        }
    }
}
