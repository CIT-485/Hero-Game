using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackManager))]
public class AttackManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AttackManager attackManager = (AttackManager)target;

        GUIContent content = new GUIContent("Current Attack");
        attackManager.index = EditorGUILayout.Popup(content, attackManager.index, attackManager.attackList.ToArray());

        base.OnInspectorGUI();
    }
}
