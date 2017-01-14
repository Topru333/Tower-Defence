using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ShootType))]
[CanEditMultipleObjects]
public class ShootTypeEditor : Editor {
    ShootType myScript;
    private ReorderableList list;

    void OnEnable () {
        myScript = (ShootType)target;
        list = new ReorderableList(serializedObject,
               serializedObject.FindProperty("Attacks"),
               true, true, true, true);
        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Tower attacks");
        };
        list.onRemoveCallback = (ReorderableList l) => {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure that you want to delete the attack?", "Yes", "No")) {
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            }
        };
    }


    public override void OnInspectorGUI () {
        EditorGUILayout.HelpBox("Add there some attacks with their damage , type of attack, elements, and effects", MessageType.None, true);
        GUILayout.Space(20);

        list.DoLayoutList();
        list.elementHeight = 160;
        

        list.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), "Note:");
            EditorGUI.PropertyField(
                new Rect(rect.x + 80, rect.y, rect.width - 90, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("note"), GUIContent.none);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 20, 60, EditorGUIUtility.singleLineHeight), "Damage:");
            EditorGUI.PropertyField(
                new Rect(rect.x + 80, rect.y + 20, rect.width - 90, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("damage"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 40, rect.width / 2 - 10, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("targetType"), GUIContent.none);
            switch (myScript.Attacks[index].attackType ) {
                case ShootType.Attack.TypeOfAttack.Fire:
                    GUI.color = Color.red;
                    break;
                case ShootType.Attack.TypeOfAttack.Poison:
                    GUI.color = Color.green;
                    break;
                case ShootType.Attack.TypeOfAttack.Electric:
                    GUI.color = Color.yellow;
                    break;
                case ShootType.Attack.TypeOfAttack.Ice:
                    GUI.color = Color.cyan;
                    break;
                case ShootType.Attack.TypeOfAttack.Physics:
                    GUI.color = Color.gray;
                    break;
            }
            EditorGUI.PropertyField(
            new Rect(rect.x + rect.width / 2, rect.y + 40, rect.width / 2 - 10, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("attackType"), GUIContent.none);
            GUI.color = Color.white;
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 60, 80, EditorGUIUtility.singleLineHeight), "Bullets delay:");
            myScript.Attacks[index].bulletsDelay = GUI.HorizontalSlider(
                new Rect(rect.x + 90, rect.y + 60, rect.width - 100, EditorGUIUtility.singleLineHeight),
                myScript.Attacks[index].bulletsDelay, 0.1f, 1f);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 80, 80, EditorGUIUtility.singleLineHeight), "Effect delay:");
            myScript.Attacks[index].effectsDisplayTime = GUI.HorizontalSlider(
                new Rect(rect.x + 90, rect.y + 80, rect.width - 100, EditorGUIUtility.singleLineHeight),
                myScript.Attacks[index].effectsDisplayTime, 0.1f, 1f);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 100, 100, EditorGUIUtility.singleLineHeight), "Random points:");
            myScript.Attacks[index].randomPointsCount = EditorGUI.IntSlider(
                new Rect(rect.x + 90, rect.y + 100, rect.width - 105, EditorGUIUtility.singleLineHeight),
                myScript.Attacks[index].randomPointsCount, 2, 10);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 120, 80, EditorGUIUtility.singleLineHeight), "Gun point:");
            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y + 120, rect.width - 105, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("gunPoint"), GUIContent.none);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 140, 80, EditorGUIUtility.singleLineHeight), "Shooting:");
            if (myScript.Attacks[index].shooting) {
                GUI.color = Color.green;
                EditorGUI.Toggle(new Rect(rect.x + 70, rect.y + 140, 50, EditorGUIUtility.singleLineHeight), true);
                GUI.color = Color.white; }
            else {
                GUI.color = Color.red;
                EditorGUI.Toggle(new Rect(rect.x + 70, rect.y + 140, 50, EditorGUIUtility.singleLineHeight), false);
                GUI.color = Color.white; }

        };
        serializedObject.ApplyModifiedProperties();
    }
}