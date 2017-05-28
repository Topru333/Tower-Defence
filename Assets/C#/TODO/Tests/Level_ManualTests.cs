using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace TD.Tests
{
    public class Level_ManualTests : MonoBehaviour
    {
        public string name;
        // Use this for initialization
        void Awake()
        {
            LevelManager.Instance.LevelLoad(name);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
