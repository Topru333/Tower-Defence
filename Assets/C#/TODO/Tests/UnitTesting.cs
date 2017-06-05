using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using TD;
using System.IO;

namespace TD.Tests
{
    public class UnitTesting
    {

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

        [Test]
        public void TowerUnitTest()
        {

        }




        [Test]
        public void UtilitiesUnitTest1 () { //Тест на отсутствие элементов списка
            TextAsset objectList = Resources.Load<TextAsset>("test1");
            Stream objectListStream = new MemoryStream(objectList.bytes);
            using (StreamReader sr = new StreamReader(objectListStream))
            {
                Utilities.ReadBlock(sr, (string _line) => {
                    string[] strSpl = _line.Split(' ');
                    Assert.AreEqual(strSpl.Length,0);
                }
                    );

            };
        }

        [Test]
        public void UtilitiesUnitTest2 () { // Тест на элементы списка
            TextAsset objectList = Resources.Load<TextAsset>("test2");
            Stream objectListStream = new MemoryStream(objectList.bytes);
            using (StreamReader sr = new StreamReader(objectListStream)) {
                Utilities.ReadBlock(sr, (string _line) => {
                    string[] strSpl = _line.Split(' ');
                    Assert.AreEqual(strSpl.Length, 2);
                }
                    );

            };
        }

        [Test]
        public void UtilitiesUnitTest3 () { // Тест на элементы списка
            Assert.Throws<NullReferenceException>(() => {
                TextAsset objectList = Resources.Load<TextAsset>("test3");

            }, "Файл не найден");
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
                wave.NPC   = new GameObject("TestNPC");
                wave.pathVertexID = 0;
                ps.NPCSpawn(wave);

            }, "Метод NPCSpawn(wave,id) пропустил некорректно заполненную структуру NpcWave, не выполненно условие NpcWave.count>0");

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                PathSystem_testLoadMethod(ps);

                NpcWave wave;
                wave.count = 1;
                wave.delay = -1;
                wave.NPC   = new GameObject("TestNPC");
                wave.pathVertexID = 0;
                ps.NPCSpawn(wave);

            }, "Метод NPCSpawn(wave,id) пропустил некорректно заполненную структуру NpcWave, не выполненно условие NpcWave.delay>0");

            Assert.Throws<ArgumentNullException>(() =>
            {
                PathSystem_testLoadMethod(ps);

                NpcWave wave;
                wave.count = 1;
                wave.delay = 1;
                wave.NPC   = null;
                wave.pathVertexID = 0;
                ps.NPCSpawn(wave);

            }, "Метод NPCSpawn(wave,id) пропустил некорректно заполненную структуру NpcWave, не выполненно условие NpcWave.NPC!=null");

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                PathSystem_testLoadMethod(ps);

                NpcWave wave;
                wave.count = 1;
                wave.delay = 1;
                wave.NPC   = new GameObject("TestNPC");
                wave.pathVertexID = 0;
                ps.NPCSpawn(wave);

            }, "Метод NPCSpawn(wave,id) пропустил некорректно заполненную структуру NpcWave, не выполненно условие NpcWave.reward>0");

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                PathSystem_testLoadMethod(ps);

                NpcWave wave;
                wave.count = 1;
                wave.delay = 1;
                wave.NPC   = new GameObject("TestNPC");
                wave.pathVertexID = -1;
                ps.NPCSpawn(wave);
            }, "Метод NPCSpawn(wave,id) пропустил некорректный параметр id");

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                PathSystem_testLoadMethod(ps);

                NpcWave wave;
                wave.count = 1;
                wave.delay = 1;
                wave.NPC   = new GameObject("TestNPC");
                wave.pathVertexID = 4;
                ps.NPCSpawn(wave);
            }, "Метод NPCSpawn(wave,id) вылетел с ошибкой");
        }

        private static void PathSystem_LoadMethodTest(PathSystem ps)
        {
            Assert.Throws<ArgumentNullException>(() => { ps.Load(null, null); }, "Метод Load(points,edges) пропустил элементы равные null");

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                List<Point> pointList = new List<Point>();
                List<Edge>  edgeList  = new List<Edge>();
                ps.Load(pointList, edgeList);
            }, "Метод Load(points,edges) пропустил элементы с некорректными размерностями");

            Assert.DoesNotThrow(() =>
            {
                PathSystem_testLoadMethod(ps);
            }, "Метод Load(points,edges) выбросил исключение");
        }

        public static void PathSystem_testLoadMethod(PathSystem ps)
        {
            List<Point> pointList;
            List<Edge> edgeList;
            PathSystem_InitPathGraph(out pointList, out edgeList);

            ps.Load(pointList, edgeList);
        }

        private static void PathSystem_InitPathGraph(out List<Point> pointList, out List<Edge> edgeList)
        {
            pointList = new List<Point>();
            edgeList  = new List<Edge>();
            pointList.Add(new Point(1, new Vector2(10, 80)));
            pointList.Add(new Point(2, new Vector2(40, 80)));

            edgeList.Add (new Edge(1, 2));
            pointList.Add(new Point(3, new Vector2(40, 60)));
            edgeList.Add (new Edge(2, 3));
        }
    }
}
