using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    
    public List<GameObject> towerStore = new List<GameObject>();    // Список башен доступных для постройки
    public int pointsOnLvl = 100;                                   // Кол-во очков полученных на уровне
    public int coinsOnLvl = 100;                                    // Кол-во денег полученных на уровне
    public int mainLifes = 10;                                      // Кол-во жизней у основной башни
    public bool pause = false;                                      // Состояние игры(pause - true/inGame - false)
    public float gameSpeed = 10f;                                   // Скорость игры
    Stack<NpcWave> waves = new Stack<NpcWave>();                    // Стек списков волн

    // «Инициализация»
    public void Awake () {
       
    }

    // «Обновление»
    public void Update () {

    }

    // Метод вызова волн
    public void WaveStart () {
        NpcWave NpcWave = waves.Pop();
        PathSystem.instance.NPCSpawn(NpcWave);
    }

    // Изменение скорости игры
    public void GameSpeed (float speed) {

    }
    // Переход в режим "пауза"(и обратно)
    public void Pause () {
        if (pause) {
            pause = false;
        }
        else pause = true;

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
