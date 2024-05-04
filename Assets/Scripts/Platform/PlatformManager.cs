using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameGuru.Controls;


namespace GameGuru.SecondCase.Platform
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private PlatformController platformRes;
        [SerializeField] private PlatformController[] predefinedItems;
        [SerializeField] private Transform parent;
        [SerializeField] private InputControl inputControl;

        private List<PlatformController> _spawnedPlatforms;
        private PlatformController _currentPlatform;

        public PlatformController LastSnappedPlatform => _spawnedPlatforms[_spawnedPlatforms.Count - 1];

        private void Awake()
        {
            _spawnedPlatforms = new List<PlatformController>();
            for (int i = 0; i < predefinedItems.Length; i++)
            {
                var platform = predefinedItems[i];
                _spawnedPlatforms.Add(platform);
            }
            SpawnPlatform();
        }

        private void Update()
        {
            if (inputControl.IsDown)
            {
                _currentPlatform.Snap();
            }
        }

        public void SpawnPlatform()
        {
            var spawnPos = LastSnappedPlatform.transform.position + Vector3.forward * LastSnappedPlatform.Size.z;
            spawnPos.x -= 1.5f;
            var platform = Instantiate(platformRes, spawnPos, Quaternion.identity);
            platform.transform.parent = parent;
            platform.transform.localScale = LastSnappedPlatform.transform.localScale;
            platform.Initiliaze();

            platform.onSnapped -= OnPlatformSnapped;
            platform.onSnapped += OnPlatformSnapped;

            _currentPlatform = platform;

        }

        private void OnPlatformSnapped(PlatformController controller)
        {
            controller.Cut(LastSnappedPlatform);
            _spawnedPlatforms.Add(controller);
            SpawnPlatform();
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
            if(parent == null)
            {
                Debug.LogError("Parent is null");
                return;
            }
            var childCount = parent.childCount;
            predefinedItems = new PlatformController[childCount];

            for (int i = 0; i < childCount; i++)
            {
                var platform = parent.GetChild(i).GetComponent<PlatformController>();
                if (platform != null)
                {
                    predefinedItems[i] = platform;
                }
            }
        }
        #endregion

    }
}
