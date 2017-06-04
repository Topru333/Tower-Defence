using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TD
{
    class Utilities
    {
        public static void ReadBlock(TextReader tr, Action<string> readAction)
        {
            tr.ReadLine();
            string line = tr.ReadLine();
            while (line != "end")
            {
                readAction(line);
                line = tr.ReadLine();
            }
        }
    }
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = typeof(IngameUI).ToString();
                }
                return _instance;
            }
        }
    }
    public static class Logger
    {

        [Conditional("ENABLE_LOGS")]

        public static void Log(string logMsg)
        {
            UnityEngine.Debug.Log(logMsg);
        }

    }
}
