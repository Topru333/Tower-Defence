using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace TD
{
    public class Level : MonoBehaviour
    {
        private int experience = 0;                               // Кол-во очков полученных на уровне
        private int money = 100;                                    // Кол-во денег полученных на уровне
        private int mainTowerLifeCount = 10;                        // Кол-во жизней у основной башни

        private Stack<NpcWave>      waves           = new Stack<NpcWave>();     // Стек списков волн
        private List<GameObject>    avaliableTowers = new List<GameObject>();   // Список доступных для постройки башен на уровне 

        private float timer = 0;
        private bool waveStarted = false;
        private int currentWaveNPC_counter=0;
        private bool levelEnded=false;
        public string level_name;

        public void Load(StreamReader sr)
        {
            //TODO: Добавить комментарии
            string line;
            line = sr.ReadLine();
            if (line != "leveldescblock")
                throw new IOException("Не найден блок описания уровня, файл уровня поврежден");

            Utilities.ReadBlock(sr,(string l)=> { avaliableTowers.Add(LevelManager.Instance.GetTowerPrefab(l)); });

            line = sr.ReadLine();
            if(!int.TryParse(line,out money))
                throw new IOException("Невозможно считать начальное количество денег, файл уровня поврежден.");

            line = sr.ReadLine();
            if (!int.TryParse(line, out mainTowerLifeCount))
                throw new IOException("Невозможно считать начальное количество жизней главной башни, файл уровня поврежден.");

            Utilities.ReadBlock(sr, (string l) => 
            {
                string[] splitStr = l.Split(' ');
                NpcWave wave;
                wave.NPC = LevelManager.Instance.GetNPCPrefab(splitStr[0]);
                if(wave.NPC==null)
                    throw new IOException(string.Format("Невозможно найти NPC с именем {0}, он отсутствует в списке доступных NPC.", splitStr[0]));
                if (!int.TryParse(splitStr[1], out wave.count))
                    throw new IOException("Невозможно считать информацию о волне NPC, ошибка при парсинге количества волн.");
                if (!int.TryParse(splitStr[2], out wave.delay))
                    throw new IOException("Невозможно считать информацию о волне NPC, ошибка при парсинге задержки между NPC.");
                if (!int.TryParse(splitStr[3], out wave.pathVertexID))
                    throw new IOException("Невозможно считать информацию о волне NPC, ошибка при парсинге вершины для системы путей.");
                waves.Push(wave);
            });

            line = sr.ReadLine();
            sr.ReadLine();
            TowerGridSystem.Instance.LoadData(sr);
            sr.ReadLine();
            sr.ReadLine();
            LevelManager.Instance.LoadPathGraph(sr);
            sr.ReadLine();
        }

        public int GetMainTowerLifeCount()
        {
            return mainTowerLifeCount;
        }

        private void Update()
        {
            var ingameUI = IngameUI.Instance;
            if (waves.Count > 0)
            {
                if (waveStarted && timer <= 0)
                {
                    WaveStart();
                }
                timer -= Time.deltaTime;
            }
            else if (currentWaveNPC_counter <= 0&& !levelEnded)
            {
                levelEnded = true;
                ingameUI.ShowEndLevelDialog(true);
            }
            ingameUI.UpdateExperince(experience);
            ingameUI.UpdateGold(money);
            ingameUI.UpdateMainTowerState(mainTowerLifeCount);
        }

        public void GiveMoney(int goldGiven)
        {
            money += goldGiven;
        }

        // Метод вызова волн
        public void WaveStart()
        {
            IngameUI.Instance.IncreaseWaveNumber();
            NpcWave npcWave = waves.Pop();
            waveStarted = true;
            timer = npcWave.count * npcWave.delay+20;
            currentWaveNPC_counter = npcWave.count;
            PathSystem.Instance.NPCSpawn(npcWave);
        }
        public void DecreaseNPC_Counter() {
            currentWaveNPC_counter--;
        }

        // Наносит урон главной башне
        public void DamageMainTower(int damage) {
            if (damage > mainTowerLifeCount)
                mainTowerLifeCount = 0;
            else
                mainTowerLifeCount -= damage;

            if(mainTowerLifeCount<=0)
                IngameUI.Instance.ShowEndLevelDialog(false);
        }

        public List<GameObject> GetAvaliableToBuildTowers()
        {
            return avaliableTowers;
        }
        public int GetWaveCount() { return waves.Count; }

        // Тратим деньги, возвращает false если слишком мало денег
        public bool SpendMoney(int toSpend) {
            if (money >= toSpend)
            {
                money -= toSpend;
                return true;
            }
            else
                return false;
        }
        // Увеличиваем значение опыта
        public void GiveExperience(int exp)
        {
            experience += exp;
        }
    }
}