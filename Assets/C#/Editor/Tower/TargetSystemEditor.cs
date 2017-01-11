using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TargetSystem))]
public class TargetSystemEditor : Editor {

    void OnSceneGUI () {
        TargetSystem fow = (TargetSystem)target;
        Handles.color = Color.black;
        Handles.DrawWireArc(fow.upOfTower.position, Vector3.up,Vector3.forward,360, fow.ViewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.ViewAngle / 2);
        Vector3 viewAngleB = fow.DirFromAngle(fow.ViewAngle / 2);

        Handles.DrawLine(fow.upOfTower.position, fow.upOfTower.position + viewAngleA * fow.ViewRadius);
        Handles.DrawLine(fow.upOfTower.position, fow.upOfTower.position + viewAngleB * fow.ViewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets) {
            Handles.DrawLine(fow.upOfTower.position, visibleTarget.position);
        }
    }
}
