using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathSystem : MonoBehaviour {
    private static PathSystem _instance;
    public static PathSystem Instance {
        get
        {
            if(_instance == null)
            {
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<PathSystem>();
                singleton.name = typeof(PathSystem).ToString();
            }
            return _instance;
        }
    }

    public int Start_id;                                                            // Айди стартовой точки для спавна нпс


    private float Radius = 0.5f;                                                       // Радиус отрисованной точки

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

    public void Load(List<Point> _points, List<Edge> _edges)
    {
#if DEBUG
        if (_points == null || _edges == null)
            throw new ArgumentNullException("_points,_edges","PathSystem.Load: параметры к методу пустые.");
        if (_points.Count<2 || _edges.Count<1)
            throw new ArgumentOutOfRangeException("_points.Count,_edges.Count", "PathSystem.Load: параметры имеют слишком маленькое количество элементов.");
#endif
        points = _points;
        edges = _edges;
    }

    /// <summary>
    /// Функция вызова волны NPC
    /// </summary>
    /// <param name="wave">структура волны(все данный о волне в ней)</param>
    /// <param name="id">Индекс вершины старта волны</param>
    public void NPCSpawn (NpcWave wave, int id) {
#if DEBUG
        Debug.LogFormat("{0},{1},{2},{3}", wave.count, wave.delay, wave.reward,id);
        if (wave.NPC == null) throw new ArgumentNullException("NPCSpawn - wave isn't correct - null");
        if ( wave.count <= 0 || wave.delay <= 0 || wave.reward <= 0 || id <= 0|| id >= Points.Count) throw new ArgumentOutOfRangeException("NPCSpawn - wave isn't correct");
#endif
        var edgeList = Instance.Edges.FindAll((Edge e) => { return e.ID_in == id; });
        if (edgeList!=null && edgeList.Count > 0)
        {
            int edgeID= UnityEngine.Random.Range(0, edgeList.Count);
            Debug.Log(edgeList.Count);
            StartCoroutine("WaveMob", new NPCWaveInstanceInfo { _edge = edgeList[edgeID], _wave = wave });
        }
    }
    private class NPCWaveInstanceInfo
    {
        public Edge _edge;
        public NpcWave _wave;
    }

    IEnumerator WaveMob (NPCWaveInstanceInfo waveInfo) {
        while (true) {
            yield return new WaitForSeconds(waveInfo._wave.delay);
            var instancePos = Points[waveInfo._edge.ID_in-1].Position;
            var npc = Instantiate(waveInfo._wave.NPC, new Vector3(instancePos.x, 0, instancePos.y), Quaternion.identity);
            npc.GetComponent<NPC>().SetMovementEdge(waveInfo._edge);
            waveInfo._wave.count--;
            if (waveInfo._wave.count <= 0) {
                StopCoroutine("WaveMob");
            }
        }
    }

    void Awake () {

    }
    void Update () {

    }

    // Возвращает следующее ребро
    public  Edge GetNextEdge (Edge lastEdge) {
#if DEBUG
        if (lastEdge == null) throw new ArgumentNullException("GetNewEdge - null");
        if (lastEdge.ID_in <= 0 || lastEdge.ID_out <= 0) throw new ArgumentOutOfRangeException("GetNewEdge - edge isn't correct");
#endif
        var foundEdges = edges.FindAll((Edge a) => { return a.ID_in == lastEdge.ID_out; });
        if (foundEdges != null && foundEdges.Count > 0)
        {
            int edgeID = UnityEngine.Random.Range(0, foundEdges.Count);
            return foundEdges[edgeID];
        }
        return null;
    }

    
    /// <summary>
    /// Поиск следующей точки по айди
    /// </summary>
    /// <param name="id">айди точки на входе ребра</param>
    /// <returns>айди точки на выходе ребра</returns>
    public  Point NextPointSearch (int id)
    {
#if DEBUG
        if (id <= 0) throw new ArgumentOutOfRangeException("NextPointSearch - id isn't correct");
#endif
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
            Gizmos.DrawSphere(new Vector3( points[i].Position.x,0, points[i].Position.y), Radius);
        }
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


