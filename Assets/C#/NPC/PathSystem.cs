using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSystem : MonoBehaviour {
    private System.Random rand = new System.Random();

    private List<Point> Points = new List<Point>();                                // Список имеющихся точек
    private List<Edge> Edges = new List<Edge>();                                   // Список имеющихся ребер


    /// <summary>
    /// Поиск следующей точки по айди
    /// </summary>
    /// <param name="id">айди точки на входе ребра</param>
    /// <returns>айди точки на выходе ребра</returns>
    private Point PointSearch (int id) {
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
        return new Point(0,new Vector2(0,0));                                       // если не найдена возвращаем нуливую позицию
    }

}

                                                                                    //Струкутра для ребра с переменными:айди начала и айди выхода точки
public struct Edge {
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
public struct Point {
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

