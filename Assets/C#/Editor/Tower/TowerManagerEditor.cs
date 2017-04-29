using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;

[CustomEditor(typeof(TowerManager))]
public class TowerManagerEditor : Editor {
    TowerManager myScript;
    private ReorderableList list;



    void OnEnable () {
        myScript = (TowerManager)target;
        list = new ReorderableList(serializedObject,
               serializedObject.FindProperty("towersInScene"),
               true, true, true, true);
        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Towers in scene");
        };
        list.displayAdd = false;
        list.displayRemove = false;

    }
  
    public override void OnInspectorGUI () {
        serializedObject.Update();
        
        EditorGUILayout.HelpBox("This script for work with towers in scene. First list - towers that u can use in scene. Second list - towers that right now in scene.", MessageType.None, true);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("towerStore"),true);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("In scene " + list.count + " towers");
        list.DoLayoutList();
        
        list.elementHeight = 160;
        DrawElementsOfScene(list);

        GUILayout.Space(10);
        serializedObject.ApplyModifiedProperties();
    }

    public void DrawElementsOfScene (ReorderableList List) {
        list.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            TowerInformation st = myScript.towersInScene[index].GetComponent<TowerInformation>();
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), st.name);
            EditorGUI.LabelField(new Rect(rect.x + 70, rect.y, 60, EditorGUIUtility.singleLineHeight), st.lvl.ToString() + " lvl");
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 20, 60, EditorGUIUtility.singleLineHeight), myScript.towersInScene[index].transform.position.ToString());
        };
    }
}