using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;

[Serializable]
public struct EnemyData
{
    public GameObject   prefab;// префаб группы врагов
    public int          count;// количество врагов в группе
    public float        delay;// задержка между одинаковыми врагами
}

public class NPCSpawnPoint : MonoBehaviour {
    public Transform Tower;
    public List<EnemyData> enemies;
    [Range(0.01f, 10)]
    public float groupDelay = 0.05f;// задержка между группами врагов	

    private float countdownTimer=0;
    private int currentGroup = 0;
    private int enemyLeft = 0;

    void Awake()
    {
        enemyLeft = enemies[currentGroup].count;
    }

    void Update () {

        countdownTimer += Time.deltaTime;
        if(currentGroup< enemies.Count&& enemyLeft > 0)
            SpawnGroup(currentGroup);
        if (enemyLeft <= 0)
        {
            if (countdownTimer > groupDelay)
            {
                currentGroup++;
                if (currentGroup < enemies.Count)
                    enemyLeft = enemies[currentGroup].count;
                countdownTimer = 0;
            }
        }

    }
    void SpawnGroup(int i)// спавнит i-ю группу врагов.
    {

        if (countdownTimer > enemies[currentGroup].delay)
        {
            var enemy = Instantiate(enemies[currentGroup].prefab, transform);
            enemy.GetComponent<NPC_Movement>().nextWayPoint = Tower;
            countdownTimer = 0;
            enemyLeft--;
        }
        
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(NPCSpawnPoint))]
public class NPCSpawnPointEditor : Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("enemies"),
                true, true, true, true);
        list.drawElementCallback = DrawListElement;
        list.drawHeaderCallback = (Rect r)=>{ EditorGUI.LabelField(r,"Spawn Groups"); };
    }
    private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("count"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + 60, rect.y, rect.width - 60 - 200, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("prefab"), GUIContent.none);
        EditorGUI.Slider(
            new Rect(rect.x + rect.width - 200, rect.y, 200, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("delay"),0,1, GUIContent.none);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
    void OnSceneGUI()
    {
        NPCSpawnPoint spawnPoint = (NPCSpawnPoint)target;

    }
}
#endif