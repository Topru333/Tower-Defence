using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(TowerManager))]
public class TowerManagerEditor : Editor {
    TowerManager myScript;
    private ReorderableList list;

    public enum Types {
        Fast_Killer,
        Tesla_Tower
    }

    //void OnEnable () {
    //    myScript = (TowerManager)target;
    //    list = new ReorderableList(serializedObject,
    //           serializedObject.FindProperty("towers"),
    //           true, true, true, true);
    //    list.drawHeaderCallback = (Rect rect) => {
    //        EditorGUI.LabelField(rect, "Towers at scene");
    //    };
    //    list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {
    //        var menu = new GenericMenu();
    //        var guids = AssetDatabase.FindAssets("", new[] { "Assets/Prefabs/Towers" });
    //        foreach (var guid in guids) {
    //            //menu.AddItem(new GUIContent("Bosses/" + Path.GetFileNameWithoutExtension(path)),
    //            false, clickHandler,
    //            new WaveCreationParams() { Type = Types.Tesla_Tower, Level  = 1 });
    //        }
    //        menu.ShowAsContext();
    //    };
    //}

    public override void OnInspectorGUI () {
        GUILayout.Space(10);
        serializedObject.Update();
        list.DoLayoutList();
        list.elementHeight = 160;


        GUILayout.Space(10);
        serializedObject.ApplyModifiedProperties();
    }
    private void clickHandler (object target) {
    }
    private struct WaveCreationParams {
        public Types Type;
        public string Level;
    }
}