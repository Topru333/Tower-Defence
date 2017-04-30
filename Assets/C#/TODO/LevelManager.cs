using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class LevelManager : MonoBehaviour {
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<LevelManager>();
                singleton.name = typeof(LevelManager).ToString();
            }
            return _instance;
        }
    }

    public List<GameObject> towerStore = new List<GameObject>();    // Список башен доступных для постройки
    public int pointsOnLvl = 100;                                   // Кол-во очков полученных на уровне
    public int coinsOnLvl = 100;                                    // Кол-во денег полученных на уровне
    public int mainTowerLifeCount = 10;                                      // Кол-во жизней у основной башни
    public bool pause = false;                                      // Состояние игры(pause - true/inGame - false)
    public float gameSpeed = 10f;                                   // Скорость игры
    Stack<NpcWave> waves = new Stack<NpcWave>();                    // Стек списков волн

    private static void ReadBlock (TextReader tr, Action<string> readAction) {
        tr.ReadLine();
        string line = tr.ReadLine();
        while (line != "end") {
            readAction(line);
            line = tr.ReadLine();
        }
    }

   
    // Загружает данные о путях из файла
    public void LoadPathGraph (string path) {
        List<Point> points = new List<Point>();
        List<Edge> edges = new List<Edge>();
        using (StreamReader sr = new StreamReader(path)) {
            ReadBlock(sr, (string _line) => {
                string[] splitLine = _line.Split(' ');
                int a1;
                float a2, a3;
                Debug.Log(splitLine[0] + " " + splitLine[1] + " " + splitLine[2] + " ");
                if (int.TryParse(splitLine[0], out a1) && float.TryParse(splitLine[1], out a2) && float.TryParse(splitLine[2], out a3)) {
                    points.Add(new Point(a1, new Vector2(a2, a3)));
                }
                else {
                    throw new Exception("Data was corrupted on points!");
                }

            });
            ReadBlock(sr, (string _line) => {
                string[] splitLine = _line.Split(' ');
                int a1, a2;
                if (int.TryParse(splitLine[0], out a1) && int.TryParse(splitLine[1], out a2)) {
                    edges.Add(new Edge(a1, a2));
                }
                else {
                    throw new Exception("Data was corrupted on edges! ");
                }
            });
        }
        PathSystem.Instance.Load(points,edges);
    }

    // «Инициализация»
    public void Awake () {
       
    }

    // «Обновление»
    public void Update () {

    }

    // Метод вызова волн
    public void WaveStart () {
        NpcWave NpcWave = waves.Pop();
        PathSystem.Instance.NPCSpawn(NpcWave,0);
    }

    // Изменение скорости игры
    public void GameSpeed (float speed) {

    }
    // Переход в режим "пауза"(и обратно)
    public void Pause () {
        pause = !pause;
    }

    // Метод завершения уровня
    public void LevelEnd() {

    }
}

// Доп. структура "Волна"
public struct NpcWave {
    public GameObject NPC; // Префаб "NPC"
    public int count;      // Кол-во NPC на вызов
    public int delay;      // Задержка между вызовами
    public int reward;     // Итоговый приз за волну (очки)
}
