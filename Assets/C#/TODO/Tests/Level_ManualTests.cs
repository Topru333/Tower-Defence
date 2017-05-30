using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace TD.Tests
{
    public class Level_ManualTests : MonoBehaviour
    {
        public string levelname;
        // Use this for initialization
        void Awake()
        {
            LevelManager.Instance.LevelLoad(levelname);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
