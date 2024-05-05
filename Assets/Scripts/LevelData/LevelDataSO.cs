using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameGuru.SecondCase.Level
{
    [CreateAssetMenu(fileName = "Data", menuName = "GameGuru/SecondCase/LevelData", order = 1)]
    public class LevelDataSO : ScriptableObject
    {
        public Level[] levels;


        [Serializable]
        public struct Level
        {
            public Platform.PlatformController platformRes;
            public Transform finishRes;
            public int startPlatformCount;
            public int platformCount;

        }
    }
}
