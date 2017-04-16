using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSystem : MonoBehaviour {
    public static PathSystem instance;

    public int Start_id;                                                            // Айди стартовой точки для спавна нпс


    private float Radius = 2;                                                       // Радиус отрисованной точки

    private  System.Random rand = new System.Random();

    private  List<Point> points = new List<Point>();                          // Список имеющихся точек
    private  List<Edge> edges = new List<Edge>();                             // Список имеющихся ребер

    public  List<Point> Points {
        get {
            return points;
        }
    }
    public  List<Edge> Edges {
        get {
            return edges;
        }
    }


    /// <summary>
    /// Функция вызова волны NPC
    /// </summary>
    /// <param name="wave">структура волны(все данный о волне в ней)</param>
    /// <param name="id">Индекс вершины старта волны</param>
    public  void NPCSpawn (NpcWave wave) {
        Vector2 currentPos = Vector2.zero;
        for (int i = 0; i <Points.Count; i++) {
            if (Points[i].ID == Start_id) {
                currentPos = Points[i].Position;
            }
        }
        if (currentPos.Equals(Vector2.zero)){
            return;
        }
        npc = wave.NPC;
        pos = currentPos;
        count = wave.count;
        StartCoroutine("WaveMob",wave.delay);
        
    }

    private GameObject npc;
    private Vector3 pos;
    private int count;

    IEnumerator WaveMob (float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            Instantiate(npc, pos, Quaternion.identity);
            count--;
            if (count <= 0) {
                StopCoroutine("WaveMob");
            }
        }
    }

    void Awake () {
        if (instance != null) { Debug.LogError("More than 1 Path system on the level"); return; }
        instance = this;
    }
    void Update () {

    }


    public  Edge GetNextEdge (int id_in) {
        for(int i = 0; i < edges.Count - 1; i++) {
            if(edges[i].ID_in == id_in) {
                return edges[i];
            }
        }
        return null;
    }

    
    /// <summary>
    /// Поиск следующей точки по айди
    /// </summary>
    /// <param name="id">айди точки на входе ребра</param>
    /// <returns>айди точки на выходе ребра</returns>
    public  Point NextPointSearch (int id) {
        List<Edge> found = new List<Edge>();                                        //список ребер что были найдены
        int thisone = -1;

        for (int i = 0; i < edges.Count; i++) {
            if (edges[i].ID_in == id) {
                found.Add(edges[i]);                                                //заполняем список
            }
        }
        
        if(found.Count > 1) 
            { thisone = found[rand.Next(0,found.Count-1)].ID_out; }                 // берем рандомный из них
        else if(found.Count == 1) { thisone = found[0].ID_out; }                    // берем тот единственный что есть

        for (int i = 0; i < points.Count; i++) {
            if (points[i].ID == thisone) return points[i];                          // возвращаем следующую точку
        }
        return null;                                                                // если не найдена возвращаем нулл
    }

    public void OnDrawGizmos () {
        Gizmos.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.5f);
        for (int i = 0; i < points.Count; i++) {
            Gizmos.DrawSphere(points[i].Position, Radius);
        }
    }

    public void Init () {
        /* Заполняем списки вершин и ребер в ручную (Points, Edges)+ unit test */
        /* В векторе пишем X и Z, это примеры */
        points.Add(new Point(0, new Vector2(1, 8)));
        points.Add(new Point(1, new Vector2(4, 8)));

        edges.Add(new Edge(0,1));

        points.Add(new Point(2, new Vector2(4, 6)));

        edges.Add(new Edge(1, 2));
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

    #region Получение точки из ребра
    public Vector2 GetPointIn (List<Point> points) {
        for(int i = 0;i < points.Count; i++) {
            if(points[i].ID == id_in) {
                return points[i].Position;
            }
        }
        return Vector2.zero; // Если не найдет вернет нулевой вектор!
    }

    public Vector2 GetPointOut (List<Point> points) {
        for (int i = 0; i < points.Count; i++) {
            if (points[i].ID == id_out) {
                return points[i].Position;
            }
        }
        return Vector2.zero; // Если не найдет вернет нулевой вектор!
    }
    #endregion
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
    public Vector2 Position {
        get {
            return pos;
        }
    }

    public Point (int i,Vector2 j) {
        id = i;
        pos = j;
    }
}


