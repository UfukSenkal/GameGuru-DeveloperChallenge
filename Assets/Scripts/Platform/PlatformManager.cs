using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameGuru.SecondCase.Character;
using GameGuru.SecondCase.Helper;
using GameGuru.Helper;

namespace GameGuru.SecondCase.Platform
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private Pool<PlatformController> platformPool;
        [SerializeField] private PlayerController player;
        [SerializeField] private LevelConstructor levelConstructor;
        [SerializeField] private Vector2 xSpawnPoints;
        [SerializeField] private SoundEffectModule fitSoundEffect;
        [SerializeField] private Material[] stackMaterials;

        private List<PlatformController> _spawnedPlatforms;
        private PlatformController _currentPlatform;
        private int _currentLevelSpawnCount;
        private int _comboCount;
        private int _lastMaterialIndex = -1;

        public PlatformController LastSnappedPlatform => _spawnedPlatforms[_spawnedPlatforms.Count - 1];

        public void Initiliaze()
        {
            fitSoundEffect.Initiliaze();
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
            var direction = UnityEngine.Random.Range(0, 2);
            bool isGoingRight = direction == 0;
            Debug.Log("direction : " + direction);
            spawnPos.x += isGoingRight ? xSpawnPoints.x : xSpawnPoints.y;

            PlatformController platform = SpawnPlatform(spawnPos);
            platform.Initiliaze(isGoingRight, LastSnappedPlatform.Scale);
            platform.transform.parent = levelConstructor.CurrentParent;

            platform.onSnapped -= OnPlatformSnapped;
            platform.onSnapped += OnPlatformSnapped;

            platform.onPerfectFit -= OnPerfectFit;
            platform.onPerfectFit += OnPerfectFit;

            _currentPlatform = platform;

            _currentLevelSpawnCount++;
        }

        private void OnPerfectFit(bool isPerfectFit)
        {
            if (isPerfectFit)
            {
                fitSoundEffect.IncreasePitch(_comboCount);
                fitSoundEffect.PlaySound();
                _comboCount++;
                return;
            }

            _comboCount = 0;
            fitSoundEffect.Reset();
        }

        public PlatformController SpawnPlatform(Vector3 spawnPos)
        {
            var platform = platformPool.Pop();
            platform.SetMaterial(GetRandomMaterial());
            platform.transform.position = spawnPos;
            return platform;
        }
        private Material GetRandomMaterial()
        {
            var randomIndex = UnityEngine.Random.Range(0, stackMaterials.Length);

            randomIndex = randomIndex == _lastMaterialIndex ? randomIndex++ : randomIndex;
            randomIndex = randomIndex >= stackMaterials.Length ? 0 : randomIndex;

            return stackMaterials[randomIndex];
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

       

        #region Inspector Methods
        public void RepositionPredefinedPlatforms()
        {
            DestroyChildren();
            levelConstructor.Initiliaze(transform);
            for (int i = 0; i < levelConstructor.levelData.levels.Length; i++)
            {
                levelConstructor.IncreaseLevelID();
                levelConstructor.ConstructLevel(this,out var instanceList);
            }
            return;

        }

        #endregion

    }
}
