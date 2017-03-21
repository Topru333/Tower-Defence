using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(TargetSystem))]
[CanEditMultipleObjects]
public class TargetSystemEditor : Editor {


    public string[] options = new string[] { "Rotate", "NonRotate" };
    public int index = 0;
    SerializedProperty rotate;
    SerializedProperty rotate_y;
    SerializedProperty rotate_x;
    SerializedProperty centerGun_x;
    SerializedProperty upOf_tower;
    SerializedProperty view_radius;
    SerializedProperty view_angle;
    SerializedProperty damping_rotation;
    SerializedProperty target_mask;
    SerializedProperty obstacle_mask;
    SerializedProperty current_target;

    private bool Rotate;

    TargetSystem myScript;

    void OnEnable () {
        myScript = (TargetSystem)target;
        rotate = serializedObject.FindProperty("Rotate");
        rotate_y = serializedObject.FindProperty("rotateY");
        rotate_x = serializedObject.FindProperty("rotateX");
        centerGun_x = serializedObject.FindProperty("centerGunX");
        upOf_tower = serializedObject.FindProperty("upOfTower");
        view_radius = serializedObject.FindProperty("ViewRadius");
        view_angle = serializedObject.FindProperty("ViewAngle");
        damping_rotation = serializedObject.FindProperty("dampingRotation");
        target_mask = serializedObject.FindProperty("targetsMask");
        obstacle_mask = serializedObject.FindProperty("wallMask");
        current_target = serializedObject.FindProperty("currentTarget");
    }
    void OnSceneGUI () {
        
        Handles.color = Color.black;
        Handles.DrawWireArc(myScript.upOfTower.position, Vector3.up, Vector3.forward, 360, myScript.ViewRadius);
        Vector3 viewAngleA = myScript.DirFromAngle(-myScript.ViewAngle / 2);
        Vector3 viewAngleB = myScript.DirFromAngle(myScript.ViewAngle / 2);

        Handles.DrawLine(myScript.upOfTower.position, myScript.upOfTower.position + viewAngleA * myScript.ViewRadius);
        Handles.DrawLine(myScript.upOfTower.position, myScript.upOfTower.position + viewAngleB * myScript.ViewRadius);


    }

    public override void OnInspectorGUI () {

        serializedObject.Update();
        EditorGUILayout.HelpBox("This script created for rotate tower and search a target. Choice system of rotating a bit down.", MessageType.None, true);
        
        EditorGUILayout.PropertyField(rotate);
        EditorGUI.BeginDisabledGroup(myScript.Rotate == false);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(rotate_y);
        EditorGUILayout.PropertyField(rotate_x);
        EditorGUILayout.PropertyField(centerGun_x);
        EditorGUILayout.PropertyField(upOf_tower);
        EditorGUI.EndDisabledGroup();
        if (myScript.rotateX) {
            myScript.rotateY = true;
        }
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(view_radius);
        EditorGUILayout.PropertyField(view_angle);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(damping_rotation);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(target_mask);
        EditorGUILayout.PropertyField(obstacle_mask);
        GUILayout.Space(10);
        
        if(myScript.currentTarget != null) {
            EditorGUILayout.PropertyField(current_target);
            EditorGUILayout.LabelField("Position:", myScript.currentTarget.position.ToString());
        }

        serializedObject.ApplyModifiedProperties();
    }
}
