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

        private List<PlatformController> _spawnedPlatforms;
        private PlatformController _currentPlatform;

        public PlatformController LastSnappedPlatform => _spawnedPlatforms[_spawnedPlatforms.Count - 1];

        public void Initiliaze()
        {
            platformPool.Initiliaze();
            _spawnedPlatforms = new List<PlatformController>();
            for (int i = 0; i < predefinedItems.Length; i++)
            {
                var platform = predefinedItems[i];
                platform.ID = _spawnedPlatforms.Count;
                _spawnedPlatforms.Add(platform);
            }
        }

        public void SnapPlatform()
        {
            _currentPlatform.Snap();

        }

        public void SpawnPlatform()
        {
            var spawnPos = LastSnappedPlatform.transform.position + Vector3.forward * LastSnappedPlatform.Size.z;
            spawnPos.x -= 1.5f;
            var platform = platformPool.Pop();
            platform.transform.position = spawnPos;
            platform.transform.localScale = LastSnappedPlatform.transform.localScale;
            platform.ID = _spawnedPlatforms.Count;
            platform.Initiliaze();

            platform.onSnapped -= OnPlatformSnapped;
            platform.onSnapped += OnPlatformSnapped;

            _currentPlatform = platform;

        }

        private void OnPlatformSnapped(PlatformController controller)
        {
            controller.Cut(LastSnappedPlatform, out bool isGameOver);

            if (!isGameOver)
            {
                _spawnedPlatforms.Add(controller);
                player.SetCenterOfPlatform(controller.MiddleCenter.x);
                SpawnPlatform();
            }
        }

        public void ResetAllPlatforms()
        {
            platformPool.ResetAll();
            Initiliaze();
        }

        public void LoadNextLevel()
        {
            
        }

        #region Inspector Methods
        public void RepositionPredefinedPlatforms()
        {
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
            var childCount = platformPool.Parent.childCount;
            predefinedItems = new PlatformController[childCount];

            for (int i = 0; i < childCount; i++)
            {
                var platform = platformPool.Parent.GetChild(i).GetComponent<PlatformController>();
                if (platform != null)
                {
                    predefinedItems[i] = platform;
                }
            }
        }

     
        #endregion

    }
}
