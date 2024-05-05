using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameGuru.Controls;
using GameGuru.SecondCase.Character;

namespace GameGuru.SecondCase.Platform
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private Pool<PlatformController> platformPool;
        [SerializeField] private PlatformController[] predefinedItems;
        [SerializeField] private PlayerController player;
        [SerializeField] private LevelConstructor levelConstructor;

        private List<PlatformController> _spawnedPlatforms;
        private PlatformController _currentPlatform;
        private int _currentLevelSpawnCount;

        public PlatformController LastSnappedPlatform => _spawnedPlatforms[_spawnedPlatforms.Count - 1];

        public void Initiliaze()
        {
            _currentLevelSpawnCount = 0;
            DestroyChildren();
            platformPool.Initiliaze();
            _spawnedPlatforms = new List<PlatformController>();

            levelConstructor.ConstructLevel(this,out List<PlatformController> platformInstanceList);
            _spawnedPlatforms.AddRange(platformInstanceList);

        }

        public void SnapPlatform()
        {
            _currentPlatform.Snap();

        }

        public void SpawnMovingPlatform()
        {
            var spawnPos = LastSnappedPlatform.transform.position + Vector3.forward * LastSnappedPlatform.Size.z;
            spawnPos.x -= 1.5f;

            PlatformController platform = SpawnPlatform(spawnPos);
            platform.Initiliaze();
            platform.transform.localScale = LastSnappedPlatform.transform.localScale;
            platform.ID = _spawnedPlatforms.Count;

            platform.onSnapped -= OnPlatformSnapped;
            platform.onSnapped += OnPlatformSnapped;

            _currentPlatform = platform;

            _currentLevelSpawnCount++;
        }

        private PlatformController SpawnPlatform(Vector3 spawnPos)
        {
            var platform = platformPool.Pop();
            platform.transform.position = spawnPos;
            return platform;
        }

        private void OnPlatformSnapped(PlatformController controller)
        {
            controller.Cut(LastSnappedPlatform, out bool isGameOver);

            if (!isGameOver)
            {
                _spawnedPlatforms.Add(controller);
                player.SetCenterOfPlatform(controller.MiddleCenter.x);
                if (_currentLevelSpawnCount >= levelConstructor.LastLevel.platformCount) return;

                SpawnMovingPlatform();
            }
        }

        public void ResetAllPlatforms()
        {
            platformPool.ResetAll();
            levelConstructor.Initiliaze(transform);
            Initiliaze();
        }

        public void LoadNextLevel()
        {
            _currentLevelSpawnCount = 0;
            levelConstructor.IncreaseLevelID();
            levelConstructor.ConstructLevel(this,out var instanceList);
            _spawnedPlatforms.AddRange(instanceList);
        }
        public void DestroyChildren()
        {
            platformPool.DestroyAll();
            var childCount = transform.childCount;
            for (int i = childCount - 1; i > -1; i--)
            {
                var child = transform.GetChild(i);
#if UNITY_EDITOR
                DestroyImmediate(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }
        }

        [Serializable]
        public struct LevelConstructor
        {
            public Level.LevelDataSO levelData;

            private Vector3 _lastFinishPos;

            public int LevelID { get; private set; }
            public Level.LevelDataSO.Level LastLevel { get; private set; }

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
                    var spawnPos = startPos +  platformSize * i;
                    currentFinishPos += platformSize;

                    var platformInstance = platformManager.SpawnPlatform(spawnPos);
                    platformInstance.transform.parent = levelObject.transform;
                    instanceList.Add(platformInstance);

                }

                currentFinishPos += platformSize * currentLevel.platformCount;

                var finishInstance = Instantiate(currentLevel.finishRes, platformManager.transform);
                finishInstance.transform.position = currentFinishPos;
                finishInstance.transform.parent = levelObject.transform;

                _lastFinishPos = currentFinishPos;

            }
            public Level.LevelDataSO.Level GetCurrentLevel()
            {
                int levelLength = levelData.levels.Length;
                bool isOverLevels = LevelID >= levelLength;
                var currentLevelID = isOverLevels ? UnityEngine.Random.Range(0, levelLength) : LevelID;
                var currentLevel = levelData.levels[currentLevelID];
                LastLevel = currentLevel;

                return currentLevel;
            }

        }

        #region Inspector Methods
        public void RepositionPredefinedPlatforms()
        {
            DestroyChildren();
            levelConstructor.Initiliaze(transform);
            for (int i = 0; i < levelConstructor.levelData.levels.Length; i++)
            {
                levelConstructor.IncreaseLevelID();
                levelConstructor.ConstructLevel(this,out var instanceList);
                //_spawnedPlatforms.AddRange(instanceList);
            }
            return;

            for (int i = 1; i < predefinedItems.Length; i++)
            {
                var currentPlatform = predefinedItems[i - 1];
                var nextPlatform = predefinedItems[i];

                var spawnPos = currentPlatform.transform.position + Vector3.forward * currentPlatform.Size.z;
                nextPlatform.transform.position = spawnPos;

            }
        }

        public void FindPredefinedPlatforms()
        {
            if (platformPool.Parent == null)
            {
                Debug.LogError("Parent is null");
                return;
            }
            var childCount = platformPool.Parent.GetChild(0).childCount;
            predefinedItems = new PlatformController[childCount];

            for (int i = 0; i < childCount; i++)
            {
                var platform = platformPool.Parent.GetChild(0).GetChild(i).GetComponent<PlatformController>();
                if (platform != null)
                {
                    predefinedItems[i] = platform;
                }
            }
        }


        #endregion

    }
}
