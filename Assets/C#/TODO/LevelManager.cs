using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TD
{
    public class LevelManager : MonoBehaviour
    {
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

        private Dictionary<string, GameObject> base_towers = new Dictionary<string, GameObject>();    // Список башен доступных для постройки
        private Dictionary<string, GameObject> base_npcs   = new Dictionary<string, GameObject>();    // Список башен доступных для постройки

        private bool pause = true;                                      // Состояние игры(pause - true/inGame - false)
        private float gameSpeed = 1f;                                   // Скорость игры

        public Level CurrentLevel=null;

        // Загружает данные о путях из потока
        public void LoadPathGraph(StreamReader sr)
        {
            List<Point> points = new List<Point>();
            List<Edge> edges = new List<Edge>();
            Utilities.ReadBlock(sr, (string _line) =>
            {
                string[] splitLine = _line.Split(' ');
                int a1;
                float a2, a3;
                if (int.TryParse(splitLine[0], out a1) && float.TryParse(splitLine[1], out a2) && float.TryParse(splitLine[2], out a3))
                {
                    points.Add(new Point(a1, new Vector2(a2, a3)));
                }
                else
                {
                    throw new IOException("Data was corrupted at vertex block!");
                }
            });
            Utilities.ReadBlock(sr, (string _line) =>
            {
                string[] splitLine = _line.Split(' ');
                int a1, a2;
                if (int.TryParse(splitLine[0], out a1) && int.TryParse(splitLine[1], out a2))
                {
                    edges.Add(new Edge(a1, a2));
                }
                else
                {
                    throw new IOException("Data was corrupted at edge block!");
                }
            });
            PathSystem.Instance.Load(points, edges);
        }

        // Загружает данные о NPC из потока
        public void LoadNPCList(StreamReader sr)
        {
                Utilities.ReadBlock(sr, (string _line) =>
                {
                    string[] strSpl = _line.Split(' ');
                    base_npcs.Add(strSpl[0], Resources.Load<GameObject>(strSpl[1]));
                }
                );
        }

        // Загружает данные о башнях из потока
        public void LoadTowerList(StreamReader sr)
        {
            Utilities.ReadBlock(sr, (string _line) =>
            {
                string[] strSpl = _line.Split(' ');
                base_towers.Add(strSpl[0], Resources.Load<GameObject>(strSpl[1]));
            }
            );
        }

        public GameObject GetTowerPrefab(string hash) { return base_towers[hash]; }
        public GameObject GetNPCPrefab(string hash) { return base_npcs[hash]; }

        // «Инициализация»
        public void Awake()
        {
            TextAsset objectList= Resources.Load<TextAsset>("objectlist");
            Stream objectListStream = new MemoryStream(objectList.bytes);
            using (StreamReader sr = new StreamReader(objectListStream))
            {
                LoadNPCList(sr);
                LoadTowerList(sr);
            }
        }

        // «Обновление»
        public void Update()
        {

        }

        // Метод загрузки уровня
        public void LevelLoad(string name)
        {
            //Scene levelScene = SceneManager.CreateScene(name);
            
            //SceneManager.SetActiveScene(levelScene);
            //GameObject ingameMenu = Resources.Load<GameObject>("Menus/InGameMenu");

            GameObject levelHolder = new GameObject("LevelHolder");
            CurrentLevel = levelHolder.AddComponent<Level>();
            CurrentLevel.level_name = name;
            TextAsset level = Resources.Load<TextAsset>("Levels/" + name);
            Stream levelStream = new MemoryStream(level.bytes);
            //Instantiate(ingameMenu);
            using (StreamReader sr = new StreamReader(levelStream))
                CurrentLevel.Load(sr);
        }

        // Изменение скорости игры
        public void GameSpeed(float speed)
        {
            gameSpeed = speed;
            Time.timeScale = gameSpeed;
        }

        // Переход в режим "пауза"(и обратно)
        public void Pause()
        {
            pause = !pause;
            if(pause)
                Time.timeScale = 0;
            else
                Time.timeScale = gameSpeed;
        }

        // Метод завершения уровня
        public void LevelEnd()
        {

        }
    }

    // Доп. структура "Волна"
    [Serializable]
    public struct NpcWave
    {
        public GameObject NPC;      // Префаб "NPC"
        public int count;           // Кол-во NPC на вызов
        public int delay;           // Задержка между вызовами
        public int pathVertexID;    // Индекс вершины из которой будет вызвана волна
    }
}