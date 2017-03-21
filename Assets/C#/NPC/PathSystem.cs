using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSystem : MonoBehaviour {
    public static PathSystem instance;

    public int Start_id;                                                            // Айди стартовой точки для спавна нпс


    private float Radius = 2;                                                       // Радиус отрисованной точки

    private static System.Random rand = new System.Random();

    private static List<Point> Points = new List<Point>();                          // Список имеющихся точек
    private static List<Edge> Edges = new List<Edge>();                             // Список имеющихся ребер

    private float timer,delay;

    /// <summary>
    /// Функция вызова волны NPC
    /// </summary>
    /// <param name="wave">структура волны(все данный о волне в ней)</param>
    /// <param name="id">Индекс вершины старта волны</param>
    static void NPCSpawn (NpcWave wave,int id) {
        //todo: Спавнит мобов в вершине...
    }
    void Awake () {
        if (instance != null) { Debug.LogError("More than 1 Path system on the level"); return; }
        instance = this;
    }
    void Update () {
        timer += Time.deltaTime;
    }

    /// <summary>
    /// Поиск следующей точки по айди
    /// </summary>
    /// <param name="id">айди точки на входе ребра</param>
    /// <returns>айди точки на выходе ребра</returns>
    public static Point PointSearch (int id) {
        List<Edge> found = new List<Edge>();                                        //список ребер что были найдены
        int thisone = -1;

        for (int i = 0; i < Edges.Count; i++) {
            if (Edges[i].ID_in == id) {
                found.Add(Edges[i]);                                                //заполняем список
            }
        }
        
        if(found.Count > 1) 
            { thisone = found[rand.Next(0,found.Count-1)].ID_out; }                 // берем рандомный из них
        else if(found.Count == 1) { thisone = found[0].ID_out; }                    // берем тот единственный что есть

        for (int i = 0; i < Points.Count; i++) {
            if (Points[i].ID == thisone) return Points[i];                          // возвращаем следующую точку
        }
        return null;                                                                // если не найдена возвращаем нулл
    }

    public void OnDrawGizmos () {
        Gizmos.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.5f);
        for (int i = 0; i < Points.Count; i++) {
            Gizmos.DrawSphere(Points[i].Position, Radius);
        }
    }

    public static void Init () {
        /* Заполняем списки вершин и ребер в ручную (Points, Edges)+ unit test */
        /* В векторе пишем X и Z, это примеры */
        Points.Add(new Point(0, new Vector2(1, 8)));
        Points.Add(new Point(1, new Vector2(4, 8)));

        Edges.Add(new Edge(0,1));

        Points.Add(new Point(2, new Vector2(4, 6)));

        Edges.Add(new Edge(1, 2));
    }
}

                                                                                    //Струкутра для ребра с переменными:айди начала и айди выхода точки
public class Edge {
    private int id_in,id_out;

    public int ID_in {
        get {
            return id_in;
        }
    }
    public int ID_out {
        get {
            return id_out;
        }
    }

    public Edge (int i, int j) {
        id_in = i;
        id_out = j;
    }
}

                                                                                      //Структура точки с переменными: айди точки и ее позиция(двухмерная так как весь путь у нас по сути на одной высоте
public class Point {
    private int id;
    private Vector2 pos;

    public int ID {
        get {
            return id;
        }
    }
    public Vector3 Position {
        get {
            return pos;
        }
    }

    public Point (int i,Vector2 j) {
        id = i;
        pos = j;
    }
}

public struct NpcWave {
}
