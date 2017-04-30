using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;

public class NPCPlayModeTest {

	[Test]
	public void PathSystemUnitTest()
    {
        var ps = PathSystem.Instance;

        Assert.IsNotNull(ps, "PathSystem: невозможно получить экземпляр объекта");

        PathSystem_LoadMethodTest(ps);
        PathSystem_NPCSpawnMethodTest(ps);
        PathSystem_GetNextEdgeMethodTest(ps);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            ps.NextPointSearch(-1);

        }, "Метод NextPointSearch(id) пропустил отрицательные индексы");
        
    }

    [Test]
    public void NPCUnitTest()
    {

    }

    private static void PathSystem_GetNextEdgeMethodTest(PathSystem ps)
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            ps.GetNextEdge(null);

        }, "Метод GetNextEdge(edge) пропустил null");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            ps.GetNextEdge(new Edge(-1, -1));

        }, "Метод GetNextEdge(edge) пропустил edge с отрицательными индексами");
    }

    private static void PathSystem_NPCSpawnMethodTest(PathSystem ps)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            NpcWave wave;
            wave.count = -1;
            wave.delay = 1;
            wave.NPC = new GameObject("TestNPC");
            wave.reward = 1;
            ps.NPCSpawn(wave, 0);

        }, "Метод NPCSpawn(wave,id) пропустил некорректно заполненную структуру NpcWave, не выполненно условие NpcWave.count>0");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            NpcWave wave;
            wave.count = 1;
            wave.delay = -1;
            wave.NPC = new GameObject("TestNPC");
            wave.reward = 1;
            ps.NPCSpawn(wave, 0);

        }, "Метод NPCSpawn(wave,id) пропустил некорректно заполненную структуру NpcWave, не выполненно условие NpcWave.delay>0");

        Assert.Throws<ArgumentNullException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            NpcWave wave;
            wave.count = 1;
            wave.delay = 1;
            wave.NPC = null;
            wave.reward = 1;
            ps.NPCSpawn(wave, 0);

        }, "Метод NPCSpawn(wave,id) пропустил некорректно заполненную структуру NpcWave, не выполненно условие NpcWave.NPC!=null");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            NpcWave wave;
            wave.count = 1;
            wave.delay = 1;
            wave.NPC = new GameObject("TestNPC");
            wave.reward = -100;
            ps.NPCSpawn(wave, 0);

        }, "Метод NPCSpawn(wave,id) пропустил некорректно заполненную структуру NpcWave, не выполненно условие NpcWave.reward>0");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            NpcWave wave;
            wave.count = 1;
            wave.delay = 1;
            wave.NPC = new GameObject("TestNPC");
            wave.reward = 100;
            ps.NPCSpawn(wave, -1);
        }, "Метод NPCSpawn(wave,id) пропустил некорректный параметр id");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            PathSystem_testLoadMethod(ps);

            NpcWave wave;
            wave.count = 1;
            wave.delay = 1;
            wave.NPC = new GameObject("TestNPC");
            wave.reward = 100;
            ps.NPCSpawn(wave, 4);
        }, "Метод NPCSpawn(wave,id) пропустил некорректный параметр id");
    }

    private static void PathSystem_LoadMethodTest(PathSystem ps)
    {
        Assert.Throws<ArgumentNullException>(() => { ps.Load(null, null); }, "Метод Load(points,edges) пропустил элементы равные null");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            List<Point> pointList = new List<Point>();
            List<Edge> edgeList = new List<Edge>();
            ps.Load(pointList, edgeList);
        }, "Метод Load(points,edges) пропустил элементы с некорректными размерностями");

        Assert.DoesNotThrow(() =>
        {
            PathSystem_testLoadMethod(ps);
        }, "Метод Load(points,edges) выбросил исключение");
    }

    private static void PathSystem_testLoadMethod(PathSystem ps)
    {
        List<Point> pointList;
        List<Edge> edgeList;
        PathSystem_InitPathGraph(out pointList, out edgeList);

        ps.Load(pointList, edgeList);
    }

    private static void PathSystem_InitPathGraph(out List<Point> pointList, out List<Edge> edgeList)
    {
        pointList = new List<Point>();
        edgeList = new List<Edge>();
        pointList.Add(new Point(0, new Vector2(1, 8)));
        pointList.Add(new Point(1, new Vector2(4, 8)));

        edgeList.Add(new Edge(0, 1));

        pointList.Add(new Point(2, new Vector2(4, 6)));

        edgeList.Add(new Edge(1, 2));
    }
}
