using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TD;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    private List<LevelData> levels = new List<LevelData>(); // Список доступных к игре уровней
    public List<LevelData> getLevels {
        get {
            return levels;
        }
    }
    // «Инициализация»
    public void Awake () {
        TextAsset levelList = Resources.Load<TextAsset>("levelslist");
        Stream levelListStream = new MemoryStream(levelList.bytes);
        using (StreamReader sr = new StreamReader(levelListStream)) {
            loadLevelIcons(sr);
        }
    }

    // Загружает данные о Уровнях из потока
    public void loadLevelIcons (StreamReader sr) {
        Utilities.ReadBlock(sr, (string _line) =>
        {
            string[] strSpl = _line.Split('#');
            levels.Add(new LevelData(strSpl[0], strSpl[1], Resources.Load<Texture2D>(strSpl[2])));
        }
        );
    }

}

[Serializable]
public struct LevelData {
    public Texture2D icon;           // Иконка уровня
    public string    name;           // Название уровня
    public string    description;    // Описание уровня

    public LevelData (string name, string description, Texture2D icon) {
        this.name = name;
        this.description = description;
        this.icon = icon;
    }
}


