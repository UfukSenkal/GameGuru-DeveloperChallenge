using GameGuru.SecondCase.Platform;
using System.Collections.Generic;
using System;
using UnityEngine;
using  GameGuru.SecondCase.Level;

[Serializable]
public struct LevelConstructor
{
    public LevelDataSO levelData;

    private Vector3 _lastFinishPos;

    public int LevelID { get; private set; }
    public LevelDataSO.Level LastLevel { get; private set; }
    public Transform CurrentParent { get; private set; }

    public void IncreaseLevelID() => LevelID++;

    public void Initiliaze(Transform startTr)
    {
        LevelID = 0;
        _lastFinishPos = startTr.transform.position;
    }

    public void ConstructLevel(in PlatformManager platformManager, out List<PlatformController> instanceList)
    {
        var currentLevel = GetCurrentLevel();
        int startPlatformCount = currentLevel.startPlatformCount;

        var startPos = platformManager.transform.position + _lastFinishPos;
        var platformSize = Vector3.forward * currentLevel.platformRes.Size.z;
        var currentFinishPos = _lastFinishPos;

        instanceList = new List<PlatformController>();

        GameObject levelObject = new GameObject("Level " + LevelID.ToString());
        levelObject.transform.parent = platformManager.transform;
        for (int i = 0; i < startPlatformCount; i++)
        {
            var spawnPos = startPos + platformSize * i;
            currentFinishPos += platformSize;

            var platformInstance = platformManager.SpawnPlatform(spawnPos);
            platformInstance.transform.parent = levelObject.transform;
            instanceList.Add(platformInstance);

        }

        currentFinishPos += platformSize * currentLevel.platformCount;

        var finishInstance = GameObject.Instantiate(currentLevel.finishRes, platformManager.transform);
        finishInstance.transform.position = currentFinishPos;
        finishInstance.transform.parent = levelObject.transform;

        CurrentParent = levelObject.transform;
        _lastFinishPos = currentFinishPos;

    }
    public LevelDataSO.Level GetCurrentLevel()
    {
        int levelLength = levelData.levels.Length;
        bool isOverLevels = LevelID >= levelLength;
        var currentLevelID = isOverLevels ? UnityEngine.Random.Range(0, levelLength) : LevelID;
        var currentLevel = levelData.levels[currentLevelID];
        LastLevel = currentLevel;

        return currentLevel;
    }

}