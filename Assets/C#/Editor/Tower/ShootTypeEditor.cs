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
    private int r;

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

    void OnSceneGUI () {
        foreach (Attack a in myScript.Attacks) {
            if (a.targetType == Attack.TypeOfTargeting.ManyTargets && (a.attackType == Attack.TypeOfAttack.Electric)) {
                Handles.color = Color.yellow;
                Handles.DrawWireArc(myScript.transform.position, Vector3.up, Vector3.forward, 360, a.disToNextTarget);
            }
        }
    }
    public override void OnInspectorGUI () {
        serializedObject.Update();
        EditorGUILayout.HelpBox("Add there some attacks with their damage , type of attack, elements, and effects", MessageType.None, true);
        EditorGUILayout.HelpBox("Only 1 attack for 1 gunPoint in tower!", MessageType.Warning, true);
        GUILayout.Space(20);
        DrawListAttacks();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawListAttacks () {
        list.DoLayoutList();
        list.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            r = 0;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y + r, 60, EditorGUIUtility.singleLineHeight), "Note:");
            EditorGUI.PropertyField(
                new Rect(rect.x + 80, rect.y + r, rect.width - 90, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("note"), GUIContent.none);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 20 + r, 60, EditorGUIUtility.singleLineHeight), "Damage:");
            EditorGUI.PropertyField(
                new Rect(rect.x + 80, rect.y + 20 + r, rect.width - 90, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("damage"), GUIContent.none);
            if (myScript.Attacks[index].targetType == Attack.TypeOfTargeting.ManyTargets && (myScript.Attacks[index].attackType == Attack.TypeOfAttack.Electric)) {
                EditorGUI.LabelField(new Rect(rect.x, rect.y + 40 + r, 60, EditorGUIUtility.singleLineHeight), "Count:");
                EditorGUI.PropertyField(
                    new Rect(rect.x + 80, rect.y + 40 + r, rect.width - 90, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("countOfTargets"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x, rect.y + 60 + r, 173, EditorGUIUtility.singleLineHeight), "Distance(max) to next target:");
                EditorGUI.PropertyField(
                    new Rect(rect.x + 172, rect.y + 60 + r, rect.width - 182, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("disToNextTarget"), GUIContent.none);
                r += 40;
            }
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 40 + r, rect.width / 2 - 10, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("targetType"), GUIContent.none);
            switch (myScript.Attacks[index].attackType) {
                case Attack.TypeOfAttack.Fire:
                    GUI.color = Color.red;
                    break;
                case Attack.TypeOfAttack.Poison:
                    GUI.color = Color.green;
                    break;
                case Attack.TypeOfAttack.Electric:
                    GUI.color = Color.yellow;
                    break;
                case Attack.TypeOfAttack.Ice:
                    GUI.color = Color.cyan;
                    break;
                case Attack.TypeOfAttack.Physics:
                    GUI.color = Color.gray;
                    break;
            }
            EditorGUI.PropertyField(
            new Rect(rect.x + rect.width / 2, rect.y + r + 40, rect.width / 2 - 10, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("attackType"), GUIContent.none);
            GUI.color = Color.white;
            EditorGUI.LabelField(new Rect(rect.x, rect.y + r + 60, 80, EditorGUIUtility.singleLineHeight), "Bullets delay:");
            myScript.Attacks[index].bulletsDelay = GUI.HorizontalSlider(
                new Rect(rect.x + 90, rect.y + r + 60, rect.width - 100, EditorGUIUtility.singleLineHeight),
                myScript.Attacks[index].bulletsDelay, 0.1f, 1f);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + r + 80, 80, EditorGUIUtility.singleLineHeight), "Effect delay:");
            myScript.Attacks[index].effectsDisplayTime = GUI.HorizontalSlider(
                new Rect(rect.x + 90, rect.y + 80 + r, rect.width - 100, EditorGUIUtility.singleLineHeight),
                myScript.Attacks[index].effectsDisplayTime, 0.1f, 1f);
            list.elementHeight = 120 + r;
        };
    }
}