using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(TowerInformation))]
[CanEditMultipleObjects]
public class TowerInformationEditor : Editor {
    TowerInformation myScript;

    void OnEnable () {
        myScript = (TowerInformation)target;
    }

    public override void OnInspectorGUI () {
        serializedObject.Update();
        EditorGUILayout.HelpBox("Here will be full information about tower.", MessageType.None, true);
        GUILayout.Space(10);
        myScript.Icon = (Sprite)EditorGUILayout.ObjectField("Tower icon :", myScript.Icon, typeof(Sprite), false);
        GUILayout.Space(10);
        myScript.Name = EditorGUILayout.TextField("Name: ", myScript.Name);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("costForBuy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sellCoefficient"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lvlСoefficient"));
        EditorGUILayout.LabelField("Information about tower :");
        myScript.Information = EditorGUILayout.TextField("", myScript.Information, GUILayout.Height(30));
        GUILayout.Space(10);
        EditorGUILayout.LabelField(myScript.lvl.ToString() + " lvl");
        EditorGUILayout.LabelField("Cost for sell: " + myScript.costForSell.ToString());
        EditorGUILayout.LabelField("Cost for lvlup: " + myScript.costForLvlUp.ToString());
        EditorGUILayout.HelpBox("((costForBuy + (costForBuy / 2 + lvlСoefficient * (lvl - 1)))/100) * sellCoefficient", MessageType.None, true);
        EditorGUILayout.HelpBox("costForLvlUp = costForBuy/2 + lvlСoefficient * lvl", MessageType.None, true);
        GUILayout.Space(10);
        serializedObject.ApplyModifiedProperties();
    }
}
